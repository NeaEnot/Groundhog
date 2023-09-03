using Core.Interfaces.Network;
using Core.Interfaces.Storage;
using Core.Logic;
using Core.Models.Settings;
using Core.Models.Settings.Lang;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;

namespace Core
{
    public static class GroundhogContext
    {
        public static ITaskInstanceLogic TaskInstanceLogic { get; set; }
        public static ITaskLogic TaskLogic { get; set; }
        public static IPurposeLogic PurposeLogic { get; set; }
        public static IPurposeGroupLogic PurposeGroupLogic { get; set; }
        public static INoteLogic NoteLogic { get; set; }

        public static INetworkLogic NetworkStorageLogic { get; set; }
        public static INetworkLogic NetworkLanguageLogic { get; set; }

        public static IBackupLogic LocalBackupLogic { get; set; }
        public static IBackupLogic CloudBackupLogic { get; set; }

        public static Language Language { get; set; }
        public static string DefaultLanguage => LanguageLogic.DefaultLanguage;

        private static AppSettings settings;
        public static AppSettings Settings { 
            get
            {
                if (settings == null)
                {
                    try
                    {
                        using (StreamReader reader = new StreamReader($"{StoragePath}{Split}AppSettings.json"))
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

        internal static char Split
        {
            get
            {
                string system = System.Runtime.InteropServices.RuntimeInformation.OSDescription;
                char split;

                if (system.Contains("Microsoft Windows"))
                    split = '\\';
                else
                    split = '/';

                return split;
            }
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

        public static string LanguagesPath
        {
            get
            {
                string system = System.Runtime.InteropServices.RuntimeInformation.OSDescription;
                string storagePath;

                if (system.Contains("Microsoft Windows"))
                    storagePath = $"{StoragePath}{Split}Languages";
                else
                    storagePath = StoragePath;

                DirectoryInfo storage = new DirectoryInfo(storagePath);
                if (!storage.Exists)
                    storage.Create();

                return storagePath;
            }
        }

        public static string[] Languages
        {
            get
            {
                DirectoryInfo langsDir = new DirectoryInfo(LanguagesPath);
                if (!langsDir.Exists)
                    langsDir.Create();

                FileInfo[] files = langsDir.GetFiles("*.lng");
                if (files.Length == 0)
                    LanguageLogic.CreateDefault();

                string[] languages = files.Select(req => req.Name.Replace(".lng", "")).ToArray();

                return languages;
            }
        }

        public static void SaveSettings()
        {
            using (StreamWriter writer = new StreamWriter($"{StoragePath}{Split}AppSettings.json"))
            {
                string json = JsonConvert.SerializeObject(Settings);
                writer.Write(json);
            }
        }

        public static Language LoadLanguage(string language)
        {
            Language lang = LanguageLogic.Load($"{LanguagesPath}{Split}{language}.lng");
            settings.Language = language;

            return lang;
        }
    }
}
