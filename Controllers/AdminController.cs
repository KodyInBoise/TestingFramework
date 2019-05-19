using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestingFramework.Data;
using TestingFramework.Extensions;

namespace TestingFramework.Controllers
{
    public class AdminController : Controller
    {
        readonly ApplicationDbContext _database;

        public AdminController([FromServices]ApplicationDbContext db)
        {
            _database = db;
        }

        public IActionResult Index()
        {
            if (!RoleHelper.UserIsAdmin(_database, User))
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        public IActionResult Logs()
        {
            if (!RoleHelper.UserIsAdmin(_database, User))
            {
                return RedirectToAction("Index", "Home");
            }

            var entries = LoggingUtil.GetCurrentLogEntries();

            return View(entries);
        }

        public IActionResult Users()
        {
            if (!RoleHelper.UserIsAdmin(_database, User))
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }
    }
}