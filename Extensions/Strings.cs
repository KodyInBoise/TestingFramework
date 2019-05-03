using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestingFramework.Extensions
{
    public class Strings
    {
        public class Status
        {
            public const string Open = "Open";
            public const string Closed = "Closed";
            public const string InProgress = "In Progress";
        }


        public static string ImportTests = "Test details should be entered in the following format: \r \r" + 
            "Category, Description, Expected Result, Value";
    }
}
