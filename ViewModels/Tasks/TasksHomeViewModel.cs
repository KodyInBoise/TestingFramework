using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestingFramework.Models.Tasks;

namespace TestingFramework.ViewModels.Tasks
{
    public class TasksHomeViewModel
    {
        public IEnumerable<TaskModel> OpenTasks { get; set; }
        public IEnumerable<TaskModel> UserTasks { get; set; }
    }
}
