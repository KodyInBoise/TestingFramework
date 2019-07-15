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
            TaskComments = await database.TaskComments.ToListAsync()
         };
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
