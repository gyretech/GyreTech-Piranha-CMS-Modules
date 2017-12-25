using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Threading.Tasks;
using Piranha.AspNetCore.Identity.EF.Data;

namespace Piranha.AspNetCore.Identity.EF.Code
{
    class EfIdentityAppClaimsPrincipalFactory : UserClaimsPrincipalFactory<EfIdentityUser, IdentityRole>
    {
        public EfIdentityAppClaimsPrincipalFactory(UserManager<EfIdentityUser> userManager, RoleManager<Microsoft.AspNetCore.Identity.IdentityRole> roleManager, IOptions<IdentityOptions> options) : base(userManager, roleManager, options)
        {
        }

        public override async Task<ClaimsPrincipal> CreateAsync(EfIdentityUser user)
        {
            var principal = await base.CreateAsync(user);

            // Only do this if the user's first and last names are provided
            if (user.FirstName != null && user.LastName != null)
            {
                ((ClaimsIdentity)principal.Identity).AddClaims(new[] {
                    new Claim(ClaimTypes.GivenName, user.FirstName),
                    new Claim(ClaimTypes.Surname, user.LastName),
                });
            }

            return principal;
        }

    }
}
