﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestingFramework.Extensions;
using static TestingFramework.Extensions.EnumExtensions;

namespace TestingFramework.Models.Tasks
{
    public class TaskModel : ITask
    {
        public Guid ID { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public Guid? Owner { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime Completed { get; set; }
        public List<TaskHistoryModel> History { get; set; }
        public int TaskType { get; set; }
        public Guid TaskTypeObjectID { get; set; }


        public TaskHistoryModel AddHistory(string body)
        {
            var entry = new TaskHistoryModel
            {
                TaskID = ID,
                Timestamp = DateTime.Now,
                Body = body
            };

            if (History == null)
                History = new List<TaskHistoryModel>();

            History.Add(entry);

            return entry;
        }
    }
}
