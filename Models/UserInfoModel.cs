using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TestingFramework.Models
{
    public class UserInfoModel
    {
        [Key]
        public Guid UserID { get; set; }
        public Guid RoleID { get; set; }
        public string Name { get; set; }
        public DateTime LastActivity { get; set; }
    }
}
