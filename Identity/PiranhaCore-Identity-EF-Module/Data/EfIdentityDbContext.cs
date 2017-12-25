using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Expressions;
using Piranha.AspNetCore.Identity.EF.Code;

namespace Piranha.AspNetCore.Identity.EF.Data
{
    public sealed class EfIdentityDbContext : IdentityDbContext<EfIdentityUser>
    {
        static bool _isInitialized = false;

        static readonly object Mutex = new object();
        

        public EfIdentityDbContext(DbContextOptions<EfIdentityDbContext> options, Action<EfIdentityDbBuilder> builder) : base(options)
        {
            var config = new EfIdentityDbBuilder();
            builder?.Invoke(config);

            if (_isInitialized) return;

            lock (Mutex)
            {
                if (_isInitialized || !config.Migrate) return;

                Database.Migrate();

                // Seed if Any
                Seed();

                // Set Initialized
                _isInitialized = true;
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<EfIdentityUser>().Ignore(t => t.Claims);
            builder.Entity<EfIdentityUser>().Ignore(t => t.Password);
            builder.Entity<EfIdentityUser>().Ignore(t => t.IsDefault);
            base.OnModelCreating(builder);
        }

        private void Seed()
        {
            SaveChanges();

            // Do some stuff here
        }
    }
}
