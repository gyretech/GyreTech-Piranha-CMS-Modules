using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Piranha.AspNetCore.Identity.EF.Data;

namespace Piranha.AspNetCore.Identity.EF.Code
{
    public sealed class EfIdentityDbBuilder
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public EfIdentityDbBuilder()
        {
            Migrate = true;
        }

        // public string[] AddClaimPermissions { get; get; }

        /// <summary>
        /// Gets/sets the connection string.
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Gets/sets if the database should be updated
        /// to latest migration automatically.
        /// </summary>
        public bool Migrate { get; set; }

        /// <summary>
        /// Gets/set if we should enable First Name and Last Name Claims
        /// </summary>
        public bool EnableFirstLastNameClaim { get; set; }


        /// <summary>
        /// Gets/sets the optional Identity options.
        /// </summary>
        public IdentityOptions IdentityOptions { get; set; }

        /// <summary>
        /// Gets/sets the optional Cookie options
        /// </summary>
        public CookieAuthenticationOptions CookieAuthenticationOptions { get; set; }

        /// <summary>
        /// Gets/sets the initial users to create
        /// </summary>
        public EfIdentityUser[] Users { get; set; }

        public List<string[]> InitialClaims { get; set; }
    }
}
