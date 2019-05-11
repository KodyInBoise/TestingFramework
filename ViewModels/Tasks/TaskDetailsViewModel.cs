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
        public SelectList StatusOptions { get; set; }
        public SelectList UserOptions { get; set; }
        public string OwnerName { get; set; }

        public bool IsClosed()
        {
            return Task.Status == Strings.Status.Closed;
        }

        public bool HasHowner()
        {
            return Utils.ValidateGuid(Task.Owner);
        }
    }
}
