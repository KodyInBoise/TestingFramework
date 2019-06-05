using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestingFramework.Extensions
{
    public class EnumExtensions
    {
        public static class Tasks
        {
            public static TaskType GeneralTask => GeneralTaskType();
            public static TaskType ScorecardTask => ScorecardTaskType();

            public static List<TaskType> GetTaskTypes()
            {
                return new List<TaskType>()
                {
                    GeneralTask, 
                    ScorecardTask,
                };
            }

            public static TaskType FromValue(int value)
            {
                return GetTaskTypes().FirstOrDefault(t => t.Value == value);
            }

            public static TaskType FromName(string name)
            {
                return GetTaskTypes().FirstOrDefault(t => t.Name == name);
            }

            static TaskType GeneralTaskType()
            {
                return new TaskType
                {
                    Value = 0,
                    Name = "General",
                };
            }

            static TaskType ScorecardTaskType()
            {
                return new TaskType
                {
                    Value = 1,
                    Name = "Scorecard",
                };
            }

            public class TaskType
            {
                public int Value { get; set; }
                public string Name { get; set; }
                public Guid ObjectID { get; set; }
            }
        }      
    }
}
