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
            var userID = Utils.GetUserID(user);

            var userInfo = database.UserInfos.FirstOrDefault(u => u.UserID == userID);
            var role = database.Roles.FirstOrDefault(r => r.Type == roleType);

            if (userInfo == null && Utils.ValidateGuid(userID))
            {
                userInfo = new UserInfoModel
                {
                    UserID = userID, 
                    RoleID = role.ID
                };

                database.UserInfos.Add(userInfo);
            }
            else
            {
                userInfo.RoleID = role.ID;

                database.Update(userInfo);
            }

            database.SaveChanges();

            return userInfo;
        }

        public static UserInfoModel GetUserInfo(ApplicationDbContext database, ClaimsPrincipal user)
        {
            var userID = Utils.GetUserID(user);

            var userInfo = database.UserInfos.FirstOrDefault(u => u.UserID == userID);

            if (userInfo == null)
            {
                userInfo = UpdateUserRole(database, user, DefaultRoleType);
            }

            return userInfo;
        }
    }
}
