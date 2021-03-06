﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Aim.DataMapper;
using AimLogin.DbModel;
using AimLogin.Misc;
using AimLogin.Services;
using Database.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Website.Models;
using Website.Other;

namespace Website.Controllers
{
    public class AccountController : Controller
    {
        readonly LivestockContext livestockDb;
        readonly AimLoginContext aimloginDb;
        readonly IAimUserManager aimloginUsers;
        readonly DataMapService<User> aimLoginData;
        readonly IAimHasher aimHasher;

        private CookieOptions DEFAULT_COOKIE_OPTIONS => new CookieOptions
        {
            Expires = DateTime.Now.AddHours(4),
            IsEssential = true,
            SameSite = SameSiteMode.Strict,
            Secure = true
        };

        public AccountController(LivestockContext livestockDb, AimLoginContext aimloginDb, IAimUserManager aimloginUsers, DataMapService<User> aimLoginData, IAimHasher aimHasher)
        {
            this.livestockDb = livestockDb;
            this.aimLoginData = aimLoginData;
            this.aimloginDb = aimloginDb;
            this.aimloginUsers = aimloginUsers;
            this.aimHasher = aimHasher;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Signup()
        {
            return View();
        }

        public IActionResult ResetPassForm()
        {
            return View();
        }

        [Authorize(Roles = "student,staff,admin")]
        public IActionResult Profile()
        {
            return View();
        }

        public IActionResult Login([FromQuery] string ReturnUrl)
        {
            ViewData["ReturnUrl"] = ReturnUrl;
            return View();
        }

        [Authorize(Roles = "student,staff,admin")]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if(model.ReturnUrl == null || !Url.IsLocalUrl(model.ReturnUrl))
                model.ReturnUrl = "/";

            if (!ModelState.IsValid)
                return View(model);

            User user;
            try
            {
                user = await this.aimloginUsers.GetUserByLogin(model.Username, model.Password);

                if (user == null)
                    throw new Exception("Internal error: User object is null, yet GetUserByLogin passed.");
            }
            catch (EmailNotVerifiedException)
            {
                return RedirectToAction("VerifyEmail", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }

            await HttpContext.AimSignInAsync(user, this.aimloginUsers);
            return Redirect(model.ReturnUrl);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Signup(SignupViewModel model)
        {
            if(ModelState.IsValid)
            {
                using (var aimTransact = await this.aimloginDb.Database.BeginTransactionAsync())
                using (var liveTransact = await this.livestockDb.Database.BeginTransactionAsync())
                {
                    // Create the user, showing any error to them if one happens.
                    User user = null;
                    try
                    {
                        user = await this.aimloginUsers.CreateNewUser(model.Username, model.Password, model.EmailAddress);
                        if (user == null)
                            throw new Exception($"Could not create new user.");
                    }
                    catch(PasswordValidationException ex)
                    {
                        var messages = ex.Message.Split('-', StringSplitOptions.RemoveEmptyEntries);
                        foreach(var msg in messages)
                            ModelState.AddModelError(nameof(model.Password), msg);

                        return View(model);
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError(nameof(model.Username), ex.Message);
                        return View(model);
                    }

                    // Create their user info.
                    await this.aimLoginData.SingleValue<AlUserInfo>().SetAsync(
                        user,
                        new AlUserInfo
                        {
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            PrivacyConsent = model.PrivacyConsent,
                            TosConsent = model.ToSConsent,
                            Comment = "N/A"
                        }
                    );

                    // If they're the first user, set them as an admin so there's at least one admin account.
                    if (this.aimloginDb.Users.Count() == 1)
                        await this.aimLoginData.SetRoleFor(user, RoleEnum.Admin, this.livestockDb);
                    else
                        await this.aimLoginData.SetRoleFor(user, RoleEnum.Temp, this.livestockDb);

                    liveTransact.Commit();
                    aimTransact.Commit();
                }
                return RedirectToAction("Verify", "Home", new { type = "account" });
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            var cookie = new AimLoginCookie(HttpContext);
            cookie.SessionToken = "";
            cookie.SaveChanges(DEFAULT_COOKIE_OPTIONS);

            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "student,staff,admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(AccountChangePasswordViewModel model)
        {
            if(ModelState.IsValid)
            {
                var userId = Convert.ToInt32(HttpContext.User.FindFirstValue(AimLoginClaims.UserId));
                var result = await this.ChangePasswordForUser(userId, model.OldPassword, model.Password);
                
                return result ?? View(model);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassForm(AccountResetPasswordViewModel model)
        {
            if(!ModelState.IsValid)
                return View(model);

            var email = await this.aimloginDb.UserEmail.SingleOrDefaultAsync(e => e.Email == model.Email);
            if(email == null)
            {
                ModelState.AddModelError(nameof(model.Email), "The given email does not exist.");
                return View(model);
            }

            var userId = await this.aimLoginData.SingleValue<UserEmail>().ReverseLookupUserId(email);
            if(userId == null)
            {
                ModelState.AddModelError(nameof(model.Email), "Internal Server Error: No userId could be found in a reverse lookup.");
            }

            var result = await this.ChangePasswordForUser(userId.Value, null, model.Password);
            return result ?? View(model);
        }

        #region Common Operations
        public async Task<IActionResult> ChangePasswordForUser(int userId, string oldPass, string newPass)
        {
            var user = await this.aimloginDb.Users.FindAsync(userId);
            var loginInfo = await this.aimLoginData.SingleValue<UserLoginInfo>().GetOrDefaultAsync(user);

            // Please mr security auditor, don't eat me.
            if(oldPass != null)
            {
                var passHash = await this.aimHasher.HashWithSalt(oldPass, loginInfo.Salt);
                if (!passHash.SequenceEqual(loginInfo.PassHash))
                {
                    ModelState.AddModelError("OldPassword", "The current password is incorrect.");
                    return null;
                }
            }

            try
            {
                await this.aimloginUsers.ChangeUserPassword(user, newPass);
            }
            catch (PasswordValidationException ex)
            {
                ModelState.AddModelError("Password", ex.Message);
                return null;
            }

            return RedirectToAction("Verify", "Home", new { type = "changepass" });
        }
        #endregion

        #region Callbacks
        public async Task<IActionResult> VerifyEmail(string token)
        {
            var email = await this.aimloginDb.UserEmail.FirstOrDefaultAsync(e => e.VerifyToken == token);
            if(email == null)
                throw new Exception($"No email exists for verification token {token}.");

            var userId   = await this.aimLoginData.SingleValue<UserEmail>().ReverseLookupUserId(email);
            var user     = await this.aimloginDb.Users.FindAsync(userId);
            var userInfo = await this.aimLoginData.SingleValue<UserLoginInfo>().GetOrDefaultAsync(user);
            
            if(userInfo.HasEmailBeenVerified)
                return RedirectToAction("Index", "Home");

            userInfo.HasEmailBeenVerified = true;
            await this.aimLoginData.SingleValue<UserLoginInfo>().SetAsync(user, userInfo);
            await HttpContext.AimSignInAsync(user, this.aimloginUsers);

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> ChangePasswordVerify(string token)
        {
            await this.aimloginUsers.FinishChangeUserPassword(token);
            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region AJAX
        [Authorize(Roles = "staff,admin")]
        [HttpPost]
        public IActionResult GetUsersFilteredValueDescription([FromBody] UserAJAXFilter model)
        {
            var userInfo = this.aimLoginData.SingleValue<AlUserInfo>();
            // Slightly inefficient, but even when we reach 100 users (which is very far off) this won't really matter.
            return Json(this.aimloginDb.Users
                                       .ToList()
                                       .Select(u => 
                                       {
                                           var info = userInfo.GetOrDefaultAsync(u).Result;
                                           return new { value = u.UserId, description = $"{info.FirstName} {info.LastName}" };
                                       })
                                       .Where(vd => String.IsNullOrWhiteSpace(model.NamesRegex)
                                                 || Regex.IsMatch(vd.description, model.NamesRegex, RegexOptions.IgnoreCase))
                                       .OrderBy(vd => vd.description)
                       );
        }
        #endregion
    }

    public class UserAJAXFilter
    {
        public string NamesRegex { get; set; }
    }
}