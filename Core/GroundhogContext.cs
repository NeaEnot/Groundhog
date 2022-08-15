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
        public static AppSettings Settings { 
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
            set
            {
                settings = value;
                SaveSettings();
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

        public static bool IsColorSchemaExist (List<string> keys)
        {
            foreach (string key in keys)
                if (!Settings.ColorSchema.Colors.ContainsKey(key))
                    return false;

            return true;
        }

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
            foreach (string key in colors.Keys)
            {
                if (Settings.ColorSchema.Colors.ContainsKey(key))
                    Settings.ColorSchema.Colors[key] = colors[key];
                else
                    Settings.ColorSchema.Colors.Add(key, colors[key]);
            }

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
