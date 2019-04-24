using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestingFramework.Models;

namespace TestingFramework.ViewModels
{
    public class CategoryDetailsViewModel
    {
        public CategoryModel Category { get; set; }
        public IEnumerable<ITest> Tests { get; set; }
    }
}
