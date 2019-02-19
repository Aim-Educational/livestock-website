using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Website.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Website.Middleware
{
    public class AuthenticatedUserClaimsMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthenticatedUserClaimsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            if(httpContext.User.Identity.IsAuthenticated)
            {
                var accounts = httpContext.RequestServices.GetService<IAccountInfoService>();
                var identity = httpContext.User.Identity as ClaimsIdentity;
                if(identity == null)
                    throw new Exception("Identity is somehow null");

                var userInfo = await accounts.GetUserByEmailAsync(identity.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null);
                if (userInfo != null)
                    accounts.AddClaimsFor(userInfo, identity);
                else
                    accounts.AddTemporaryUserClaims(identity);
            }

            await _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class AuthenticatedUserClaimsMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthenticatedUserClaimsMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthenticatedUserClaimsMiddleware>();
        }
    }
}
