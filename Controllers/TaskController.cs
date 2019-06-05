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
                AvailableTasks = _database.Tasks.Where(t => (t.Owner == null || t.Owner == default(Guid)) && t.Status != Strings.TaskStatus.Closed),
                UserTasks = _database.Tasks.Where(t => t.Owner == Utils.GetUserID(User) && t.Status == Strings.TaskStatus.Open)
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var viewModel = new CreateTaskViewModel
            {
                UserOptions = new SelectList(_database.AspNetUsers.ToList(), "Id", "UserName"),
                TaskTypeOptions = new SelectList(EnumExtensions.Tasks.GetTaskTypes(), "Value", "Name")
            };

            viewModel.TaskTypeOptions.First().Selected = true;

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Create(string taskType)
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateTaskViewModel viewModel)
        {
            var task = new TaskModel
            {
                Created = DateTime.Now,
                CreatedBy = User.Identity.Name,
                Status = Strings.TaskStatus.Open,
                Description = viewModel.Description,
                Owner = viewModel.SelectedOwner,
                History = new List<TaskHistoryModel>()
            };

            _database.Tasks.Add(task);

            var historyEntry = task.AddHistory($"Created by {User.Identity.Name}");

            _database.TaskHistory.Add(historyEntry);
            _database.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Details(Guid id, bool? viewHistory = null, bool? addComment = null)
        {
            var task = _database.Tasks.Find(id);

            if (task == null)
                return NotFound();

            var statusOptions = new List<string>()
            {
                Strings.TaskStatus.Open,
                Strings.TaskStatus.Closed,
                Strings.TaskStatus.InProgress
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
                AddComment = addComment == true,
                CommentBody = "",
            };

            if (task.Status == Strings.TaskStatus.Closed && viewHistory == null)
            {
                viewModel.ViewHistory = true;
            }

            viewModel.SetOriginalInfo();

            if (viewModel.ViewHistory)
            {
                viewModel.Task.History = _database.TaskHistory.Where(th => th.TaskID == task.ID).OrderBy(th => th.Timestamp).ToList();
                viewModel.Task.History.Reverse();
            }
            else
            {
                var comments = _database.TaskComments.Where(c => c.TaskID == task.ID).OrderBy(c => c.Timestamp).ToList();
                comments.Reverse();

                foreach (var comment in comments)
                {
                    var user = _database.AspNetUsers.Find(comment.UserID.ToString());

                    comment.UserName = user?.UserName ?? "";
                }

                viewModel.Comments = comments;
            }

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Details(TaskDetailsViewModel viewModel)
        {
            if (viewModel.Task.Status == Strings.TaskStatus.Closed)
                viewModel.Task.Completed = DateTime.Now;

            if (viewModel.AddComment && !string.IsNullOrEmpty(viewModel.CommentBody))
            {
                var comment = new TaskCommentModel
                {
                    Timestamp = DateTime.Now,
                    TaskID = viewModel.Task.ID,
                    UserID = Utils.GetUserID(User),
                    Body = viewModel.CommentBody
                };

                var commentHistory = viewModel.Task.AddHistory($"{User.Identity.Name} added a comment");

                _database.TaskComments.Add(comment);
                _database.TaskHistory.Add(commentHistory);
            }

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
                ClosedTasks = _database.Tasks.Where(t => t.Status == Strings.TaskStatus.Closed),
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