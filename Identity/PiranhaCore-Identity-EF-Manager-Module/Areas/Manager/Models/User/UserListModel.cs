using System.Diagnostics.Tracing;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Piranha.AspNetCore.Identity.EF;
using Piranha.AspNetCore.Identity.EF.Code;
using Piranha.AspNetCore.Identity.EF.Data;
using Piranha.AspNetCore.Identity.EF.Extensions;

namespace Piranha.AspNetCore.Identity.EF.Manager.Areas.Manager.Models
{
    public class UserListModel
    {
        public UserListModel()
        {
            UserList = new PagedList<EfIdentityUser>();
        }

        #region Properties

        public PagedList<EfIdentityUser> UserList { get; set; }

        #endregion

        public static UserListModel Get(UserManager<EfIdentityUser> userManager)
        {
            var model = new UserListModel();

            model.UserList = userManager.Users.OrderBy(x => x.Id).ToPage(1, 25);

            return model;
        }

        
    }
}