using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestingFramework.Models
{
    public interface IHistoryEntry
    {
        Guid ID { get; set; }
        DateTime Timestamp { get; set; }
        string Body { get; set; }
    }
}
