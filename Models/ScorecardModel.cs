using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestingFramework.Models
{
    public class ScorecardModel
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<ScorecardTestModel> Tests { get; set; }
        public double DefaultTestValue { get; set; }
    }
}
