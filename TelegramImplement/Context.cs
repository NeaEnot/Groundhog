using Core.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace TelegramImplement
{
    internal class Context
    {
        private static Context instance;

        public List<Accaunt> Accaunts { get; private set; }
        public List<Task> Tasks { get; private set; }
        public List<TaskInstance> TaskInstances { get; private set; }

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
            SaveToFile(Accaunts);
            SaveToFile(Tasks);
            SaveToFile(TaskInstances);
        }

        internal void Load()
        {
            Accaunts = LoadFromFile<Accaunt>();
            Tasks = LoadFromFile<Task>();
            TaskInstances = LoadFromFile<TaskInstance>();
        }

        private void SaveToFile<T>(List<T> list)
        {
            using (StreamWriter writer = new StreamWriter($"{typeof(T).Name}s.json"))
            {
                string json = JsonConvert.SerializeObject(list);
                writer.Write(json);
            }
        }

        private List<T> LoadFromFile<T>()
        {
            try
            {
                using (StreamReader reader = new StreamReader($"{typeof(T).Name}s.json"))
                {
                    string json = reader.ReadToEnd();
                    List<T> restored = JsonConvert.DeserializeObject<List<T>>(json);
                    return restored ?? new List<T>();
                }
            }
            catch
            {
                return new List<T>();
            }
        }
    }
}
