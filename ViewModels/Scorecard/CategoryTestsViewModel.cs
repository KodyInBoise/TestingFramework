using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestingFramework.Models;

namespace TestingFramework.ViewModels.Scorecard
{
    public class CategoryTestsViewModel
    {
        public Guid ProgressID { get; set; }
        public Guid ScorecardID { get; set; }
        public string ScorecardName { get; set; }
        public string CategoryName { get; set; }
        public IEnumerable<ScorecardTestModel> Tests { get; set; }
        public IEnumerable<ScorecardTestResultModel> TestResults { get; set; }
    }
}
