using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Piranha.AspNetCore.Identity.EF.Data
{
    public class EfIdentityUser : IdentityUser
    {
        public EfIdentityUser(bool defaultUser, params string[] claims) : this()
        {
            IsDefault = defaultUser;
            Claims.AddRange(claims);
        }

        public EfIdentityUser()
        {
            Claims = new List<string>();
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [NotMapped]
        public string Password { get; set; }

        [NotMapped]
        public List<string> Claims { get; set; }

        [NotMapped]
        public bool IsDefault { get; set; }
    }
}
