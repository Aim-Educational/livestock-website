using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aim.DataMapper;
using AimLogin.DbModel;
using AimLogin.Misc;
using AimLogin.Services;
using Database.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        private CookieOptions DEFAULT_COOKIE_OPTIONS => new CookieOptions
        {
            Expires = DateTime.Now.AddHours(4),
            IsEssential = true,
            SameSite = SameSiteMode.Strict,
            Secure = true
        };

        public AccountController(LivestockContext livestockDb, AimLoginContext aimloginDb, IAimUserManager aimloginUsers, DataMapService<User> aimLoginData)
        {
            this.livestockDb = livestockDb;
            this.aimLoginData = aimLoginData;
            this.aimloginDb = aimloginDb;
            this.aimloginUsers = aimloginUsers;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Signup()
        {
            return View();
        }

        public IActionResult Login([FromQuery] string ReturnUrl)
        {
            ViewData["ReturnUrl"] = ReturnUrl;
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
                        user = await this.aimloginUsers.CreateNewUser(model.Username, model.Password);
                        if (user == null)
                            throw new Exception($"Could not create new user.");
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
                            EmailAddress = model.EmailAddress,
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

                    // Sign them in.
                    await HttpContext.AimSignInAsync(user, this.aimloginUsers);

                    liveTransact.Commit();
                    aimTransact.Commit();
                }
                return RedirectToActionPermanent("Index", "Home");
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
            return RedirectToActionPermanent("Index", "Home");
        }
    }
}