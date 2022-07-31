using Core;
using Core.Models;
using Newtonsoft.Json;
using StorageFile.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace StorageFile
{
    internal class Context
    {
        private static Context instance;

        internal List<Task> Tasks { get; set; }
        internal List<TaskInstance> TaskInstances { get; set; }
        internal List<Purpose> Purposes { get; set; }
        internal List<PurposeGroup> PurposeGroups { get; set; }

        private Dictionary<string, int> hashes;

        private Context()
        {
            Load();

            hashes = new Dictionary<string, int>
            {
                { "Tasks", Tasks.GetHash() },
                { "TaskInstances", TaskInstances.GetHash() },
                { "Purposes", Purposes.GetHash() },
                { "PurposeGroups", PurposeGroups.GetHash() },
            };
        }

        internal static Context Instanse
        {
            get
            {
                if (instance == null)
                    instance = new Context();

                return instance;
            }
        }

        private void Save<T>(List<T> models)
        {
            string path = $@"{GroundhogContext.StoragePath}{typeof(T).Name}s.json";
            using (StreamWriter writer = new StreamWriter(path))
            {
                string json = JsonConvert.SerializeObject(models);
                writer.Write(json);
            }
        }

        private List<T> Load<T>()
        {
            try
            {
                string path = $@"{GroundhogContext.StoragePath}\{typeof(T).Name}s.json";
                using (StreamReader reader = new StreamReader(path))
                {
                    string json = reader.ReadToEnd();
                    List<T> restored = JsonConvert.DeserializeObject<List<T>>(json);

                    return restored ?? new List<T>();
                }
            }
            catch (Exception ex)
            {
                return new List<T>();
            }
        }

        internal async void Save()
        {
            if (Tasks.GetHash() != hashes["Tasks"])
            {
                hashes["Tasks"] = Tasks.GetHash();
                Save(Tasks);
            }
            if (TaskInstances.GetHash() != hashes["TaskInstances"])
            {
                hashes["TaskInstances"] = TaskInstances.GetHash();
                Save(TaskInstances);
            }
            if (PurposeGroups.GetHash() != hashes["PurposeGroups"])
            {
                hashes["PurposeGroups"] = PurposeGroups.GetHash();
                Save(PurposeGroups);
            }
            if (Purposes.GetHash() != hashes["Purposes"])
            {
                hashes["Purposes"] = Purposes.GetHash();
                Save(Purposes);
            }
        }

        internal void Load()
        {
            Tasks = Load<Task>();
            TaskInstances = Load<TaskInstance>();
            Purposes = Load<Purpose>();
            PurposeGroups = Load<PurposeGroup>();
        }
    }
}
