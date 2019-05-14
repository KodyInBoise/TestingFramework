using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestingFramework.Extensions;

namespace TestingFramework.Models
{
    public class RoleModel
    {
        public Guid ID { get; set; }
        public RoleType Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
