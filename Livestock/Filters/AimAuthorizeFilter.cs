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
        public AimAuthorizeAttribute(string RolesAND = "", string RolesOR = "") : base(typeof(AimAuthorizeFilter))
        {
            Arguments = new[] { RolesAND.Split(',', StringSplitOptions.RemoveEmptyEntries),
                                RolesOR.Split(',', StringSplitOptions.RemoveEmptyEntries)
                              };
        }
    }

    public class AimAuthorizeFilter : IAuthorizationFilter
    {
        readonly string[] RolesAND;
        readonly string[] RolesOR;

        public AimAuthorizeFilter(string[] rolesAND, string[] rolesOR)
        {
            this.RolesAND = rolesAND;
            this.RolesOR = rolesOR;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if(!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new RedirectToActionResult("login", "account", new { returnUrl = context.HttpContext.Request.Path.ToString() });
                return;
            }

            if(context.HttpContext.User.IsInRole("temp"))
            {
                context.Result = new RedirectToActionResult("finalise", "account", null);
                return;
            }

            if(this.RolesAND.Any(r => !context.HttpContext.User.IsInRole(r)) && this.RolesAND.Count() > 0)
            {
                context.Result = new RedirectToActionResult("error", "home", new { message = $"You require the {{{this.RolesAND.Aggregate((s1, s2) => s1 + ", " + s2)}}} roles to access this page. Yell at Andy." });
                return;
            }

            if(!this.RolesOR.Any(r => context.HttpContext.User.IsInRole(r)) && this.RolesOR.Count() > 0)
            {
                context.Result = new RedirectToActionResult("error", "home", new { message = $"You require any of the {{{this.RolesOR.Aggregate((s1, s2) => s1 + ", " + s2)}}} roles to access this page. Yell at Andy." });
                return;
            }
        }
    }
}
