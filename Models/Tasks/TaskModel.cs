using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestingFramework.Models.Tasks
{
    public class TaskModel
    {
        public Guid ID { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public Guid Owner { get; set; }
        public string Description { get; set; }
        public bool Open { get; set; }
        public DateTime Completed { get; set; }
    }
}
