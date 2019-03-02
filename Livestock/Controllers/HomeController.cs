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
        public IActionResult Index([FromServices] LivestockContext db)
        {
            //var header = db.MenuHeader
            //               .Include(h => h.MenuItem)
            //               .FirstOrDefault(h => HttpContext.User.IsInRole(h.Role.Description));
            MenuHeader header = null;
            foreach (var h in db.MenuHeader.Include(h => h.Role))
            {
                if(HttpContext.User.IsInRole(h.Role?.Description ?? ""))
                {
                    db.Entry(h).Collection(h2 => h2.MenuItem).Load();

                    header = h;
                    break;
                }
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
