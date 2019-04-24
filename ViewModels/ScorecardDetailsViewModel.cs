using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestingFramework.Models;

namespace TestingFramework.ViewModels
{
    public class ScorecardDetailsViewModel
    {
        public ScorecardModel Scorecard { get; set; }
        public IEnumerable<CategoryModel> Categories { get; set; }
        public IEnumerable<ScorecardTestModel> Tests { get; set; }


        public string GetCategoryName(Guid id)
        {
            return Categories.FirstOrDefault(c => c.ID == id)?.Name ?? "";
        }

        public double GetTotalValue()
        {
            double value = 0;

            foreach (var test in Tests)
            {
                value += test.Value;
            }

            return value;
        }
    }
}
