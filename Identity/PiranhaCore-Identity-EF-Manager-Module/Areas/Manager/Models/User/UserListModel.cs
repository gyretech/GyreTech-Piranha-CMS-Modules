using Microsoft.AspNetCore.Identity;
using Piranha.AspNetCore.Identity.EF;

namespace Piranha.AspNetCore.Identity.EF.Manager.Areas.Manager.Models
{
    public class UserListModel
    {
        public UserListModel()
        {
            UserList = new PagedList<IdentityAppUser>();
        }

        #region Properties

        public PagedList<IdentityAppUser> UserList { get; set; }

        #endregion

        public static UserListModel Get(AppUserRepository repo, UserManager<IdentityAppUser> userManager)
        {
            var model = new UserListModel();

            model.UserList = repo.GetPage(1, 10);

            return model;
        }

        
    }
}