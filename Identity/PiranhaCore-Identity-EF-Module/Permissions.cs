using System.Collections.Generic;
using System.Linq;

namespace Piranha.AspNetCore.Identity.EF
{
    public static class Permission
    {
        public const string Users = "PiranhaUsers";
        public const string UsersAdd = "PiranhaUsersAdd";
        public const string UsersDelete = "PiranhaUsersDelete";
        public const string UsersEdit = "PiranhaUsersEdit";
        public const string UsersSave = "PiranhaUsersSave";

        public static string[] All(string[] piranhaPermissions = null)
        {
            var permissions = new List<string>
            {
                Users,
                UsersAdd,
                UsersDelete,
                UsersEdit,
                UsersSave
            };

            if (piranhaPermissions != null)
            {
                permissions.AddRange(piranhaPermissions.ToList());
            }

            return permissions.ToArray();
        }
    }
}