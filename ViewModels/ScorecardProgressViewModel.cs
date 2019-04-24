using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestingFramework.Extensions;
using TestingFramework.Models;

namespace TestingFramework.ViewModels
{
    public class ScorecardProgressViewModel
    {
        public ScorecardProgressModel Progress { get; set; }
        public ScorecardModel Scorecard { get; set; }
        public IEnumerable<CategoryModel> Categories { get; set; }
        public Dictionary<Guid, string> CategoryCompletePercentages { get; set; }

        public string GetCategoryName(Guid id)
        {
            return Categories.FirstOrDefault(c => c.ID == id)?.Name ?? "";
        }

        public string GetTotalCompletedPercent()
        {
            var results = Progress.GetResults();

            return Utils.GetPercentageString(results.Count(), Scorecard.Tests.Count());
        }

        public string GetTestStatus(Guid testID)
        {
            var passed = Progress.GetResults().FirstOrDefault(r => r.TestID == testID)?.Passed ?? null;
            
            if (passed == true)
            {
                return "Passed";
            }
            else if (passed == false)
            {
                return "Failed";
            }
            else
            {
                return "";
            }
        }
    }
}
