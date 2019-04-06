﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AimLogin.DbModel;
using AimLogin.Misc;
using AimLogin.Services;
using Database.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Website.Controllers
{
    public class HomeController : Controller
    {
        readonly LivestockContext livestockDb;
        readonly AimLoginContext loginDb;
        readonly IAimUserManager userManager;
        readonly IAimUserDataMapManager data;

        public HomeController(LivestockContext livestockDb, AimLoginContext loginDb, IAimUserManager userManager, IAimUserDataMapManager data)
        {
            this.livestockDb = livestockDb;
            this.loginDb = loginDb;
            this.userManager = userManager;
            this.data = data;
        }

        public async Task<IActionResult> Index()
        {
            // TODO: Fix AimUserManager.PrincipalToUser
            if(User.Identity.IsAuthenticated)
            {
                var user = await this.loginDb.Users.FirstAsync(u => u.UserId == Convert.ToInt32(User.FindFirst(AimLoginClaims.UserId).Value));
                var role = await this.data.FetchFirstFor<Role>(user);
                var header = await this.livestockDb.MenuHeader
                                                   .Select(m => m)
                                                   .Include(m => m.Role)
                                                   .Include(m => m.MenuHeaderItemMap)
                                                    .ThenInclude(map => map.MenuItem)
                                                   .FirstOrDefaultAsync(m => m.RoleId == role.RoleId);

                return View(header.MenuHeaderItemMap.Select(mh => mh.MenuItem).ToList());
            }
            else
                return View(null);
        }
    }
}