using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestingFramework.Models;

namespace TestingFramework.ViewModels.Scorecard
{
    public class EditTestResultViewModel
    {
        public ScorecardTestModel Test { get; set; }
        public ScorecardTestResultModel Result { get; set; }
    }
}
