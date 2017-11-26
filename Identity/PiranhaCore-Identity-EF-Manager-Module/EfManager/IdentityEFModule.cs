using System;
using System.IO;
using System.Reflection;
using AutoMapper;
using Piranha.AspNetCore.Identity.EF.Manager.Areas.Manager.Models;
using Piranha.Manager;

namespace Piranha.AspNetCore.Identity.EF.Manager
{
    public class IdentityEFModule : Extend.IModule
    {
        #region Properties
        /// <summary>
        /// Gets the mapper.
        /// </summary>
        public static IMapper Mapper { get; private set; }

        /// <summary>
        /// The assembly.
        /// </summary>
        internal static Assembly Assembly;

        /// <summary>
        /// Last modification date of the assembly.
        /// </summary>
        internal static DateTime LastModified;
        #endregion


        public void Init()
        {

            Menu.Items.Add(new Menu.MenuItem()
            {
                InternalId = "IdentityManager", Name = "Authentication", Css = "glyphicon glyphicon-lock", Items = new Menu.MenuItemList()
                {
                    new Menu.MenuItem()
                    {
                        InternalId = "IdentityEfUsers", Name = "Users", Controller = "User", Action = "List", Css = "glyphicon glyphicon-user"
                    }
                }
            });

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<IdentityAppUser, UserEditModel>()
                    .ForMember(m => m.ChangePassword, o => o.Ignore())
                    .ForMember(m => m.PasswordConfirm, o => o.Ignore())
                    .ForMember(m => m.UserClaims, o => o.Ignore())
                    .ForMember(m => m.AvailableClaims, o => o.Ignore());
                cfg.CreateMap<UserEditModel, IdentityAppUser>()
                    .ForMember(m => m.Claims, o => o.Ignore())
                    .ForMember(m => m.NormalizedUserName, o => o.Ignore())
                    .ForMember(m => m.NormalizedEmail, o => o.Ignore())
                    .ForMember(m => m.EmailConfirmed, o => o.Ignore())
                    .ForMember(m => m.SecurityStamp, o => o.Ignore())
                    .ForMember(m => m.ConcurrencyStamp, o => o.Ignore())
                    .ForMember(m => m.PhoneNumberConfirmed, o => o.Ignore())
                    .ForMember(m => m.TwoFactorEnabled, o => o.Ignore())
                    .ForMember(m => m.LockoutEnd, o => o.Ignore())
                    .ForMember(m => m.LockoutEnabled, o => o.Ignore())
                    .ForMember(m => m.PasswordHash, o => o.Ignore())
                    .ForMember(m => m.AccessFailedCount, o => o.Ignore());

            });

            config.AssertConfigurationIsValid();
            Mapper = config.CreateMapper();

            // Get assembly information
            Assembly = this.GetType().GetTypeInfo().Assembly;
            LastModified = new FileInfo(Assembly.Location).LastWriteTime;
        }
    }
}