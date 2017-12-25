using System;
using System.Collections.Generic;
using System.Linq;

namespace Piranha.AspNetCore.Identity.EF.Code
{
    public static class Permission
    {
        public const string Users = "PiranhaUsers";
        public const string UsersAdd = "PiranhaUsersAdd";
        public const string UsersDelete = "PiranhaUsersDelete";
        public const string UsersEdit = "PiranhaUsersEdit";
        public const string UsersSave = "PiranhaUsersSave";

        public static string[] All()
        {
            var permissions = new List<string>
            {
                Users,
                UsersAdd,
                UsersDelete,
                UsersEdit,
                UsersSave
            };

            // Include piranha permissions
            permissions.AddRange(Security.Permission.All());

            return permissions.ToArray();
        }

        public static List<string> Registered = new List<string>(All());

    }
}