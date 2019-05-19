using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestingFramework.Data;
using TestingFramework.Extensions;
using TestingFramework.Models;
using TestingFramework.ViewModels;
using TestingFramework.ViewModels.Scorecard;

namespace TestingFramework.Controllers
{
    public class HomeController : Controller
    {
        readonly ApplicationDbContext _database;

        public HomeController([FromServices] ApplicationDbContext db)
        {
            _database = db;
        }
        
        public IActionResult Index()
        {
            var viewModel = new HomeViewModel
            {
                Scorecards = _database.Scorecards.ToList(),
                ScorecardsInProgress = _database.ScorecardsInProgress.ToList(),
                ScorecardTests = _database.ScorecardTests.ToList(),
                UserTasks = _database.Tasks.Where(t => t.Owner == Utils.GetUserID(User) && t.Status == Strings.TaskStatus.Open)
            };

            foreach(var scorecard in viewModel.Scorecards)
            {
                scorecard.Tests = _database.ScorecardTests.Where(t => t.ScorecardID == scorecard.ID).ToList();
            }

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(Exception ex)
        {
            LoggingUtil.AddEntry("EXCEPTION", ex.Message);

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult StartScorecard(Guid id)
        {
            return RedirectToAction("StartScorecard", "Scorecard", new { id=id });
        }

        [HttpGet]
        public IActionResult Results()
        {
            return RedirectToAction("Results", "Scorecard");
        }

        [HttpGet]
        public IActionResult TaskDetails(Guid id)
        {
            return RedirectToAction("Details", "Task", new { id = id });
        }
    }
}
