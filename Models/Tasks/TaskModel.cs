using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestingFramework.Extensions;

namespace TestingFramework.Models.Tasks
{
    public class TaskModel
    {
        public Guid ID { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public Guid? Owner { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime Completed { get; set; }
        public List<TaskHistoryModel> History { get; set; }


        public void AddHistory(string body)
        {
            var entry = new TaskHistoryModel
            {
                TaskID = ID,
                Timestamp = DateTime.Now,
                Body = body
            };

            History.Add(entry);
        }
    }
}
