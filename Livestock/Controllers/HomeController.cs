using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Livestock.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Website.Services;
using System.Security.Claims;
using Website.Filters;
using System.Reflection;
using Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Livestock.Controllers
{
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index([FromServices] LivestockContext db)
        {
            //var header = db.MenuHeader
            //               .Include(h => h.MenuItem)
            //               .FirstOrDefault(h => HttpContext.User.IsInRole(h.Role.Description));
            MenuHeader header = null;

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                foreach (var h in db.MenuHeader.Include(h => h.Role).Where(h => h.ApplicationCode == 1)) // 1 = Main Menu
                {
                    if (HttpContext.User.IsInRole(h.Role?.Description ?? ""))
                    {
                        await db.Entry(h).Collection(h2 => h2.MenuItem).LoadAsync();

                        header = h;
                        break;
                    }
                }
            }
            else
            {
                // -1 = Not logged in.
                header = db.MenuHeader.Include(h => h.Role).FirstOrDefault(h => h.RoleId == -1 && h.ApplicationCode == 1);
                if (header != null)
                    await db.Entry(header).Collection(h => h.MenuItem).LoadAsync();
            }

            return View(header);
        }

        public IActionResult AllActions()
        {
            ViewData["Names"] = Assembly.GetExecutingAssembly()
                               .GetTypes()
                               .Where(t => typeof(Controller).IsAssignableFrom(t))
                               .Select(t => t.Name);
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(string message)
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, AdditionalInfo = message });
        }
    }
}
