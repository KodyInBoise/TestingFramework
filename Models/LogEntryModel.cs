using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestingFramework.Interfaces;

namespace TestingFramework.Models
{
    public class LogEntryModel : ILogEntry
    {
        public DateTime Timestamp { get; set; }
        public string UserName { get; set; }
        public string Content { get; set; }
    }
}
