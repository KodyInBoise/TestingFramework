using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestingFramework.Models
{
    public class ScorecardTestResultModel
    {
        public Guid ProgressID { get; set;}
        public Guid CategoryID { get; set; }
        public Guid TestID { get; set; }
        public bool? Passed { get; set; }
        public string Notes { get; set; }
    }
}
