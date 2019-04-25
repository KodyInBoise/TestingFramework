using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestingFramework.Extensions;
using TestingFramework.Models;

namespace TestingFramework.ViewModels.Scorecard
{
    public class ProgressDetailsViewModel
    {
        public ScorecardProgressModel Progress { get; set; }
        public ScorecardModel Scorecard { get; set; }

        public string GetTestsCompletedString()
        {
            var completed = Progress.GetResults().Count();
            var total = Scorecard.Tests.Count();
            var percentage = Utils.GetPercentageString(completed, total);

            return $"{completed} of {total} - {percentage}";
        }
    }
}
