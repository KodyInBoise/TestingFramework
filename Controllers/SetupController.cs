using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TestingFramework.Data;
using TestingFramework.Models;
using TestingFramework.ViewModels;

namespace TestingFramework.Controllers
{
    public class SetupController : Controller
    {
        readonly ApplicationDbContext _database;

        public SetupController([FromServices] ApplicationDbContext db)
        {
            _database = db;
        }

        public IActionResult Index()
        {
            var viewModel = new SetupViewModel
            {
                Categories = _database.Categories.ToList(),
                Tests = _database.CategoryTests.ToList(),
                Scorecards = _database.Scorecards.ToList(),
            };

            viewModel.Scorecards.ForEach(sc => sc.Tests = _database.ScorecardTests.Where(t => t.ScorecardID == sc.ID).ToList());

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateCategory(CategoryModel category)
        {
            _database.Categories.Add(category);
            _database.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult CreateCategoryTest(Guid id)
        {
            var categories = _database.Categories.ToList();

            var viewModel = new CreateCategoryTestViewModel
            {
                CategoriesList = new SelectList(categories, "ID", "Name"),
                SelectedCategoryID = id
            };

            if (viewModel.SelectedCategoryID != null)
            {
                viewModel.CategoriesList.First(c => c.Value == viewModel.SelectedCategoryID.ToString()).Selected = true;
            }

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult CreateCategoryTest(CreateCategoryTestViewModel viewModel)
        {
            var test = new CategoryTestModel
            {
                CategoryID = viewModel.SelectedCategoryID,
                Description = viewModel.Description,
                ExpectedResult = viewModel.ExpectedResult
            };

            _database.CategoryTests.Add(test);
            _database.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult CategoryDetails(Guid id)
        {
            var category = _database.Categories.Find(id);

            if (category == null)
            {
                return NotFound();
            }

            var viewModel = new CategoryDetailsViewModel
            {
                Category = category,
                Tests = _database.CategoryTests.Where(t => t.CategoryID == id)
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult CreateScorecard()
        {
            var scorecard = new ScorecardModel { DefaultTestValue = 1 };

            return View(scorecard);
        }

        [HttpPost]
        public IActionResult CreateScorecard(ScorecardModel scorecard)
        {
            _database.Scorecards.Add(scorecard);
            _database.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult ScorecardDetails(Guid id)
        {
            var scorecard = _database.Scorecards.Find(id);

            if (scorecard == null)
            {
                return NotFound();
            }

            var viewModel = new ScorecardDetailsViewModel
            {
                Scorecard = scorecard,
                Tests = _database.ScorecardTests.Where(t => t.ScorecardID == scorecard.ID),
                Categories = _database.Categories.ToList()
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult AddScorecardTests(Guid id, Guid categoryID)
        {
            var scorecard = _database.Scorecards.Find(id);

            if (scorecard == null)
            {
                return NotFound();
            }

            var categoryTests = _database.CategoryTests.Where(t => t.CategoryID == categoryID).ToList();
            foreach (var test in categoryTests.ToList())
            {
                var scorecardTest = _database.ScorecardTests.FirstOrDefault(t => t.ID == test.ID && t.ScorecardID == scorecard.ID);
                if (scorecardTest != null)
                {
                    categoryTests.Remove(test);
                }
            }

            var viewModel = new AddScorecardTestsViewModel
            {
                Scorecard = scorecard,
                SelectedCategoryID = categoryID,
                Categories = _database.Categories.ToList(),
                CategoryTests = categoryTests,
                ScorecardTestCount = _database.ScorecardTests.Where(t => t.ScorecardID == scorecard.ID).Count()
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult AddTestToScorecard(Guid scorecardID, Guid categoryID, Guid testID)
        {
            var scorecard = _database.Scorecards.Find(scorecardID);
            var categoryTest = _database.CategoryTests.Find(testID);

            if (categoryTest != null)
            {
                var scorecardTest = ScorecardTestModel.FromCategoryTest(categoryTest, scorecardID);
                scorecardTest.Value = scorecard.DefaultTestValue;

                _database.ScorecardTests.Add(scorecardTest);
                _database.SaveChanges();
            }

            return RedirectToAction("AddScorecardTests", new { id = scorecardID, categoryID = categoryID });
        }

        [HttpGet]
        public IActionResult EditScorecardTest(Guid id)
        {
            var scorecardTest = _database.ScorecardTests.Find(id);

            if (scorecardTest == null)
            {
                return NotFound();
            }

            return View(scorecardTest);
        }

        [HttpPost]
        public IActionResult EditScorecardTest(ScorecardTestModel test)
        {
            _database.ScorecardTests.Update(test);
            _database.SaveChanges();

            return RedirectToAction("ScorecardDetails", new { id = test.ScorecardID });
        }

        [HttpGet]
        public IActionResult RemoveScorecardTest(Guid id, Guid scorecardID)
        {
            var scorecardTest = _database.ScorecardTests.FirstOrDefault(t => t.ID == id && t.ScorecardID == scorecardID);

            if (scorecardTest != null)
            {
                _database.ScorecardTests.Remove(scorecardTest);
                _database.SaveChanges();
            }

            return RedirectToAction("ScorecardDetails", new { id = scorecardID });
        }

        [HttpGet]
        public IActionResult EditScorecardDetails(Guid id)
        {
            var scorecard = _database.Scorecards.Find(id);

            if (scorecard == null)
            {
                return NotFound();
            }

            return View(scorecard);
        }

        [HttpPost]
        public IActionResult EditScorecardDetails(ScorecardModel scorecard)
        {
            _database.Scorecards.Update(scorecard);
            _database.SaveChanges();

            return RedirectToAction("ScorecardDetails", new { id = scorecard.ID });
        }

        [HttpGet] 
        public IActionResult DeleteScorecard(Guid id)
        {
            var scorecard = _database.Scorecards.Find(id);

            if (scorecard != null)
            {
                _database.Scorecards.Remove(scorecard);

                var scorecardTests = _database.ScorecardTests.Where(t => t.ScorecardID == scorecard.ID).ToList();
                scorecardTests.ForEach(t =>
                {
                    _database.ScorecardTests.Remove(t);
                });

                _database.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public IActionResult AddAllCategoryTests(Guid id, Guid categoryID)
        {
            var scorecard = _database.Scorecards.Find(id);
            scorecard.Tests = _database.ScorecardTests.Where(t => t.ScorecardID == id).ToList();

            var categoryTests = _database.CategoryTests.Where(t => t.CategoryID == categoryID).ToList();

            foreach (var test in categoryTests.ToList())
            {
                var existing = scorecard.Tests.FirstOrDefault(t => t.ID == test.ID);
                if (existing == null)
                {
                    var scorecardTest = ScorecardTestModel.FromCategoryTest(test, scorecard.ID);
                    _database.ScorecardTests.Add(scorecardTest);
                }
            }

            _database.SaveChanges();
            
            return RedirectToAction("ScorecardDetails", new { id = id });
        }
    }
}