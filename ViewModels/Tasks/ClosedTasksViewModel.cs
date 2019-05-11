using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestingFramework.Models.Tasks;

namespace TestingFramework.ViewModels.Tasks
{
    public class ClosedTasksViewModel
    {
        public IEnumerable<TaskModel> ClosedTasks { get; set; }
        public Dictionary<Guid, string> UserNames { get; set; }

        public string GetUserName(Guid? id)
        {
            UserNames.TryGetValue((Guid)id, out var name);

            return name ?? "";
        }
    }
}
