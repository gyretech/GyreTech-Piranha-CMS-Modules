using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Piranha.AspNetCore.Identity.EF
{
    class IdentityAppClaimsPrincipalFactory : UserClaimsPrincipalFactory<IdentityAppUser, IdentityRole>
    {
        public IdentityAppClaimsPrincipalFactory(UserManager<IdentityAppUser> userManager, RoleManager<Microsoft.AspNetCore.Identity.IdentityRole> roleManager, IOptions<IdentityOptions> options) : base(userManager, roleManager, options)
        {
        }

        public override async Task<ClaimsPrincipal> CreateAsync(IdentityAppUser user)
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
