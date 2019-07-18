using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestingFramework.ViewModels.Tasks
{
    public class CreateTaskViewModel
    {
        public string Description { get; set; }
        public SelectList UserOptions { get; set; }
        public Guid SelectedOwner { get; set; }
        public SelectList TaskTypeOptions { get; set; }
        public int SelectedTaskType { get; set; }
        public Guid TaskObjectID { get; set; }
    }
}
