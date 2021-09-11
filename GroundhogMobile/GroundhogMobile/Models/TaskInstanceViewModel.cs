using Core;
using System;

namespace GroundhogMobile.Models
{
    internal class TaskInstanceViewModel
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public string TaskId { get; set; }
        public bool Completed { get; set; }
        //public string Text { get { return GroundhogContext.TaskLogic.Read(TaskId).Text; } }
        public string Text { get; set; }
    }
}
