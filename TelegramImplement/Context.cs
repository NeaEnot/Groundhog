using Core;
using Core.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using TLSharp.Core;

namespace TelegramImplement
{
    internal class Context
    {
        private static Context instance;

        private TelegramClient client;

        internal List<Accaunt> Accaunts { get; private set; }
        internal List<Task> Tasks { get; private set; }
        internal List<TaskInstance> TaskInstances { get; private set; }

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

        internal async void Load()
        {
            Accaunts = LoadFromFile<Accaunt>();
            await System.Threading.Tasks.Task.Run(() =>
            {
                Thread.Sleep(500);
                Tasks = LoadFromFile<Task>();
                TaskInstances = LoadFromFile<TaskInstance>();
            });
        }

        private void SaveToFile<T>(List<T> list)
        {
            using (StreamWriter writer = new StreamWriter($"{GroundhogContext.StoragePath}\\{typeof(T).Name}s.json"))
            {
                string json = JsonConvert.SerializeObject(list);
                writer.Write(json);
            }
        }

        private List<T> LoadFromFile<T>()
        {
            try
            {
                using (StreamReader reader = new StreamReader($"{GroundhogContext.StoragePath}\\{typeof(T).Name}s.json"))
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

        //private async System.Threading.Tasks.Task ConnectToTelegram()
        //{
        //    Accaunt accaunt = GroundhogContext.Accaunt;
        //    if (accaunt != null)
        //    {
        //        GroupCollection groups = GroundhogContext.AccauntLogic.ConnectionStringExpr.Match(accaunt.ConnectionString).Groups;

        //        int api_id = int.Parse(groups["api_id"].Value);
        //        string api_hash = groups["api_hash"].Value;
        //        string channel = groups["channel"].Value;

        //        client = new TelegramClient(api_id, api_hash);
        //        await client.ConnectAsync();
        //    }
        //}
    }
}
