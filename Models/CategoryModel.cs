using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestingFramework.Models
{
    public class CategoryModel
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }


        public override string ToString()
        {
            return Name ?? "";
        }
    }
}
