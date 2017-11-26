using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Piranha.AspNetCore.Identity.EF
{
    public class IdentityAppUser : IdentityUser
    {
        public IdentityAppUser(params string[] claims) : this()
        {
            Claims.AddRange(claims);
        }

        public IdentityAppUser()
        {
            Claims = new List<string>();
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [NotMapped]
        public string Password { get; set; }

        [NotMapped]
        public List<string> Claims { get; set; }
    }
}
