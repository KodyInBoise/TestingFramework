using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestingFramework.Models;

namespace TestingFramework.ViewModels.Admin
{
    public class EditUserViewModel
    {
        public UserInfoModel UserInfo { get; set; }
        public IEnumerable<RoleModel> Roles { get; set; }
        public SelectList RoleOptions { get; set; }
        public RoleModel SelectedRole { get; set; }
    }
}
