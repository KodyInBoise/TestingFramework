using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TestingFramework.Data;
using TestingFramework.Extensions;
using TestingFramework.Models.Tasks;
using TestingFramework.ViewModels.Tasks;

namespace TestingFramework.Controllers
{
    public class TaskController : Controller
    {
        readonly ApplicationDbContext _database;

        public TaskController([FromServices] ApplicationDbContext db)
        {
            _database = db;
        }

        public IActionResult Index()
        {
            var viewModel = new TasksHomeViewModel
            {
                OpenTasks = _database.Tasks.Where(t => t.Open == true),
                UserTasks = _database.Tasks.Where(t => t.Owner == Utils.GetUserID(User))
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var viewModel = new CreateTaskViewModel
            {
                UserOptions = new SelectList(_database.AspNetUsers.ToList(), "Id", "UserName")
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Create(CreateTaskViewModel viewModel)
        {
            var task = new TaskModel
            {
                Created = DateTime.Now,
                CreatedBy = User.Identity.Name,
                Open = true,
                Description = viewModel.Description,
                Owner = viewModel.SelectedOwner
            };

            _database.Tasks.Add(task);
            _database.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}