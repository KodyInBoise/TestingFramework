using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestingFramework.Models;

namespace TestingFramework.ViewModels.Admin
{
    public class LogsViewModel
    {
        public DateTime ViewingDate { get; set; }
        public IEnumerable<LogEntryModel> Entries { get; set; }
    }
}
