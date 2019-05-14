using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TestingFramework.Models.Tasks
{
    public class TaskCommentModel
    {
        public Guid ID { get; set; }
        public Guid TaskID { get; set; }
        public Guid UserID { get; set; }
        public DateTime Timestamp { get; set; }
        public string Body { get; set; }

        [NotMapped]
        public string UserName { get; set; }


        public override string ToString()
        {
            return $"{Timestamp} | {UserName}: {Body}";
        }
    }
}
