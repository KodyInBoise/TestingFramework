using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestingFramework.Models;

namespace TestingFramework.ViewModels
{
    public class SetupViewModel
    {
        public List<CategoryModel> Categories { get; set; }
        public List<CategoryTestModel> Tests { get; set; }
        public List<ScorecardModel> Scorecards { get; set; }


        public string GetCategoryName(Guid id)
        {
            try
            {
                var category = Categories.Find(x => x.ID == id);

                return category.Name;
            }
            catch
            {
                return "";
            }
        }

        public int GetCategoryTestCount(Guid categoryID)
        {
            return Tests.Where(t => t.CategoryID == categoryID)?.Count() ?? 0;
        }
    }
}
