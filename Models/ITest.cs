using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestingFramework.Models
{
    public interface ITest
    {
        Guid ID { get; set; }
        Guid CategoryID { get; set; }
        string Description { get; set; }
        string ExpectedResult { get; set; }
    }
}
