using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TestingFramework.Data;
using TestingFramework.Extensions;
using TestingFramework.ViewModels.Admin;

namespace TestingFramework.Controllers
{
    public class AdminController : Controller
    {
        readonly ApplicationDbContext _database;

        public AdminController([FromServices]ApplicationDbContext db)
        {
            _database = db;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (!RoleHelper.UserIsAdmin(_database, User))
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpGet]
        public IActionResult Logs()
        {
            if (!RoleHelper.UserIsAdmin(_database, User))
            {
                return RedirectToAction("Index", "Home");
            }

            var entries = LoggingUtil.GetCurrentLogEntries();

            return View(entries);
        }

        [HttpGet]
        public IActionResult Users()
        {
            if (!RoleHelper.UserIsAdmin(_database, User))
            {
                return RedirectToAction("Index", "Home");
            }

            var viewModel = new UsersViewModel
            {
                UserInfos = _database.UserInfos.ToList(),
                Roles = _database.Roles.ToList()
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult EditUser(Guid id)
        {
            var roles = _database.Roles.ToList();

            var currentUser = RoleHelper.GetUserInfo(_database, User);
            var currentUserRole = roles.FirstOrDefault(r => r.ID == currentUser.RoleID);

            var viewModel = new EditUserViewModel
            {
                UserInfo = _database.UserInfos.Find(id),
            };

            viewModel.SelectedRole = roles.FirstOrDefault(r => r.ID == viewModel.UserInfo.RoleID);
            roles.Remove(viewModel.SelectedRole);

            viewModel.RoleOptions = new SelectList(roles, "ID", "Name");

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult EditUser(EditUserViewModel viewModel)
        {
            var user = _database.UserInfos.Find(viewModel.UserInfo.UserID);

            user.RoleID = viewModel.SelectedRole.ID;

            _database.UserInfos.Update(user);
            _database.SaveChanges();

            return RedirectToAction("Users");
        }
    }
}