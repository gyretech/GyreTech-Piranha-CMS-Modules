using Microsoft.EntityFrameworkCore;

namespace Piranha.AspNetCore.Identity.EF
{
    public static class IdentityDbInitializer
    {
        public static void Initialize(PiranhaIdentityDbContext context)
        {
            context.Database.Migrate();
        }
    }
}
