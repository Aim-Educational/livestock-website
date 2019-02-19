using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Livestock.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Website.Filters;
using Website.Services;

namespace Website.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl ?? "/";
            return View();
        }

        public IActionResult Finalise()
        {
            return View();
        }

        public IActionResult TEMP_DebugFinalise([FromServices] IAccountInfoService accounts)
        {
            ((AccountInfoService)accounts).TEMP_CreateTemporaryUser((ClaimsIdentity)User.Identity);

            return Redirect("/");
        }
        
        public IActionResult Logout()
        {
            return SignOut(new AuthenticationProperties{ RedirectUri = "/" });
        }

        public IActionResult ChallengeGithub(string returnUrl = null)
        {
            if(!Url.IsLocalUrl(returnUrl))
                returnUrl = "/";

            return Challenge(new AuthenticationProperties{ RedirectUri = returnUrl ?? "/" }, "Github");
        }
    }
}