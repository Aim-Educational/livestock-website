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

namespace Livestock.Controllers
{
    public class HomeController : Controller
    {
        [AimAuthorize(Roles: "superamdin")]
        [HasPermission(UserPermission.LivestockModify)]
        public string Test()
        {
            return "I smell like beef";
        }

        public IActionResult Index()
        {
            return View();
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
