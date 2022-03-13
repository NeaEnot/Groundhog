using Core;
using Core.Models;
using Newtonsoft.Json;
using StorageFile.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace StorageFile
{
    internal class Context
    {
        private static Context instance;
        private static StorageModel storage;

        internal List<Task> Tasks => storage.Tasks;
        internal List<TaskInstance> TaskInstances => storage.TaskInstances;
        internal List<Purpose> Purposes => storage.Purposes;
        internal List<PurposeGroup> PurposeGroups => storage.PurposeGroups;

        private Context()
        {
            Load();
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

        internal void Save()
        {
            using (StreamWriter writer = new StreamWriter($@"{GroundhogContext.StoragePath}\storage.json"))
            {
                string json = JsonConvert.SerializeObject(storage);
                writer.Write(json);
            }
        }

        internal void Load()
        {
            if (storage == null)
                storage = new StorageModel();

            try
            {
                using (StreamReader reader = new StreamReader($@"{GroundhogContext.StoragePath}\storage.json"))
                {
                    string json = reader.ReadToEnd();
                    StorageModel restored = JsonConvert.DeserializeObject<StorageModel>(json);

                    storage.Tasks = restored.Tasks;
                    storage.TaskInstances = restored.TaskInstances;
                    storage.Purposes = restored.Purposes;
                    storage.PurposeGroups = restored.PurposeGroups;
                }
            }
            catch (Exception ex)
            { }
        }
    }
}
