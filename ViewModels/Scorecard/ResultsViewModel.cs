using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestingFramework.Extensions;
using TestingFramework.Models;

namespace TestingFramework.ViewModels.Scorecard
{
    public class ResultsViewModel
    {
        public IEnumerable<ScorecardResultModel> Results { get; set; }
        public IEnumerable<ScorecardModel> Scorecards { get; set; }
        public IEnumerable<ScorecardProgressModel> ScorecardsInProgress { get; set; }


        public string GetScorecardName(Guid id)
        {
            try
            {
                return Scorecards.FirstOrDefault(sc => sc.ID == id).Name;
            }
            catch (Exception ex)
            {
                LoggingUtil.AddException(ex);

                return "";
            }
        }

        public string GetResultsScore(Guid id)
        {
            try
            {
                var results = Results.FirstOrDefault(r => r.ID == id);

                var passed = results.GetTestResults().Where(r => r.Passed == true);

                return Utils.GetPercentageString(passed.Count(), results.TotalTestCount);
            }
            catch (Exception ex)
            {
                LoggingUtil.AddException(ex);

                return "error";
            }
        }

        public string GetProgress(Guid id)
        {
            try
            {
                var progress = ScorecardsInProgress.FirstOrDefault(p => p.ID == id);
                var scorecard = Scorecards.FirstOrDefault(s => s.ID == progress.ScorecardID);

                return Utils.GetPercentageString(progress.GetResults().Count(), scorecard.Tests.Count());
            }
            catch (Exception ex)
            {
                LoggingUtil.AddException(ex);

                return "error";
            }
        }
    }
}
