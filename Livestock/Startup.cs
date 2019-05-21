using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
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
using Microsoft.AspNetCore.Mvc.Razor;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.HttpOverrides;
using AimLogin.Middleware;
using AimLogin.DbModel;
using AimLogin.Services;
using Website.Other;
using AimLogin.Misc;
using System.IO;
using System.Text;
using Aim.DataMapper;

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
                options.DefaultRequestCulture = new RequestCulture("en-GB");
                options.SupportedCultures = new List<CultureInfo> { new CultureInfo("en-US"), new CultureInfo("en-GB") };
            });

            // Setup AimLogin
            services.Configure<AimLoginMiddlewareConfig>(c =>
            {
                // When the user is logged in, set their Name and Role claims.
                c.OnAddUserClaims.Add(async args =>
                {
                    var claims = new List<Claim>();

                    var info = await args.dataMap.SingleValue<AlUserInfo>().GetOrDefaultAsync(args.userDb);
                    claims.Add(new Claim(ClaimTypes.Name, $"{info.FirstName} {info.LastName}"));

                    var role = await args.dataMap.SingleReference<Role>().GetOrDefaultAsync(args.userDb);
                    if(role != null)
                        claims.Add(new Claim(ClaimTypes.Role, role.Description));

                    // Get a profile pic from Gravatar
                    using (var md5 = MD5.Create())
                    {
                        var email           = await args.dataMap.SingleValue<UserEmail>().GetOrDefaultAsync(args.userDb);
                        var emailFormatted  = email.Email.Trim().ToLower();
                        var emailBytes      = md5.ComputeHash(Encoding.UTF8.GetBytes(emailFormatted));
                        var emailHex        = emailBytes.Select(b => b.ToString("x2"));
                        var emailHexString  = emailHex.Aggregate((s1, s2) => $"{s1}{s2}");
                        claims.Add(new Claim(LivestockClaims.ProfileImage,
                            $"https://s.gravatar.com/avatar/{emailHexString}"
                        ));
                    }

                    args.userPrincipal.AddIdentity(new ClaimsIdentity(claims));
                });
            });

            services.Configure<IAimSmtpClientConfig>(c =>
            {
                c.Host = Configuration.GetValue<string>("Smtp:Host");
                c.Port = Configuration.GetValue<int>("Smtp:Port");
                c.Credentials = new System.Net.NetworkCredential
                {
                    UserName = Configuration.GetValue<string>("Smtp:Username"),
                    Password = Configuration.GetValue<string>("Smtp:Password")
                };
            });

            services.Configure<IAimSmtpDomainConfig>(c =>
            {
                // This will have to be hard coded for now.
                c.VerifyEmailDomain = "https://livestock.chatha.dev/Account/VerifyEmail?token=";
                c.VerifyPasswordChangeDomain = "https://livestock.chatha.dev/Account/ChangePasswordVerify?token=";
            });

            services.Configure<IAimPasswordSchemeConfig>(c => 
            {
                c.ValidateFuncs.Add(pass => (pass.Length > 8)                   ? null : "Passwords must be at least 8 characters long");
                c.ValidateFuncs.Add(pass => pass.Any(p => char.IsLetter(p))     ? null : "Passwords must contain at least a single letter");
                c.ValidateFuncs.Add(pass => pass.Any(p => char.IsDigit(p))      ? null : "Passwords must contain at least one number");
                c.ValidateFuncs.Add(pass => pass.Any(p => char.IsSymbol(p))
                                         || pass.Any(p => char.IsWhiteSpace(p)) ? null : "Passwords must contain at least one symbol ('_', '@', etc.) or space. Blame Andy"
                );
            });

            services.AddDbContext<AimLoginContext>(o => o.UseSqlServer(Configuration.GetConnectionString("AimLogin")));
            services.AddAimLogin();

            // Setup the data mapper
            new DataMapBuilder<AimLoginContext, LivestockContext, UserDataMap, LivestockEntityTypes>(services)
                .UseSingleReference<User, Role>()
                .UseSingleValue<User, AlUserInfo>();

            // Setup Misc
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddDbContext<LivestockContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Livestock")));
            services.AddHttpContextAccessor();

            // Setup Auth
            services.AddAuthentication(options => 
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme       = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme    = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.LoginPath = "/Account/Login";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, AimLoginContext loginDb)
        {
            //loginDb.Database.EnsureCreated();
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

            // For nginx
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseRequestLocalization();
            app.UseHttpsRedirection();
            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseMiddleware<AimLoginMiddleware>();

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
