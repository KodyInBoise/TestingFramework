using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestingFramework.Extensions;
using TestingFramework.Models.Tasks;

namespace TestingFramework.ViewModels.Tasks
{
    public class TaskDetailsViewModel
    {
        public TaskModel Task { get; set; }
        public SelectList StatusOptions { get; set; }
        public SelectList UserOptions { get; set; }
        public string OwnerName { get; set; }
        public bool ViewHistory { get; set; }

        public string OriginalDescription { get; set; }
        public string OriginalStatus { get; set; }
        public Guid? OriginalOwner { get; set; }

        
        public void SetOriginalInfo()
        {
            // This is used for comparing & creating update history
            OriginalDescription = Task.Description;
            OriginalStatus = Task.Status;
            OriginalOwner = Task.Owner;
        }

        public bool IsClosed()
        {
            return Task.Status == Strings.Status.Closed;
        }

        public bool HasHowner()
        {
            return Utils.ValidateGuid(Task.Owner);
        }

        public List<TaskHistoryModel> CreateUpdateHistory(string userName)
        {
            var updates = new List<TaskHistoryModel>();

            if (Task.Description != OriginalDescription)
            {
                var entry = Task.AddHistory($"{userName} updated the description: {Task.Description}");
                updates.Add(entry);
            }
            if (Task.Status != OriginalStatus)
            {
                var entry = Task.AddHistory($"{userName} set the status to {Task.Status}");
                updates.Add(entry);
            }
            if (Task.Owner != OriginalOwner)
            {
                var entry = Task.AddHistory($"{userName} updated the owner: {OwnerName}");
                updates.Add(entry);
            }

            return updates;
        }
    }    
}
