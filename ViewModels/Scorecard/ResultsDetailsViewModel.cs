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
            var t = Results.GetTestResults();
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
            var durationString = "";

            if (range.Timespan.Days > 0)
            {
                durationString += $"{range.Timespan.Days} Days, ";
            }

            durationString += $"{range.Timespan.Hours} Hours, {range.Timespan.Minutes} Minutes";

            return durationString;
        }

        public ScorecardTestModel GetScorecardTest(Guid testID)
        {
            return Scorecard.Tests.FirstOrDefault(t => t.ID == testID);
        }
    }
}
