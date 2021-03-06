using Core.Models;
using System.Collections.Generic;

namespace StorageFile.Models
{
    internal class StorageModel
    {
        public List<Task> Tasks { get; set; }
        public List<TaskInstance> TaskInstances { get; set; }
        public List<Purpose> Purposes { get; set; }
        public List<PurposeGroup> PurposeGroups { get; set; }

        internal StorageModel()
        {
            Tasks = new List<Task>();
            TaskInstances = new List<TaskInstance>();
            Purposes = new List<Purpose>();
            PurposeGroups = new List<PurposeGroup>();
        }
    }
}
