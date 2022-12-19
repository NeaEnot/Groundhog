using System;

namespace Core.Models.Storage
{
    public class TaskInstance
    {
        public string Id { get; set; }
        public string TaskId { get; set; }
        public DateTime Date { get; set; }
        public bool Completed { get; set; }

        public override int GetHashCode()
        {
            return (Id + TaskId + Date + Completed).GetHashCode();
        }
    }
}
