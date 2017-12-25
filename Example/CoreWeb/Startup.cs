using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Piranha;
using Piranha.AspNetCore.Identity.EF.Code;
using Piranha.AspNetCore.Identity.EF.Manager;
using Piranha.Local;

namespace CoreWeb
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(config =>
            {
                config.ModelBinderProviders.Insert(0, new Piranha.Manager.Binders.AbstractModelBinderProvider());
            });

            var defaultConnection = Configuration["ConnectionStrings:DefaultConnection"];

            services.AddDbContext<Db>(options => options.UseSqlServer(defaultConnection));

            // Add application services.
            services.AddSingleton<IStorage, FileStorage>();
            services.AddScoped<IDb, Db>();
            services.AddScoped<IApi, Api>();

            services.AddPiranhaManager();

            // Add addtional all available claims
            List<string[]> additionalClaims = new List<string[]>() { Piranha.Manager.Permission.All() };

            // Add Identity Security EF
            services.AddEfIdentitySecurity(o =>
            {
                o.ConnectionString = defaultConnection;
                o.InitialClaims = additionalClaims;
                o.Users = new[]
                {
                    new Piranha.AspNetCore.Identity.EF.Data.EfIdentityUser(true, Permission.All())
                    {
                        UserName = "Admin",
                        Password = "P@sswOrd1",
                        FirstName = "Admin",
                        LastName = "User",
                    }
                };
                o.EnableFirstLastNameClaim = true;
            });

            // Add Identity Security EF Manager
            services.AddPiranhaIdentityEFManager();

            return services.BuildServiceProvider();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider services)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            // Initialize Piranha
            var api = services.GetService<IApi>();
            Piranha.App.Init(api);

            // Config
            using (var config = new Config(api))
            {
                config.CacheExpiresPages = 0;
            }

            app.UseStaticFiles();

            app.UseEfIdentitySecurity();
            app.UseEfIdentityManager();

            app.UsePiranha();
            app.UsePiranhaManager();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
