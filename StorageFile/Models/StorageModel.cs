using Core.Models;
using System.Collections.Generic;

namespace StorageFile.Models
{
    internal class StorageModel
    {
        internal List<Task> Tasks { get; set; }
        internal List<TaskInstance> TaskInstances { get; set; }
        internal List<Purpose> Purposes { get; set; }
        internal List<PurposeGroup> PurposeGroups { get; set; }

        internal StorageModel()
        {
            Tasks = new List<Task>();
            TaskInstances = new List<TaskInstance>();
            Purposes = new List<Purpose>();
            PurposeGroups = new List<PurposeGroup>();
        }
    }
}
