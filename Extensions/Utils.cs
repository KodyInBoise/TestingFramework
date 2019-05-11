using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TestingFramework.Data;
using TestingFramework.Models;

namespace TestingFramework.Extensions
{
    public class Utils
    {
        public class Import
        {
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
                    catch { /* Need error logging here */ }
                }

                return tests;
            }
        }

        public class Admin
        {
            public static void ClearData(ApplicationDbContext db)
            {
                ClearScorecards(db);
                ClearScorecardTests(db);
                ClearScorecardsInProgress(db);
                ClearResults(db);
                ClearCategories(db);
                ClearCategoryTests(db);
                ClearTasks(db);
            }

            static void ClearScorecards(ApplicationDbContext db)
            {
                var scorecards = db.Scorecards.ToList();
                scorecards.ForEach(x =>
                {
                    db.Scorecards.Remove(x);
                });

                db.SaveChanges();
            }

            static void ClearScorecardTests(ApplicationDbContext db)
            {
                var tests = db.ScorecardTests.ToList();
                tests.ForEach(x =>
                {
                    db.ScorecardTests.Remove(x);
                });

                db.SaveChanges();
            }

            static void ClearResults(ApplicationDbContext db)
            {
                var results = db.ScorecardResults.ToList();
                results.ForEach(x =>
                {
                    db.ScorecardResults.Remove(x);
                });

                db.SaveChanges();
            }

            static void ClearScorecardsInProgress(ApplicationDbContext db)
            {
                var progresses = db.ScorecardsInProgress.ToList();
                progresses.ForEach(x =>
                {
                    db.ScorecardsInProgress.Remove(x);
                });

                db.SaveChanges();
            }

            static void ClearCategories(ApplicationDbContext db)
            {
                var categories = db.Categories.ToList();
                categories.ForEach(x =>
                {
                    db.Categories.Remove(x);
                });

                db.SaveChanges();
            }

            static void ClearCategoryTests(ApplicationDbContext db)
            {
                var tests = db.CategoryTests.ToList();
                tests.ForEach(x =>
                {
                    db.CategoryTests.Remove(x);
                });

                db.SaveChanges();
            }

            static void ClearTasks(ApplicationDbContext db)
            {
                var tasks = db.Tasks.ToList();
                tasks.ForEach(x =>
                {
                    db.Tasks.Remove(x);
                });

                db.SaveChanges();
            }
        }

        public static Guid GetUserID(ClaimsPrincipal user)
        {
            try
            {
                return Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value);           
            }
            catch (Exception ex)
            {
                // TODO: Add logging
                return default(Guid);
            }
        }

        public static bool ValidateGuid(Guid? guid)
        {
            return guid != null && guid != default(Guid);
        }

        public static string GetPercentageString(double value, double total, int places = 2)
        {
            try
            {
                var percentage = Math.Round((value / total), places);

                if (percentage < 1)
                {
                    var percentageString = percentage.ToString().Split('.')[1];
                    if (percentageString.Length == 1)
                    {
                        percentageString += "0";
                    }

                    return $"{percentageString}%";
                }
                else
                {
                    return "100%";
                }
            }
            catch
            {
                return "0%";
            }
        }

        public static string GetTimespanString(TimeSpan timespan)
        {
            var timespanString = "";

            if (timespan.Days > 0)
            {
                timespanString += $"{timespan.Days} Days, ";
            }

            timespanString += $"{timespan.Hours} Hours, {timespan.Minutes} Minutes";

            return timespanString;
        }
    }

    public class DateTimeRange
    {
        public TimeSpan Timespan { get; private set; }

        public DateTimeRange(DateTime start, DateTime end)
        {
            Timespan = end.Subtract(start);
        }
    }
}
