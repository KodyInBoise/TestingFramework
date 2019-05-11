using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestingFramework.Models;

namespace TestingFramework.ViewModels.Setup
{
    public class EditCategoryTestViewModel
    {
        public Guid SelectedCategoryID { get; set; }
        public SelectList CategoriesList { get; set; }
        public CategoryTestModel Test { get; set; }
    }
}
