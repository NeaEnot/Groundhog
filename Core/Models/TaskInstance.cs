using System;

namespace Core.Models
{
    public class TaskInstance
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public string TaskId { get; set; }
        public bool Completed { get; set; }
    }
}
