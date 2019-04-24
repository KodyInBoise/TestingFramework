using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestingFramework.Models;

namespace TestingFramework.ViewModels
{
    public class CreateCategoryTestViewModel
    {
        public Guid SelectedCategoryID { get; set; }
        public SelectList CategoriesList { get; set; }
        public string Description { get; set; }
        public string ExpectedResult { get; set; }
    }
}
