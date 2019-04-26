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
                    role = await this.data.SingleReference<Role>().GetOrDefaultAsync(user)
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
                Role = await this.data.SingleReference<Role>().GetOrDefaultAsync(user)
            };

            ViewData["Roles"] = this.livestockDb.Role.ToList();
            return View(info);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserEdit()
        {
            try
            {
                // Bind the model ourselves since ASP seems to suck for this specific case.
                var model = new UserEditViewModel();
                model.UserId = Convert.ToInt32(Request.Form["UserId"]);

                var user = this.loginDb.Users.Include(u => u.UserDataMaps).First(u => u.UserId == model.UserId);
                model.Info = await this.data.SingleValue<AlUserInfo>().GetOrDefaultAsync(user);
                model.Role = await this.data.SingleReference<Role>().GetOrDefaultAsync(user);

                // Modify things
                model.Info.FirstName    = Request.Form["Info.FirstName"];
                model.Info.LastName     = Request.Form["Info.LastName"];
                model.Info.EmailAddress = Request.Form["Info.EmailAddress"];
                model.Role              = this.livestockDb.Role.Find(Convert.ToInt32(Request.Form["Role.RoleId"]));
       
                await this.data.SingleValue<AlUserInfo>().SetAsync(user, model.Info);
                await this.data.SingleReference<Role>().SetAsync(user, model.Role);

                return RedirectToActionPermanent("UserList", "Admin");
            }
            catch(Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }
    }
}