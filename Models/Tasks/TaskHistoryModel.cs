using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestingFramework.Models.Tasks
{
    public class TaskHistoryModel : IHistoryEntry
    {
        public Guid ID { get; set; }
        public Guid TaskID { get; set; }
        public DateTime Timestamp { get; set; }
        public string Body { get; set; }
    }
}
