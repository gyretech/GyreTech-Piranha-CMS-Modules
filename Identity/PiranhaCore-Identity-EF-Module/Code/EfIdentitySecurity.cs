using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Piranha.AspNetCore.Identity.EF.Data;

namespace Piranha.AspNetCore.Identity.EF.Code
{
    public class EfIdentitySecurity : ISecurity
    {
        private readonly SignInManager<EfIdentityUser> _signInManager;
        private readonly UserManager<EfIdentityUser> _userManager;
        // Default Contructor
        public EfIdentitySecurity(Action<EfIdentityDbBuilder> options, SignInManager<EfIdentityUser> signInManager, UserManager<EfIdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;

            var config = new EfIdentityDbBuilder();
            options?.Invoke(config);

            foreach (var initialClaim in config.InitialClaims)
            {
                foreach (var claim in initialClaim)
                {
                    if(!Permission.Registered.Any(x => x.Equals(claim)))
                        Permission.Registered.Add(claim);
                }
            }
            

            if (!config.Users.Any()) return;

            if (!config.Users.Any(u => u.IsDefault) || config.Users.Count(u => u.IsDefault) > 1)
            {

                // logger.LogError("You must have one Default User!");

                return;
            }

            foreach (var appUser in config.Users)
            {
                List<Claim> claims =  new List<Claim>();

                // Setup default user with all claims
                if (appUser.IsDefault)
                {
                    claims.AddRange(Permission.Registered.Select(claim => new Claim(claim, claim)));

                    claims.Clear();
                }
                

                if (appUser.Claims.Any())
                {
                    claims.AddRange(appUser.Claims.Select(claim => new Claim(claim, claim)));
                }

                var user = _userManager.FindByNameAsync(appUser.UserName).Result;

                if (user != null) continue;

                var created = _userManager.CreateAsync(appUser, appUser.Password).Result.Succeeded;

                if (created)
                {
                    var unused = _userManager.AddClaimsAsync(appUser, claims).Result.Succeeded;
                }
            }
        }


        public bool Authenticate(string username, string password)
        {
            var user = _userManager.FindByNameAsync(username).Result;

            if (user == null) return false;

            var result = _signInManager.CheckPasswordSignInAsync(user, password, false).Result;

            return result.IsNotAllowed;
        }

       

        public async Task<bool> SignIn(object context, string username, string password)
        {
            if (!(context is HttpContext)) return false;
            await _signInManager.SignOutAsync();

            var result = await _signInManager.PasswordSignInAsync(username, password, false, false);

            if (!result.Succeeded) return false;

            var user = ((HttpContext)context).User;

            await ((HttpContext) context).SignInAsync(user);

            return result.Succeeded;
        }

        public async Task SignOut(object context)
        {
            if (context is DefaultHttpContext httpContext)
            {
                await _signInManager.SignOutAsync();

                await httpContext.SignOutAsync();
            }
        }

        public void SeedUser(IServiceProvider services, string userName, string password, string[] all)
        {
            using (var scope = services.CreateScope())
            {
                var svcs = scope.ServiceProvider;
                try
                {
                    List<Claim> claims = all.Select(claim => new Claim(claim, claim)).ToList();

                    if (!claims.Any()) return;

                    var userManager = svcs.GetRequiredService<UserManager<EfIdentityUser>>();

                    if (userManager == null) return;

                    var user = userManager.FindByNameAsync(userName).Result ?? new EfIdentityUser() { UserName = userName };

                    var result = userManager.CreateAsync(user, password).Result.Succeeded;

                    if (result)
                    {
                        var unused = userManager.AddClaimsAsync(user, claims).Result.Succeeded;
                    }
                    
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger>();
                    logger?.LogError(ex, "Error occured.");

                    throw;
                }
            }
        }

        
    }
}