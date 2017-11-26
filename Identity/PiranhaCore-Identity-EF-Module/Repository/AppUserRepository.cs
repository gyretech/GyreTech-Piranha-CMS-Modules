using System.Linq;
using Microsoft.EntityFrameworkCore;
using Piranha.AspNetCore.Identity.EF.Extensions;

namespace Piranha.AspNetCore.Identity.EF
{
    public class AppUserRepository : EfRepository<IdentityAppUser>
    {
        public AppUserRepository(PiranhaIdentityDbContext context) : base(context)
        {
        }

        public PagedList<IdentityAppUser> FindUsersByEmail(string email, int pageIndex, int pageSize)
        {
            return Entities.AsNoTracking().Where(u => u.Email == email).OrderBy(x => x.FirstName).ToPage(pageIndex, pageSize);
        }

        public PagedList<IdentityAppUser> FindUsersByName(string name, int pageIndex, int pageSize)
        {
            return Entities.AsNoTracking().Where(u => u.UserName == name).OrderBy(x => x.UserName).ToPage(pageIndex, pageSize);
        }

        public PagedList<IdentityAppUser> GetPage(int pageIndex, int pageSize)
        {
            return Entities.AsNoTracking().OrderBy(x => x.UserName).ToPage(pageIndex, pageSize);
        }
    }
}