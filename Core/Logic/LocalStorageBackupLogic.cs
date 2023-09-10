using Core.Interfaces.Network;
using Core.Models.Settings;
using Core.Models.Storage;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Core.Logic
{
    public class LocalStorageBackupLogic : IBackupLogic
    {
        public List<string> Backups =>
            Directory
            .GetFiles(GroundhogContext.StoragePath)
            .Where(f => f.EndsWith(".backup"))
            .Select(f => f.Replace(".backup", "").Replace(GroundhogContext.StoragePath + GroundhogContext.Split, ""))
            .ToList();

        public void MakeBackup(string key)
        {
            List<Task> tasks = GroundhogContext.TaskLogic.Read();
            List<TaskInstance> taskInstances = new List<TaskInstance>();
            foreach (Task task in tasks)
                taskInstances.AddRange(GroundhogContext.TaskInstanceLogic.Read(task.Id));
            List<PurposeGroup> groups = GroundhogContext.PurposeGroupLogic.Read();
            List<Purpose> purposes = new List<Purpose>();
            foreach (PurposeGroup group in groups)
                purposes.AddRange(GroundhogContext.PurposeLogic.Read(group.Id));
            List<Note> notes = GroundhogContext.NoteLogic.Read();

            StorageModel model = new StorageModel
            {
                Tasks = tasks,
                TaskInstances = taskInstances,
                PurposeGroups = groups,
                Purposes = purposes,
                Notes = notes,
                AppSettings = GroundhogContext.Settings
            };

            string path = $@"{GroundhogContext.StoragePath}{GroundhogContext.Split}{key}.backup";
            using (StreamWriter writer = new StreamWriter(path))
            {
                string json = JsonConvert.SerializeObject(model);
                writer.Write(json);
            }
        }

        public void RestoreBackup(string key)
        {
            string path = $@"{GroundhogContext.StoragePath}{GroundhogContext.Split}{key}.backup";
            StorageModel model;

            using (StreamReader reader = new StreamReader(path))
            {
                string json = reader.ReadToEnd();
                model = JsonConvert.DeserializeObject<StorageModel>(json);
            }

            GroundhogContext.TaskLogic.Delete(null);
            GroundhogContext.TaskLogic.Create(model.Tasks);
            GroundhogContext.TaskInstanceLogic.Delete();
            GroundhogContext.TaskInstanceLogic.Create(model.TaskInstances);
            GroundhogContext.PurposeGroupLogic.Delete(null);
            GroundhogContext.PurposeGroupLogic.Create(model.PurposeGroups);
            GroundhogContext.PurposeLogic.Delete();
            GroundhogContext.PurposeLogic.Create(model.Purposes);
            GroundhogContext.NoteLogic.Delete(null);
            GroundhogContext.NoteLogic.Create(model.Notes);

            if (model.AppSettings.ConnectionStringStorage != GroundhogContext.Settings.ConnectionStringStorage)
                model.AppSettings.ConnectionStringStorage = GroundhogContext.Settings.ConnectionStringStorage;
            if (model.AppSettings.ConnectionStringLanguage != GroundhogContext.Settings.ConnectionStringLanguage)
                model.AppSettings.ConnectionStringLanguage = GroundhogContext.Settings.ConnectionStringLanguage;

            GroundhogContext.Settings = model.AppSettings;
        }

        public void DeleteBackup(string key)
        {
            string path = $@"{GroundhogContext.StoragePath}{GroundhogContext.Split}{key}.backup";
            File.Delete(path);
        }

        private class StorageModel
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
}
