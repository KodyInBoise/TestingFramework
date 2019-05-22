using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestingFramework.Models;

namespace TestingFramework.ViewModels.Admin
{
    public class UsersViewModel
    {
        public IEnumerable<UserInfoModel> UserInfos { get; set; }
        public IEnumerable<RoleModel> Roles { get; set; }
    }
}
