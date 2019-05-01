using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TestingFramework.Models
{
    public class ScorecardTestModel : ITest
    {
        public Guid ID { get; set; }
        public Guid TestID { get; set; }
        public Guid ScorecardID { get; set; }
        public Guid CategoryID { get; set;}
        public string Description { get; set; }
        public string ExpectedResult { get; set; }
        public double Value { get; set; }
        
        [NotMapped]
        public ScorecardTestResultModel Result { get; set; }

        public static ScorecardTestModel FromCategoryTest(CategoryTestModel test, Guid scorecardID)
        {
            return new ScorecardTestModel
            {
                TestID = test.ID,
                ScorecardID = scorecardID,
                CategoryID = test.CategoryID,
                Description = test.Description,
                ExpectedResult = test.ExpectedResult,
            };
        }
    }
}
