using Core;
using Core.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace NetworkTelegram
{
    internal class Context
    {
        private static Context instance;

        internal List<Accaunt> Accaunts { get; set; }
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
            using (StreamWriter writer = new StreamWriter($"{GroundhogContext.StoragePath}\\storage.json"))
            {
                string json = JsonConvert.SerializeObject((Accaunts, Tasks, TaskInstances));
                writer.Write(json);
            }
        }

        internal void Load()
        {
            try
            {
                using (StreamReader reader = new StreamReader($"{GroundhogContext.StoragePath}\\storage.json"))
                {
                    string json = reader.ReadToEnd();
                    (List<Accaunt>, List<Task>, List<TaskInstance>) restored = JsonConvert.DeserializeObject<(List<Accaunt>, List<Task>, List<TaskInstance>)>(json);
                    Accaunts = restored.Item1;
                    Tasks = restored.Item2;
                    TaskInstances = restored.Item3;
                }
            }
            catch
            {
                Accaunts = new List<Accaunt>();
                Tasks = new List<Task>();
                TaskInstances = new List<TaskInstance>();
            }
        }
    }
}
