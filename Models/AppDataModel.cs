using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestingFramework.Models.Tasks;

namespace TestingFramework.Models
{
   public class AppDataModel
   {
      public IEnumerable<CategoryModel> Categories { get; set; }
      public IEnumerable<CategoryTestModel> CategoryTests { get; set; }
      public IEnumerable<ScorecardModel> Scorecards { get; set; }
      public IEnumerable<ScorecardTestModel> ScorecardTests { get; set; }
      public IEnumerable<ScorecardProgressModel> ScorecardsInProgress { get; set; }
      public IEnumerable<ScorecardResultModel> ScorecardResults { get; set; }
      public IEnumerable<TaskModel> Tasks { get; set; }
      public IEnumerable<TaskHistoryModel> TaskHistory { get; set; }
      public IEnumerable<TaskCommentModel> TaskComments { get; set; }
      public IEnumerable<RoleModel> Roles { get; set; }
   }
}
