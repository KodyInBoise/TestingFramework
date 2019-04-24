﻿using System;
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


        public string GetScorecardName(Guid id)
        {
            return Scorecards.FirstOrDefault(sc => sc.ID == id)?.Name ?? "";
        }

        public string GetResultsScore(Guid id)
        {
            var results = Results.FirstOrDefault(r => r.ID == id);

            var passed = results.GetTestResults().Where(r => r.Passed == true);

            return Utils.GetPercentageString(passed.Count(), results.TotalTestCount);
        }
    }
}