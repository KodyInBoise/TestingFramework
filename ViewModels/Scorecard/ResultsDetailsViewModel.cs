using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestingFramework.Extensions;
using TestingFramework.Models;

namespace TestingFramework.ViewModels.Scorecard
{
    public class ResultsDetailsViewModel
    {
        public ScorecardResultModel Results { get; set; }
        public ScorecardModel Scorecard { get; set; }

        public string GetTestsCompletedString()
        {
            var completed = Results.GetTestResults().Count();
            var total = Scorecard.Tests.Count();
            var percentage = Utils.GetPercentageString(completed, total);

            return $"{completed} of {total} - {percentage}";
        }

        public string GetCurrentScore()
        {
            var passed = Results.GetTestResults().Where(r => r.Passed == true).Count();
            var total = Scorecard.Tests.Count();

            return Utils.GetPercentageString(passed, total);
        }

        public string GetDuration()
        {
            var range = new DateTimeRange(Results.StartedTimestamp, Results.CompletedTimestamp);

            return range.Timespan.ToString();
        }
    }
}
