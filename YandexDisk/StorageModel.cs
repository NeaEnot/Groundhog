using Core.Models;
using System.Collections.Generic;

namespace YandexDisk
{
    internal class StorageModel
    {
        public List<Task> Tasks { get; set; }
        public List<TaskInstance> TaskInstances { get; set; }
        public List<Purpose> Purposes { get; set; }
        public List<PurposeGroup> PurposeGroups { get; set; }
        public List<Note> Notes { get; set; }

        internal StorageModel()
        {
            Tasks = new List<Task>();
            TaskInstances = new List<TaskInstance>();
            Purposes = new List<Purpose>();
            PurposeGroups = new List<PurposeGroup>();
            Notes = new List<Note>();
        }
    }
}
