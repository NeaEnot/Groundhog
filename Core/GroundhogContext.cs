using Core.Enums;
using Core.Interfaces;
using Core.Interfaces.Storage;
using Core.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Core
{
    public static class GroundhogContext
    {
        public static ITaskInstanceLogic TaskInstanceLogic { get; set; }
        public static ITaskLogic TaskLogic { get; set; }
        public static IPurposeLogic PurposeLogic { get; set; }
        public static IPurposeGroupLogic PurposeGroupLogic { get; set; }
        public static INoteLogic NoteLogic { get; set; }

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

        public static string ConnectionString
        { 
            get
            {
                return Settings.ConnectionString;
            }
            set
            {
                Settings.ConnectionString = value != null ? value : "";
                SaveSettings();
            }
        }

        public static int OptimizationRange
        {
            get
            {
                return Settings.OptimizationRange;
            }
            set
            {
                Settings.OptimizationRange = value;
                SaveSettings();
            }
        }

        public static bool IsColorSchemaExist =>
            Settings.ColorSchema.Colors.Keys.Count != 0;

        public static int GetPlanningRange(RepeatMode mode)
        {
            return Settings.PlanningRanges[mode];
        }

        public static void SetPlanningRanges(Dictionary<RepeatMode, int> dict)
        {
            foreach (RepeatMode mode in Enum.GetValues(typeof(RepeatMode)))
                Settings.PlanningRanges[mode] = dict[mode];

            SaveSettings();
        }

        public static string GetColor(string key)
        {
            return Settings.ColorSchema.Colors[key];
        }

        public static void SetColors(Dictionary<string, string> colors)
        {
            Settings.ColorSchema.Colors = colors;
            SaveSettings();
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

        private static void SaveSettings()
        {
            using (StreamWriter writer = new StreamWriter($"{StoragePath}\\AppSettings.json"))
            {
                string json = JsonConvert.SerializeObject(Settings);
                writer.Write(json);
            }
        }
    }
}
