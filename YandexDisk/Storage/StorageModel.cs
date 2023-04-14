using Core.Models.Settings;
using Core.Models.Storage;
using System.Collections.Generic;

namespace YandexDisk.Storage
{
    internal class StorageModel
    {
        public List<Task> Tasks { get; set; }
        public List<TaskInstance> TaskInstances { get; set; }
        public List<Purpose> Purposes { get; set; }
        public List<PurposeGroup> PurposeGroups { get; set; }
        public List<Note> Notes { get; set; }
        public AppSettings AppSettings { get; set; }

        internal StorageModel()
        {
            Tasks = new List<Task>();
            TaskInstances = new List<TaskInstance>();
            Purposes = new List<Purpose>();
            PurposeGroups = new List<PurposeGroup>();
            Notes = new List<Note>();
            AppSettings = new AppSettings();
        }
    }
}
