using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TestingFramework.Data;
using TestingFramework.Extensions;
using TestingFramework.Models;
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

            var identityUsers = _database.AspNetUsers.ToList();
            var userInfos = new List<UserInfoModel>();

            foreach (var user in identityUsers)
            {
                try
                {
                    userInfos.Add(RoleHelper.GetUserInfo(_database, Guid.Parse(user.Id)));
                }
                catch (Exception ex)
                {
                    LoggingUtil.AddException(ex);
                }
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
            if (!RoleHelper.UserIsAdmin(_database, User))
            {
                return RedirectToAction("Index", "Home");
            }

            var roles = _database.Roles.ToList();

            var currentUser = RoleHelper.GetContextUserInfo(_database, User);
            var currentUserRole = roles.FirstOrDefault(r => r.ID == currentUser.RoleID);

            var viewModel = new EditUserViewModel
            {
                UserInfo = _database.UserInfos.Find(id),
            };

            viewModel.CurrentRole = roles.FirstOrDefault(r => r.ID == viewModel.UserInfo.RoleID);
            roles.Remove(viewModel.CurrentRole);

            viewModel.RoleOptions = new SelectList(roles, "ID", "Name");

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult EditUser(EditUserViewModel viewModel)
        {
            if (!RoleHelper.UserIsAdmin(_database, User))
            {
                return RedirectToAction("Index", "Home");
            }

            var user = _database.UserInfos.Find(viewModel.UserInfo.UserID);

            user.RoleID = viewModel.SelectedRoleID;

            _database.UserInfos.Update(user);
            _database.SaveChanges();

            return RedirectToAction("Users");
        }

        [HttpGet]
        public IActionResult DeleteUser(Guid id)
        {
            // This needs to be a confirmation page with deletion on post
            var userInfo = _database.UserInfos.Find(id);
            var identityUser = _database.AspNetUsers.Find(id.ToString());

            if (userInfo == null)
            {
                return NotFound();
            }

            if (identityUser != null)
                _database.AspNetUsers.Remove(identityUser);

            _database.UserInfos.Remove(userInfo);
            _database.SaveChanges();

            LoggingUtil.AddEntry(User.Identity.Name, $"Deleted user {userInfo.Name}");

            return RedirectToAction("Users");
        }
    }
}