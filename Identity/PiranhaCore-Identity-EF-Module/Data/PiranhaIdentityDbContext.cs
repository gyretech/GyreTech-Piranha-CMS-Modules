using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Piranha.AspNetCore.Identity.EF
{
    public class PiranhaIdentityDbContext : IdentityDbContext<IdentityAppUser>
    {
        public PiranhaIdentityDbContext(DbContextOptions<PiranhaIdentityDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<IdentityAppUser>().Ignore(t => t.Claims);
            builder.Entity<IdentityAppUser>().Ignore(t => t.Password);
            base.OnModelCreating(builder);
        }


    }
}
