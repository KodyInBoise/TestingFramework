using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestingFramework.Extensions
{
    public class Strings
    {
        public class TaskStatus
        {
            public const string Open = "Open";
            public const string Closed = "Closed";
            public const string InProgress = "In Progress";
        }

        public class TestStatus
        {
            public const string Passed = "Passed";
            public const string Failed = "Failed";
            public const string NotStarted = "NotStarted";

            public static string GetString(bool? passed)
            {
                return passed == true ? Passed : Failed ?? NotStarted;
            }

            public static bool? FromString(string status)
            {
                switch (status)
                {
                    case Passed:
                        return true;
                    case Failed:
                        return false;
                    default:
                        return null;
                }
            }
        }


        public static string ImportTests = "Test details should be entered in the following format: \r \r" + 
            "Category, Description, Expected Result, Value";
    }
}
