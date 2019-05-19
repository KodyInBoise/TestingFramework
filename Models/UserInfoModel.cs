using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TestingFramework.Models
{
    public class UserInfoModel
    {
        [Key]
        public Guid UserID { get; set; }
        public Guid RoleID { get; set; }
    }
}
