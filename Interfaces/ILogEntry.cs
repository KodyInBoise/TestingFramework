using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestingFramework.Interfaces
{
    public interface ILogEntry
    {
        DateTime Timestamp { get; set; }
        string UserName { get; set; }
        string Content { get; set; }
    }
}
