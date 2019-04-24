using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestingFramework.Models;

namespace TestingFramework.ViewModels
{
    public class AddScorecardTestsViewModel
    {
        public ScorecardModel Scorecard { get; set; }
        public Guid SelectedCategoryID { get; set; }
        public IEnumerable<CategoryModel> Categories { get; set; }
        public IEnumerable<ITest> CategoryTests { get; set; }
        public int ScorecardTestCount { get; set; }
    }
}
