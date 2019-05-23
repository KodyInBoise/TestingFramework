using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TestingFramework.Data;
using TestingFramework.Models;

namespace TestingFramework.Extensions
{
    public enum RoleType : int
    {
        Admin = 0,
        PowerUser = 1,
        User = 2
    }


    public class RoleHelper
    {
        public const RoleType DefaultRoleType = RoleType.User;


        public static void SeedAppRoles(ApplicationDbContext database)
        {
            var roles = database.Roles.ToList();

            if (roles.FirstOrDefault(r => r.Type == RoleType.Admin) == null)
            {
                database.Roles.Add(new RoleModel
                {
                    Type = RoleType.Admin,
                    Name = "Admin",
                    Description = "Application Administrator"
                });
            }

            if (roles.FirstOrDefault(r => r.Type == RoleType.PowerUser) == null)
            {
                database.Roles.Add(new RoleModel
                {
                    Type = RoleType.PowerUser,
                    Name = "Power User",
                    Description = "Application user with setup capabilities"
                });
            }

            if (roles.FirstOrDefault(r => r.Type == RoleType.User) == null)
            {
                database.Roles.Add(new RoleModel
                {
                    Type = RoleType.User,
                    Name = "User",
                    Description = "Application user who can participate in testing and view results"
                });
            }

            database.SaveChanges();
        }

        public static UserInfoModel UpdateUserRole(ApplicationDbContext database, ClaimsPrincipal user, RoleType roleType)
        {
            var userInfo = GetContextUserInfo(database, user);
            var userRole = database.Roles.Find(userInfo.RoleID);

            if (userRole.Type == roleType)
            {
                return userInfo;
            }

            var newRole = database.Roles.FirstOrDefault(r => r.Type == roleType);

            userInfo.RoleID = newRole.ID;

            database.UserInfos.Update(userInfo);
            database.SaveChanges();

            LoggingUtil.AddEntry(user.Identity.Name, $"User role changed from {userRole.Name} to {newRole.Name}");

            return userInfo;
        }

        public static UserInfoModel GetContextUserInfo(ApplicationDbContext database, ClaimsPrincipal user)
        {
            var userID = Utils.GetUserID(user);

            var userInfo = database.UserInfos.FirstOrDefault(u => u.UserID == userID);

            if (userInfo == null)
            {
                var defaultRole = database.Roles.FirstOrDefault(r => r.Type == DefaultRoleType);

                userInfo = new UserInfoModel
                {
                    UserID = userID,
                    RoleID = defaultRole.ID
                };

                database.UserInfos.Add(userInfo);
                database.SaveChanges();

                LoggingUtil.AddEntry(user.Identity.Name, "New user info added with default role.");
            }

            return userInfo;
        }

        public static UserInfoModel GetUserInfo(ApplicationDbContext database, Guid userID)
        {
            var userInfo = database.UserInfos.FirstOrDefault(u => u.UserID == userID);

            if (userInfo == null)
            {
                var defaultRole = database.Roles.FirstOrDefault(r => r.Type == DefaultRoleType);

                userInfo = new UserInfoModel
                {
                    UserID = userID,
                    RoleID = defaultRole.ID,
                    Name = database.AspNetUsers.Find(userID.ToString())?.UserName ?? ""
                };

                database.UserInfos.Add(userInfo);
                database.SaveChanges();

                LoggingUtil.AddEntry(userInfo.Name, "New user info added with default role.");
            }
            else if (string.IsNullOrEmpty(userInfo.Name))
            {
                userInfo.Name = database.AspNetUsers.Find(userInfo.UserID.ToString())?.UserName ?? "";

                database.UserInfos.Update(userInfo);
                database.SaveChanges();
            }

            return userInfo;
        }

        public static bool UserIsAdmin(ApplicationDbContext database, ClaimsPrincipal user)
        {
            var userInfo = GetContextUserInfo(database, user);
            var userRole = database.Roles.Find(userInfo.RoleID);

            return userRole.Type == RoleType.Admin;
        }
    }
}
