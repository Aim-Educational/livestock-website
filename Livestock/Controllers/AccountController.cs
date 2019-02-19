using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Livestock.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Website.Filters;

namespace Website.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Finalise()
        {
            return View();
        }

        [AimAuthorize]
        public IActionResult Logout()
        {
            return SignOut(new AuthenticationProperties{ RedirectUri = "/" });
        }

        public IActionResult ChallengeGithub()
        {
            return Challenge(new AuthenticationProperties{ RedirectUri = "/" }, "Github");
        }
    }
}