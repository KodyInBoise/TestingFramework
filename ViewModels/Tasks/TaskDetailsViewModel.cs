using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestingFramework.Extensions;
using TestingFramework.Models.Tasks;

namespace TestingFramework.ViewModels.Tasks
{
    public class TaskDetailsViewModel
    {
        public TaskModel Task { get; set; }
        public List<SelectListItem> StatusOptions { get; set; }

        public bool IsClosed()
        {
            return Task.Status == Strings.Status.Closed;
        }
    }
}
