using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aim.DataMapper;
using AimLogin.DbModel;
using AimLogin.Misc;
using AimLogin.Services;
using Database.Models;
using Livestock.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Website.Controllers
{
    public class HomeController : Controller
    {
        readonly LivestockContext livestockDb;
        readonly AimLoginContext loginDb;
        readonly IAimUserManager userManager;
        readonly DataMapService<User> data;

        public HomeController(LivestockContext livestockDb, AimLoginContext loginDb, IAimUserManager userManager, DataMapService<User> data)
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
                var role = await this.data.SingleReference<Role>().GetOrDefaultAsync(user);
                if(role == null)
                    return RedirectToAction("Error", "Home", new { message = "You somehow don't have a role. This means something's up with the database. Yell at Andy." });

                var header = await this.livestockDb.MenuHeader
                                                   .Select(m => m)
                                                   .Include(m => m.Role)
                                                   .Include(m => m.MenuHeaderItemMap)
                                                    .ThenInclude(map => map.MenuItem)
                                                   .FirstOrDefaultAsync(m => m.RoleId == role.RoleId);

                return (header == null) ? View(null)
                                        : View(header.MenuHeaderItemMap.Select(mh => mh.MenuItem).ToList());
            }
            else
                return View(null);
        }

        public IActionResult Verify(string type)
        {
            // 'type' is a string, so it was using the wrong overload.
            return View("Verify", type);
        }

        public IActionResult Error(string message)
        {
            return View(new ErrorViewModel{ RequestId = HttpContext.TraceIdentifier, AdditionalInfo = message });
        }
    }
}