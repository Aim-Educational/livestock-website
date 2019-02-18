using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Database.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.OAuth;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using Website.Services;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Globalization;

namespace Livestock
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            // I want all the generated code stuffed into a folder so it's not cluttering up my screen.
            services.Configure<RazorViewEngineOptions>(options =>
            {
                // {2} is area, {1} is controller,{0} is the action 
                options.ViewLocationFormats.Add("/Views/Generated/{1}/{0}" + RazorViewEngine.ViewExtension);
            });

            // This is here so the MVC binder uses British date format, instead of American.
            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("en-GB");
                options.SupportedCultures = new List<CultureInfo> { new CultureInfo("en-US"), new CultureInfo("en-GB") };
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddDbContext<LivestockContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Livestock")));
            services.AddScoped<IAccountInfoService, AccountInfoService>();

            var accounts = services.BuildServiceProvider().GetService<IAccountInfoService>();
            services.AddAuthentication(options => 
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme       = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme    = "Github";
            })
            .AddCookie()
            .AddOAuth("Github", options =>
            {
                options.ClientId     = Configuration["LIVESTOCK_OAUTH_GITHUB_ID"];
                options.ClientSecret = Configuration["LIVESTOCK_OAUTH_GITHUB_SECRET"];
                options.CallbackPath = new PathString("/oauth-github");

                options.AuthorizationEndpoint   = "https://github.com/login/oauth/authorize";
                options.TokenEndpoint           = "https://github.com/login/oauth/access_token";
                options.UserInformationEndpoint = "https://api.github.com/user";

                options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "email");

                options.Events = new OAuthEvents
                {
                    OnCreatingTicket = async context =>
                    {
                        var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
                        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);

                        var response = await context.Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, context.HttpContext.RequestAborted);
                        response.EnsureSuccessStatusCode();

                        var user = JObject.Parse(await response.Content.ReadAsStringAsync());
                        context.RunClaimActions(user);

                        if(user["email"].Type == JTokenType.Null)
                            throw new Exception("No public email is associated with this account.");
                        
                        var userInfo = await accounts.GetUserByEmailAsync(user["email"].Value<string>());
                        if(userInfo != null)
                            accounts.AddClaimsFor(userInfo, context.Identity);
                        else
                            accounts.AddTemporaryUserClaims(context.Identity);
                    }
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseRequestLocalization();
            app.UseHttpsRedirection();
            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                  );
            });
        }
    }
}
