using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Website.Services;

namespace Website.Filters
{
    public class HasPermissionAttribute : TypeFilterAttribute
    {
        public HasPermissionAttribute(UserPermission permission) : base(typeof(HasPermissionFilter))
        {
            Arguments = new object[] { permission };
        }
    }

    public class HasPermissionFilter : IAuthorizationFilter
    {
        readonly UserPermission Permission;
        readonly IAccountInfoService Accounts;

        public HasPermissionFilter(UserPermission permission, IAccountInfoService accounts)
        {
            this.Permission = permission;
            this.Accounts = accounts;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var userTask = this.Accounts.GetUserByPrincipleAsync(context.HttpContext.User);
            userTask.Wait();
            var user = userTask.Result;

            if(user == null)
            {
                context.Result = new RedirectToActionResult("login", "account", null);
                return;
            }
            
            if(!this.Accounts.HasPermission(user, this.Permission))
            {
                context.Result = new RedirectToActionResult("error", "home", new { message = $"You do not have the '{this.Permission}' permission. Go yell at Andy or something." });
                return;
            }
        }
    }
}
