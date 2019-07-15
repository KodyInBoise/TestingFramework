using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestingFramework.Data;
using TestingFramework.Models;

namespace TestingFramework.Extensions
{
   public class ImportExportUtil
   {
      public const string EXPORT_FILENAME = "export.tfdata";

      public static async Task<AppDataModel> ExportApplicationData(ApplicationDbContext database)
      {
         return new AppDataModel
         {
            Categories = await database.Categories.ToListAsync(),
            CategoryTests = await database.CategoryTests.ToListAsync(),
            Scorecards = await database.Scorecards.ToListAsync(),
            ScorecardTests = await database.ScorecardTests.ToListAsync(),
            ScorecardsInProgress = await database.ScorecardsInProgress.ToListAsync(),
            ScorecardResults = await database.ScorecardResults.ToListAsync(),
            Tasks = await database.Tasks.ToListAsync(),
            TaskHistory = await database.TaskHistory.ToListAsync(),
            TaskComments = await database.TaskComments.ToListAsync(),
            Roles = await database.Roles.ToListAsync()
         };
      }

      public static async Task<bool> ImportApplicationData(ApplicationDbContext database, AppDataModel data)
      {
         try
         {
            // Categories
            foreach (var category in data.Categories)
            {
               var exists = await database.Categories.FirstOrDefaultAsync(x => x.ID == category.ID) != null;
               if (!exists)
               {
                  database.Categories.Add(category);
               }
            }

            await database.SaveChangesAsync();

            // CategoryTests
            foreach (var test in data.CategoryTests)
            {
               var exists = await database.Categories.FirstOrDefaultAsync(x => x.ID == test.ID) != null;
               if (!exists)
               {
                  database.CategoryTests.Add(test);
               }
            }

            await database.SaveChangesAsync();

            // Scorecards
            foreach (var scorecard in data.Scorecards)
            {
               var exists = await database.Scorecards.FirstOrDefaultAsync(x => x.ID == scorecard.ID) != null;
               if (!exists)
               {
                  database.Scorecards.Add(scorecard);
               }
            }

            await database.SaveChangesAsync();

            // ScorecardTests
            foreach (var test in data.ScorecardTests)
            {
               var exists = await database.ScorecardTests.FirstOrDefaultAsync(x => x.ID == test.ID) != null;
               if (!exists)
               {
                  database.ScorecardTests.Add(test);
               }
            }

            await database.SaveChangesAsync();

            // ScorecardsInProgress
            foreach (var progress in data.ScorecardsInProgress)
            {
               var exists = await database.ScorecardsInProgress.FirstOrDefaultAsync(x => x.ID == progress.ID) != null;
               if (!exists)
               {
                  database.ScorecardsInProgress.Add(progress);
               }
            }

            await database.SaveChangesAsync();

            // ScorecardResults
            foreach (var results in data.ScorecardResults)
            {
               var exists = await database.ScorecardResults.FirstOrDefaultAsync(x => x.ID == results.ID) != null;
               if (!exists)
               {
                  database.ScorecardResults.Add(results);
               }
            }

            await database.SaveChangesAsync();

            // Tasks
            foreach (var task in data.Tasks)
            {
               var exists = await database.Tasks.FirstOrDefaultAsync(x => x.ID == task.ID) != null;
               if (!exists)
               {
                  database.Tasks.Add(task);
               }
            }

            await database.SaveChangesAsync();

            // TaskHistory
            foreach (var history in data.TaskHistory)
            {
               var exists = await database.TaskHistory.FirstOrDefaultAsync(x => x.ID == history.ID) != null;
               if (!exists)
               {
                  database.TaskHistory.Add(history);
               }
            }

            await database.SaveChangesAsync();

            // TaskComments
            foreach (var comment in data.TaskComments)
            {
               var exists = await database.TaskComments.FirstOrDefaultAsync(x => x.ID == comment.ID) != null;
               if (!exists)
               {
                  database.TaskComments.Add(comment);
               }
            }

            await database.SaveChangesAsync();

            // Roles
            foreach (var role in data.Roles)
            {
               var exists = await database.Roles.FirstOrDefaultAsync(x => x.ID == role.ID) != null;
               if (!exists)
               {
                  database.Roles.Add(role);
               }
            }

            await database.SaveChangesAsync();

            return true;
         }
         catch (Exception ex)
         {
            LoggingUtil.AddException(ex);

            return false;
         }
      }

      public static List<ImportTestModel> CreateTests(char separator, string text)
      {
         var tests = new List<ImportTestModel>();
         var lines = text.Split(new[] { '\r', '\n' });

         foreach (var line in lines)
         {
            try
            {
               var parts = line.Split(separator);
               Double.TryParse(parts[3], out var value);

               tests.Add(new ImportTestModel
               {
                  CategoryName = parts[0],
                  Description = parts[1],
                  ExpectedResult = parts[2],
                  Value = value
               });
            }
            catch (Exception ex) { LoggingUtil.AddException(ex); }
         }

         return tests;
      }
   }
}
