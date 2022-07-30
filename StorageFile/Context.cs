using Core;
using Core.Interfaces;
using Core.Models;
using Newtonsoft.Json;
using StorageFile.Extensions;
using StorageFile.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

        private Dictionary<object, int> hashes;

        private Context()
        {
            Load();

            hashes = new Dictionary<object, int>
            {
                { Tasks, Tasks.GetHash() },
                { TaskInstances, TaskInstances.GetHash() },
                { Purposes, Purposes.GetHash() },
                { PurposeGroups, PurposeGroups.GetHash() },
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

        private void Save<T>(List<T> models) where T : IHashable
        {
            int hash = models.GetHash();
            using (StreamWriter writer = new StreamWriter($@"{GroundhogContext.StoragePath}\{typeof(T).Name}s.json"))
            {
                string json = JsonConvert.SerializeObject(models);
                writer.Write(json);
            }
        }

        private List<T> Load<T>() where T : IHashable
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
            foreach (object key in hashes.Keys)
            {
                List<IHashable> list = key as List<IHashable>;
                int hash = list.GetHash();

                if (hash != hashes[key])
                {
                    Save(list);
                    hashes[key] = hash;
                }
            }
        }

        internal void Load()
        {
            if (storage == null)
                storage = new StorageModel();

            Tasks = Load<Task>();
            TaskInstances = Load<TaskInstance>();
            Purposes = Load<Purpose>();
            PurposeGroups = Load<PurposeGroup>();
        }
    }
}
