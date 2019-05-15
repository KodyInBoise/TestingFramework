using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestingFramework.Interfaces
{
    public interface IScorecardResult
    {
        Guid ScorecardID { get; set; }
        string User { get; set; }
    }
}
