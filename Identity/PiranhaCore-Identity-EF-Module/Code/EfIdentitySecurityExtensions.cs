using System;
using System.Linq;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Piranha.AspNetCore.Identity.EF.Data;

namespace Piranha.AspNetCore.Identity.EF.Code
{
    public static class EfIdentitySecurityExtensions
    {
        public static IServiceCollection AddEfIdentitySecurity(this IServiceCollection services, Action<EfIdentityDbBuilder> builder)
        {
            services.AddSingleton<Action<EfIdentityDbBuilder>>(builder);

            var config = new EfIdentityDbBuilder();
            builder?.Invoke(config);

            if (config.InitialClaims.Any())
            {
                foreach (var initialClaim in config.InitialClaims)
                {
                    foreach (var claim in initialClaim)
                    {
                        Permission.Registered.Add(claim);
                    }
                }
            }

            services.AddDbContext<EfIdentityDbContext>(options =>
                options.UseSqlServer(config.ConnectionString));
            
            services.AddIdentity<EfIdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<EfIdentityDbContext>()
                .AddDefaultTokenProviders();

            if (config.EnableFirstLastNameClaim)
                services.AddScoped<IUserClaimsPrincipalFactory<EfIdentityUser>, EfIdentityAppClaimsPrincipalFactory>();

            if (config.IdentityOptions != null)
            {
                services.Configure(ConfgureEfIdentityOptions(config.IdentityOptions));
            }

            if (config.CookieAuthenticationOptions != null)
            {
                services.Configure(ConfigureCookieOptions(config.CookieAuthenticationOptions));
            }
            else
            {
                // Set some defaults that I like
                services.ConfigureApplicationCookie(options =>
                {
                    options.Cookie.HttpOnly = true;
                    options.Cookie.Expiration = TimeSpan.FromDays(100);
                    options.LoginPath ="/Manager/Login";
                    options.LogoutPath = "/Manager/Logout";
                });
            }


            return services.AddSingleton<ISecurity,EfIdentitySecurity>();
        }


        #region Helpers

        private static Action<IdentityOptions> ConfgureEfIdentityOptions(IdentityOptions userOptions)
        {
            return options =>
            {
                options.Password.RequireDigit = userOptions.Password.RequireDigit;
                options.Password.RequiredLength = userOptions.Password.RequiredLength;
                options.Password.RequireNonAlphanumeric = userOptions.Password.RequireNonAlphanumeric;
                options.Password.RequireUppercase = userOptions.Password.RequireUppercase;
                options.Password.RequireLowercase = userOptions.Password.RequireLowercase;
                options.Password.RequiredUniqueChars = userOptions.Password.RequiredUniqueChars;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = userOptions.Lockout.DefaultLockoutTimeSpan;
                options.Lockout.MaxFailedAccessAttempts = userOptions.Lockout.MaxFailedAccessAttempts;
                options.Lockout.AllowedForNewUsers = userOptions.Lockout.AllowedForNewUsers;

                // User settings
                options.User.RequireUniqueEmail = userOptions.User.RequireUniqueEmail;
            };
        }

        private static Action<CookieAuthenticationOptions> ConfigureCookieOptions(CookieAuthenticationOptions userOptions)
        {
            return options =>
            {
                options.Cookie.HttpOnly = userOptions.Cookie.HttpOnly;
                options.Cookie.Expiration = userOptions.Cookie.Expiration;
                options.LoginPath = userOptions.LoginPath;
                options.LogoutPath = userOptions.LogoutPath;
            };
        }
        
        #endregion


        public static IApplicationBuilder UseEfIdentitySecurity(this IApplicationBuilder builder)
        {
            return builder.UseAuthentication();
        }

      
    }
}