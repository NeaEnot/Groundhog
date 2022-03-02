using Core.Enums;
using Core.Interfaces;
using Core.Models;
using Newtonsoft.Json;
using System;
using System.IO;

namespace Core
{
    public static class GroundhogContext
    {
        public static ITaskInstanceLogic TaskInstanceLogic  { get; set; }
        public static ITaskLogic TaskLogic { get; set; }
        public static INetworkLogic NetworkLogic { get; set; }

        private static AppSettings settings;
        private static AppSettings Settings { 
            get
            {
                if (settings == null)
                {
                    try
                    {
                        using (StreamReader reader = new StreamReader($"{StoragePath}\\AppSettings.json"))
                        {
                            string json = reader.ReadToEnd();
                            settings = JsonConvert.DeserializeObject<AppSettings>(json);
                        }
                    }
                    catch
                    {
                        settings = new AppSettings();
                    }
                }

                return settings;
            }
        }

        public static string ConnectionString { 
            get
            {
                try
                {
                    return Settings.ConnectionString;
                }
                catch
                {
                    return null;
                }
            }
            set
            {
                Settings.ConnectionString = value != null ? value : "";
                using (StreamWriter writer = new StreamWriter($"{StoragePath}\\AppSettings.json"))
                {
                    string json = JsonConvert.SerializeObject(Settings);
                    writer.Write(json);
                }
            }
        }

        public static int GetPlanningRange(RepeatMode mode)
        {
            return Settings.PlanningRanges[mode];
        }

        public static void SetPlanningRange(RepeatMode mode, int value)
        {
            Settings.PlanningRanges[mode] = value;
        }

        public static string StoragePath
        {
            get
            {
                string system = System.Runtime.InteropServices.RuntimeInformation.OSDescription;
                string storagePath;

                if (system.Contains("Microsoft Windows"))
                    storagePath = "storage";
                else
                    storagePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

                DirectoryInfo storage = new DirectoryInfo(storagePath);
                if (!storage.Exists)
                    storage.Create();

                return storagePath;
            }
        }
    }
}
