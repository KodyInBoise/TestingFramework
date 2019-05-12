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
                AvailableTasks = _database.Tasks.Where(t => (t.Owner == null || t.Owner == default(Guid)) && t.Status != Strings.Status.Closed),
                UserTasks = _database.Tasks.Where(t => t.Owner == Utils.GetUserID(User) && t.Status == Strings.Status.Open)
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
                Status = Strings.Status.Open,
                Description = viewModel.Description,
                Owner = viewModel.SelectedOwner,
                History = new List<TaskHistoryModel>()
            };

            _database.Tasks.Add(task);

            var historyEntry = task.AddHistory($"{User.Identity.Name} created new task");

            _database.TaskHistory.Add(historyEntry);
            _database.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Details(Guid id, bool? viewHistory = null)
        {
            var task = _database.Tasks.Find(id);

            if (task == null)
                return NotFound();

            var statusOptions = new List<string>()
            {
                Strings.Status.Open,
                Strings.Status.Closed,
                Strings.Status.InProgress
            };

            var owner = _database.AspNetUsers.FirstOrDefault(u => u.Id == task.Owner.ToString());
            var users = _database.AspNetUsers.ToList();
            
            if (owner != null)
            {
                users.Remove(owner);
            }

            var viewModel = new TaskDetailsViewModel
            {
                Task = task,
                StatusOptions = new SelectList(statusOptions),
                UserOptions = new SelectList(users, "Id", "UserName"),
                OwnerName = owner?.UserName ?? "",
                ViewHistory = viewHistory == true,
            };

            if (task.Status == Strings.Status.Closed && viewHistory == null)
            {
                viewModel.ViewHistory = true;
            }

            viewModel.SetOriginalInfo();

            if (viewModel.ViewHistory)
            {
                viewModel.Task.History = _database.TaskHistory.Where(th => th.TaskID == task.ID).ToList();
                viewModel.Task.History.OrderBy(th => th.Timestamp).Reverse();
            }

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Details(TaskDetailsViewModel viewModel)
        {
            if (viewModel.Task.Status == Strings.Status.Closed)
                viewModel.Task.Completed = DateTime.Now;

            var updateHistory = viewModel.CreateUpdateHistory(User.Identity.Name);
            updateHistory.ForEach(uh => _database.TaskHistory.Add(uh));

            _database.Tasks.Update(viewModel.Task);
            _database.SaveChanges();

            return RedirectToAction("Details", new { id = viewModel.Task.ID });
        }

        [HttpGet]
        public IActionResult Closed()
        {
            var viewModel = new ClosedTasksViewModel
            {
                ClosedTasks = _database.Tasks.Where(t => t.Status == Strings.Status.Closed),
                UserNames = new Dictionary<Guid, string>()
            };

            var users = _database.Users.ToList();
            users.ForEach(u =>
            {
                viewModel.UserNames.Add(Guid.Parse(u.Id), u.UserName);
            });

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult RemoveOwner(Guid id)
        {
            var task = _database.Tasks.Find(id);

            if (task == null)
            {
                return NotFound();
            }

            task.Owner = default(Guid);

            _database.Tasks.Update(task);
            _database.SaveChanges();

            return RedirectToAction("Details", new { id = id });
        }
    }
}