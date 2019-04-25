using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TestingFramework.Data;
using TestingFramework.Extensions;
using TestingFramework.Models;
using TestingFramework.ViewModels;
using TestingFramework.ViewModels.Scorecard;

namespace TestingFramework.Controllers
{
    public class ScorecardController : Controller
    {
        readonly ApplicationDbContext _database;

        public ScorecardController([FromServices] ApplicationDbContext db)
        {
            _database = db;
        }

        [HttpGet]
        public IActionResult StartScorecard(Guid id)
        {
            var progress = new ScorecardProgressModel
            {
                ScorecardID = id,
                Started = DateTime.Now,
                CurrentIndex = 0,
                User = User.Identity.Name
            };

            _database.ScorecardsInProgress.Add(progress);
            _database.SaveChanges();

            return RedirectToAction("Progress", new { id = progress.ID });
        }

        [HttpGet]
        public IActionResult Progress(Guid id)
        {
            var progress = _database.ScorecardsInProgress.Find(id);

            var scorecard = _database.Scorecards.Find(progress.ScorecardID);
            scorecard.Tests = _database.ScorecardTests.Where(t => t.ScorecardID == scorecard.ID).ToList();

            var categories = new List<CategoryModel>();
            var categoryIDs = scorecard.Tests.Select(t => t.CategoryID).Distinct().ToList();
            categoryIDs.ForEach(c => categories.Add(_database.Categories.Find(c)));

            var viewModel = new ScorecardProgressViewModel
            {
                Progress = progress,
                Scorecard = scorecard,
                Categories = categories,
                CategoryCompletePercentages = new Dictionary<Guid, string>()
            };

            var results = progress.GetResults();
            foreach (var category in viewModel.Categories)
            {
                var categoryResults = results.Where(r => r.CategoryID == category.ID && r.Passed != null);
                var categoryTests = scorecard.Tests.Where(t => t.CategoryID == category.ID);

                viewModel.CategoryCompletePercentages.Add(category.ID, Utils.GetPercentageString(categoryResults.Count(), categoryTests.Count()));
            }

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Delete(Guid id)
        {
            var progress = _database.ScorecardsInProgress.Find(id);

            if (progress != null)
            {
                _database.ScorecardsInProgress.Remove(progress);
                _database.SaveChanges();
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult CategoryTests(Guid id, Guid categoryID)
        {
            var progress = _database.ScorecardsInProgress.Find(id);
            var tests = _database.ScorecardTests.Where(t => t.CategoryID == categoryID && t.ScorecardID == progress.ScorecardID).ToList();

            var results = new List<ScorecardTestResultModel>();
            tests.ForEach(t =>
            {
                results.Add(new ScorecardTestResultModel
                {
                    ProgressID = progress.ID,
                    TestID = t.ID,
                    Notes = "",
                    Passed = null
                });
            });

            var existingResults = progress.GetResults();
            existingResults.ForEach(r =>
            {
                var result = results.FirstOrDefault(t => t.TestID == r.TestID);
                if (result != null)
                {
                    result.Passed = r.Passed;
                    result.Notes = r.Notes;
                }
            });

            var viewModel = new CategoryTestsViewModel
            {
                ProgressID = progress.ID,
                ScorecardID = progress.ScorecardID,
                ScorecardName = _database.Scorecards.FirstOrDefault(sc => sc.ID == progress.ScorecardID).Name,
                CategoryName = _database.Categories.FirstOrDefault(c => c.ID == categoryID).Name,
                Tests = tests,
                TestResults = results
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult UpdateCategoryTestResult(Guid progressID, Guid testID, bool passed, string notes)
        {
            var progress = _database.ScorecardsInProgress.Find(progressID);
            var categoryID = _database.ScorecardTests.FirstOrDefault(t => t.ID == testID).CategoryID;

            progress.AddOrUpdateResult(testID, categoryID, passed, notes);

            _database.ScorecardsInProgress.Update(progress);
            _database.SaveChanges();

            return RedirectToAction("CategoryTests", new { id = progressID, categoryID = categoryID });
        }

        [HttpGet]
        public IActionResult UpdateTestResult(Guid progressID, Guid testID, bool passed, string notes)
        {
            var progress = _database.ScorecardsInProgress.Find(progressID);
            var categoryID = _database.ScorecardTests.FirstOrDefault(t => t.ID == testID).CategoryID;

            progress.AddOrUpdateResult(testID, categoryID, passed, notes);

            _database.ScorecardsInProgress.Update(progress);
            _database.SaveChanges();

            return RedirectToAction("EditTestResult", new { id = progressID, testID = testID });
        }

        [HttpGet]
        public IActionResult SubmitProgress(Guid id)
        {
            var progress = _database.ScorecardsInProgress.Find(id);

            var scorecardResult = ScorecardResultModel.FromProgress(progress);
            scorecardResult.TotalTestCount = _database.ScorecardTests.Where(t => t.ScorecardID == progress.ScorecardID).Count();

            _database.ScorecardResults.Add(scorecardResult);
            _database.ScorecardsInProgress.Remove(progress);
            _database.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Results()
        {
            var viewModel = new ResultsViewModel
            {
                Results = _database.ScorecardResults.ToList(),
                Scorecards = _database.Scorecards.ToList()
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult DeleteResults(Guid id)
        {
            var results = _database.ScorecardResults.Find(id);
            if (results != null)
            {
                _database.Remove(results);
                _database.SaveChanges();
            }

            return RedirectToAction("Results");
        }

        [HttpGet]
        public IActionResult EditTestResult(Guid id, Guid testID)
        {
            var progress = _database.ScorecardsInProgress.Find(id);
            var test = _database.ScorecardTests.Find(testID);

            var result = progress.GetResults().FirstOrDefault(r => r.TestID == testID);
            if (result == null)
            {
                result = new ScorecardTestResultModel
                {
                    ProgressID = progress.ID,
                    TestID = testID,
                    CategoryID = test.CategoryID
                };
            }

            var viewModel = new EditTestResultViewModel
            {
                Test = test,
                Result = result,
            };

            return View(viewModel);
        }

        [HttpGet]
        [Route("Scorecard/ProgressDetails/{id}")]
        public IActionResult ProgressDetails(Guid id)
        {
            var progress = _database.ScorecardsInProgress.Find(id);
            var scorecard = _database.Scorecards.Find(progress?.ScorecardID);
            scorecard.Tests = _database.ScorecardTests.Where(t => t.ScorecardID == scorecard.ID).ToList();

            if (progress == null || scorecard == null)
            {
                return NotFound();
            } 

            var viewModel = new ProgressDetailsViewModel
            {
                Progress = progress,
                Scorecard = _database.Scorecards.Find(progress.ScorecardID),
                ReadOnly = progress.User != User.Identity.Name
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult ProgressDetails(ProgressDetailsViewModel viewModel)
        {
            _database.ScorecardsInProgress.Update(viewModel.Progress);
            _database.SaveChanges();

            return RedirectToAction("Progress", new { id = viewModel.Progress.ID });
        }
    }
}