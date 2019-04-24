using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestingFramework.Extensions;
using TestingFramework.Models;

namespace TestingFramework.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<ScorecardModel> Scorecards { get; set; }
        public IEnumerable<ScorecardProgressModel> ScorecardsInProgress { get; set; }
        public IEnumerable<ScorecardTestModel> ScorecardTests { get; set; }

        public string GetProgress(Guid id)
        {
            var progress = ScorecardsInProgress.FirstOrDefault(p => p.ID == id);
            var tests = ScorecardTests.Where(t => t.ScorecardID == progress.ScorecardID);

            return Utils.GetPercentageString(progress.GetResults().Count(), tests.Count());
        }
    }
}
