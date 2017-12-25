using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Piranha.AspNetCore.Identity.EF.Code;
using Piranha.AspNetCore.Identity.EF.Data;
using Piranha.AspNetCore.Identity.EF.Manager.Validation;

namespace Piranha.AspNetCore.Identity.EF.Manager.Areas.Manager.Models
{
    public class UserEditModel
    {
        #region Properties

        public string Id { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [StringLength(100)]
        public string LastName { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z0-9]{3,100}$", ErrorMessage = "Username must be at least 3 characters and max 100 characters long. <br/>Also it only allows alpha and numbers.")]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Display(Name = "Phone No")]
        [StringLength(20)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Please enter valid email address..")]
        [EmailAddress(ErrorMessage = "Please enter valid email address..")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email address")]
        [StringLength(200)]
        public string Email { get; set; }

        [RequiredIfTrue("ChangePassword", ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string PasswordConfirm { get; set; }

        public IList<string> UserClaims { get; set; }

        public List<KeyValuePair<string, string>> AvailableClaims => Permission.Registered
            .Select(x => new KeyValuePair<string, string>(Regex.Replace(x, "(\\B[A-Z])", " $1"), x)).ToList();

        public bool ChangePassword { get; set; }

        #endregion

        public static async Task<UserEditModel> GetById(UserManager<EfIdentityUser> userManager, string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user != null)
            {
                var model = IdentityEFModule.Mapper.Map<EfIdentityUser, UserEditModel>(user);
                var claims = await userManager.GetClaimsAsync(user);
                if (claims.Any())
                    model.UserClaims = claims.Select(x => x.Value).ToList();
                return model;
            }

            throw new KeyNotFoundException($"No page found with the id '{id}'");
        }

        public static UserEditModel Create(UserManager<EfIdentityUser> userManager)
        {
            var model = new UserEditModel();

            model.Id = null;
            model.ChangePassword = true;

            return model;
        }

        public async Task<Tuple<bool, string>> Save(UserManager<EfIdentityUser> userManager)
        {
            var isNew = false;

            EfIdentityUser user = null;

            if (!string.IsNullOrEmpty(Id))
                user = await userManager.FindByIdAsync(Id);

            if(user == null)
                isNew = true;

            if (isNew)
            {
                user = new EfIdentityUser();

                IdentityEFModule.Mapper.Map(this, user);

                var result = await userManager.CreateAsync(user, Password);

                if (result.Succeeded)
                    Id = user.Id;

                return new Tuple<bool, string>(result.Succeeded, result.Errors.Any() ? result.Errors.ToArray()[0].Description : "");
            }

            try
            {
                IdentityEFModule.Mapper.Map(this, user);

                var result = await userManager.UpdateAsync(user);

                return new Tuple<bool, string>(result.Succeeded, result.Errors.Any() ? result.Errors.ToArray()[0].Description : "");
            }
            catch (Exception e)
            {
                return new Tuple<bool, string>(false, e.Message);
            }
            

        }

        public async Task<UserEditModel> UpdateClaimsAsync(UserManager<EfIdentityUser> userManager)
        {
            var user = await userManager.FindByIdAsync(Id);

            if(user != null)
            {
                var claims = await userManager.GetClaimsAsync(user);
                if (claims.Any())
                    UserClaims = claims.Select(x => x.Value).ToList();
            }

            return this;
        }
    }
}