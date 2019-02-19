using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Website.Filters
{
    public class AimAuthorizeAttribute : TypeFilterAttribute
    {
        public AimAuthorizeAttribute(string Roles = "") : base(typeof(AimAuthorizeFilter))
        {
            Arguments = new[] { Roles.Split(',', StringSplitOptions.RemoveEmptyEntries) };
        }
    }

    public class AimAuthorizeFilter : IAuthorizationFilter
    {
        readonly string[] Roles;

        public AimAuthorizeFilter(string[] roles)
        {
            this.Roles = roles;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if(!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new RedirectToActionResult("login", "account", null);
                return;
            }

            if(context.HttpContext.User.IsInRole("temp"))
            {
                context.Result = new RedirectToActionResult("finalise", "account", null);
                return;
            }

            if(this.Roles.Any(r => !context.HttpContext.User.IsInRole(r)))
            {
                context.Result = new RedirectToActionResult("error", "home", new { message = "You don't have the correct role to access this page." });
                return;
            }
        }
    }
}
