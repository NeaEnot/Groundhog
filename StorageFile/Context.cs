using Core;
using Core.Models;
using Newtonsoft.Json;
using StorageFile.Extensions;
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

        internal List<Task> Tasks { get => storage.Tasks; set { storage.Tasks = value; Save(storage.Tasks); } }
        internal List<TaskInstance> TaskInstances { get => storage.TaskInstances; set { storage.TaskInstances = value; Save(storage.TaskInstances); } }
        internal List<Purpose> Purposes { get => storage.Purposes; set { storage.Purposes = value; Save(storage.Purposes); } }
        internal List<PurposeGroup> PurposeGroups { get => storage.PurposeGroups; set { storage.PurposeGroups = value; Save(storage.PurposeGroups); } }

        private Context()
        {
            //Load();
            storage = new StorageModel();
            Tasks = Load<Task>();
            TaskInstances = Load<TaskInstance>();
            Purposes = Load<Purpose>();
            PurposeGroups = Load<PurposeGroup>();
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
            int hash = models.GetHash();
            using (StreamWriter writer = new StreamWriter($@"{GroundhogContext.StoragePath}\{typeof(T).Name}s.json"))
            {
                string json = JsonConvert.SerializeObject(models);
                writer.Write(json);
            }
        }

        private List<T> Load<T>()
        {
            try
            {
                using (StreamReader reader = new StreamReader($@"{GroundhogContext.StoragePath}\{typeof(T).Name}s.json"))
                {
                    string json = reader.ReadToEnd();
                    List<T> restored = JsonConvert.DeserializeObject<List<T>>(json);

                    return restored;
                }
            }
            catch (Exception ex)
            {
                return new List<T>();
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
