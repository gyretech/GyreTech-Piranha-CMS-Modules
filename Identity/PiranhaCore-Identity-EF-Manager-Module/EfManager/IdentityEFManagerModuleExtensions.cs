using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Piranha.AspNetCore.Identity.EF;
using Piranha.AspNetCore.Identity.EF.Code;
using Piranha.AspNetCore.Identity.EF.Manager;

namespace Piranha.AspNetCore.Identity.EF.Manager
{
    public static class IdentityEFManagerModuleExtensions
    {
        public static IServiceCollection AddEfIdentityManager(this IServiceCollection services)
        {
            var assembly = typeof(IdentityEFManagerModuleExtensions).GetTypeInfo().Assembly;
            var embeddedProvider = new EmbeddedFileProvider(assembly, "Piranha.AspNetCore.Identity.EF.Manager");

            // Add the file provider to the Razor view engine
            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.FileProviders.Add(embeddedProvider);
            });

            services.AddSingleton<IFileProvider>(embeddedProvider);

            // Add the manager module
            Piranha.App.Modules.Register<IdentityEFModule>();

            // Add session support
            services.AddSession();

            services.AddAuthorization(o =>
            {
                // User policies
                o.AddPolicy(Permission.Users, policy =>
                {
                    policy.RequireClaim(Piranha.Manager.Permission.Admin, Piranha.Manager.Permission.Admin);
                    policy.RequireClaim(Permission.Users, Permission.Users);
                });
                o.AddPolicy(Permission.UsersAdd, policy =>
                {
                    policy.RequireClaim(Permission.Users, Permission.Users);
                    policy.RequireClaim(Permission.UsersAdd, Permission.UsersAdd);
                });
                o.AddPolicy(Permission.UsersDelete, policy =>
                {
                    policy.RequireClaim(Permission.Users, Permission.Users);
                    policy.RequireClaim(Permission.UsersDelete, Permission.UsersDelete);
                });
                o.AddPolicy(Permission.UsersEdit, policy =>
                {
                    policy.RequireClaim(Permission.Users, Permission.Users);
                    policy.RequireClaim(Permission.UsersEdit, Permission.UsersEdit);
                });
                o.AddPolicy(Permission.UsersSave, policy =>
                {
                    policy.RequireClaim(Permission.Users, Permission.Users);
                    policy.RequireClaim(Permission.UsersSave, Permission.UsersSave);
                });
            });

            // Return the service collection
            return services;
        }

        /// <summary>
        /// Uses the piranha identity ef middleware
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseEfIdentityManager(this IApplicationBuilder builder)
        {
            return builder.UseSession().UseMiddleware<Piranha.AspNetCore.Identity.EF.Manager.ResourceMiddleware>();
            // return builder;
        }
    }

}