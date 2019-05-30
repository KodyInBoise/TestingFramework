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
using TestingFramework.ViewModels.Setup;

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
        public IActionResult ScorecardDetails(Guid id, Guid testID = default(Guid))
        {
            var scorecard = _database.Scorecards.Find(id);

            if (scorecard == null)
            {
                return NotFound();
            }

            var viewModel = new ScorecardDetailsViewModel
            {
                Scorecard = scorecard,
                Categories = _database.Categories.ToList(),
                ScrollToDiv = Utils.ValidateGuid(testID) ? $"test-{testID}" : ""
            };

            var scorecardTests = _database.ScorecardTests.Where(t => t.ScorecardID == scorecard.ID).ToList();

            viewModel.Tests = Utils.Ordering.SortScorecardTests(scorecardTests);

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

            return RedirectToAction("ScorecardDetails", new { id = test.ScorecardID, testID = test.ID });
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

        [HttpGet]
        public IActionResult ImportTests()
        {
            var viewModel = new ImportTestsViewModel
            {
                Instructions = Strings.ImportTests,
                Separator = ','
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult ImportTests(ImportTestsViewModel viewModel)
        {
            var tests = Utils.Import.CreateTests(viewModel.Separator, viewModel.Text);
            var allCategories = _database.Categories.ToList();

            tests.ForEach(t =>
            {
                var category = _database.Categories.FirstOrDefault(c => c.Name == t.CategoryName);
                if (category == null)
                {
                    category = new CategoryModel
                    {
                        Name = t.CategoryName
                    };

                    _database.Categories.Add(category);
                    _database.SaveChanges();
                }

                t.CategoryID = category.ID;

                _database.CategoryTests.Add(t);
            });

            _database.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult EditCategoryTest(Guid id)
        {
            var test = _database.CategoryTests.Find(id);
            var categories = _database.Categories.ToList();

            if (test == null)
            {
                return NotFound();
            }

            var viewModel = new EditCategoryTestViewModel
            {
                SelectedCategoryID = test.CategoryID,
                CategoriesList = new SelectList(categories, "ID", "Name"),
                Test = test,
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult EditCategoryTest(EditCategoryTestViewModel viewModel)
        {
            var originalCategoryID = viewModel.Test.CategoryID;

            viewModel.Test.CategoryID = viewModel.SelectedCategoryID;

            _database.CategoryTests.Update(viewModel.Test);
            _database.SaveChanges();

            return RedirectToAction("CategoryDetails", new { id = originalCategoryID });
        }

        [HttpGet]
        public IActionResult DeleteCategory(Guid id)
        {
            var category = _database.Categories.Find(id);

            if (category == null)
            {
                return NotFound();
            }

            _database.Categories.Remove(category);
            _database.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult EditCategoryDetails(Guid id)
        {
            var category = _database.Categories.Find(id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost]
        public IActionResult EditCategoryDetails(CategoryModel category)
        {
            _database.Categories.Update(category);
            _database.SaveChanges();

            return RedirectToAction("CategoryDetails", new { id = category.ID });
        }
    }
}