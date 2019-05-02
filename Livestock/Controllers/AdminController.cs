using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aim.DataMapper;
using AimLogin.DbModel;
using AimLogin.Services;
using Database.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Website.Models;
using Website.Other;

namespace Website.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        readonly AimLoginContext loginDb;
        readonly LivestockContext livestockDb;
        readonly DataMapService<User> data;

        public AdminController(AimLoginContext loginDb, LivestockContext livestockDb, DataMapService<User> data)
        {
            this.loginDb = loginDb;
            this.livestockDb = livestockDb;
            this.data = data;
        }

        public IActionResult Claims()
        {
            return View();
        }

        public async Task<IActionResult> UserList()
        {
            var info = new List<UserListViewModel>();
            foreach(var user in this.loginDb.Users.Include(u => u.UserDataMaps))
            {
                info.Add(new UserListViewModel
                {
                    user = user,
                    info = await this.data.SingleValue<AlUserInfo>().GetOrDefaultAsync(user),
                    role = await this.data.SingleReference<Role>().GetOrDefaultAsync(user),
                    email = await this.data.SingleValue<UserEmail>().GetOrDefaultAsync(user)
                });
            }
            return View(info);
        }

        public async Task<IActionResult> UserEdit([FromRoute] int id)
        {
            var user = this.loginDb.Users.Include(u => u.UserDataMaps).First(u => u.UserId == id);
            var info = new UserEditViewModel
            {
                UserId = id,
                Info = await this.data.SingleValue<AlUserInfo>().GetOrDefaultAsync(user),
                Role = await this.data.SingleReference<Role>().GetOrDefaultAsync(user),
                Email = await this.data.SingleValue<UserEmail>().GetOrDefaultAsync(user)
            };

            ViewData["Roles"] = this.livestockDb.Role.ToList();
            return View(info);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserEdit([Bind("UserId,Info,Role,Email")] UserEditViewModel model)
        {
            try
            {
                // Get the db versions of the information.
                var user  = this.loginDb.Users.Include(u => u.UserDataMaps).First(u => u.UserId == model.UserId);
                var info  = await this.data.SingleValue<AlUserInfo>().GetOrDefaultAsync(user);
                var email = await this.data.SingleValue<UserEmail>().GetOrDefaultAsync(user, new Lazy<UserEmail>(new UserEmail()));
                var role  = this.livestockDb.Role.Find(model.Role.RoleId);

                // Update the db information with the model information.
                info.FirstName    = model.Info.FirstName;
                info.LastName     = model.Info.LastName;
                email.Email       = model.Email.Email;
       
                // Update the mappings.
                await this.data.SingleValue<AlUserInfo>().SetAsync(user, info);
                await this.data.SingleReference<Role>().SetAsync(user, role);
                await this.data.SingleValue<UserEmail>().SetAsync(user, email);

                return RedirectToActionPermanent("UserList", "Admin");
            }
            catch(Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewData["Roles"] = this.livestockDb.Role.ToList();
                return View(model);
            }
        }
    }
}