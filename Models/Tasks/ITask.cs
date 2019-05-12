using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestingFramework.Models.Tasks
{
    public interface ITask
    {
        DateTime Created { get; set; }
        string CreatedBy { get; set; }
        Guid? Owner { get; set; }
        string Description { get; set; }
        string Status { get; set; }
        DateTime Completed { get; set; }
    }
}
