using Core.Interfaces;
using System;

namespace Core.Models
{
    public class TaskInstance : IHashable
    {
        public string Id { get; set; }
        public string TaskId { get; set; }
        public DateTime Date { get; set; }
        public bool Completed { get; set; }

        public int GetHash()
        {
            return (Id + TaskId + Date + Completed).GetHashCode();
        }
    }
}
