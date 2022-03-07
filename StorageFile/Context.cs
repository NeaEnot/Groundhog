using Core;
using Core.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace StorageFile
{
    internal class Context
    {
        private static Context instance;

        internal List<Task> Tasks { get; set; }
        internal List<TaskInstance> TaskInstances { get; set; }

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
                string json = JsonConvert.SerializeObject((Tasks, TaskInstances));
                writer.Write(json);
            }
        }

        internal void Load()
        {
            try
            {
                using (StreamReader reader = new StreamReader($@"{GroundhogContext.StoragePath}\storage.json"))
                {
                    string json = reader.ReadToEnd();
                    (List<Task>, List<TaskInstance>) restored = JsonConvert.DeserializeObject<(List<Task>, List<TaskInstance>)>(json);
                    Tasks = restored.Item1;
                    TaskInstances = restored.Item2;
                }
            }
            catch
            {
                Tasks = new List<Task>();
                TaskInstances = new List<TaskInstance>();
            }
        }
    }
}
