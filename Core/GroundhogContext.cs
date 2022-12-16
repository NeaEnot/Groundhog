using Core.Interfaces;
using Core.Interfaces.Storage;
using Core.Models;
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

        public static string[] Languages
        {
            get
            {
                DirectoryInfo langsDir = new DirectoryInfo($"{StoragePath}\\Languages");
                if (!langsDir.Exists)
                    langsDir.Create();

                FileInfo[] files = langsDir.GetFiles("*.lng");
                if (files.Length == 0)
                { }

                string[] languages = files.Select(req => req.Name.Replace("", "")).ToArray();

                return languages;
            }
        }

        public static void SaveSettings()
        {
            using (StreamWriter writer = new StreamWriter($"{StoragePath}\\AppSettings.json"))
            {
                string json = JsonConvert.SerializeObject(Settings);
                writer.Write(json);
            }
        }

        public static void LoadLanguage(string language)
        {
            settings.Language = Language.ReadFromFile($"{StoragePath}\\Languages\\{language}.lng");
        }

        private static void GenerateDefaultLanguage()
        {
            Language language = new Language
            {
                Settings = "Settings",
                ConnectionString = "Connection string",
                PlanningAndOptimizationRange = "Planning and optimization range",
                ColorSchema = "Color schema",
                Syncronization = "Syncronization",
                Download = "Download",
                Upload = "Upload",
                Tasks = "Tasks",
                Purposes = "Purposes",
                Notes = "Notes",
                Task = "Task",
                RepeatMode = "Repeat mode",
                TransferTaskToNextDay = "Transfer task to next day",
                OffsetNextTasks = "Offset next tasks",
                PlanningRange = "Planning range",
                OptimizationRange = "Optimization range",
                Save = "Save",
                Duplicate = "Duplicate",
                Update = "Update",
                Delete = "Delete",
                DeleteAllInstances = "Delete all instances",
                List = "List",
                Calendar = "Calendar",
                DaysPlanning = "Days planning",
                DaysOfWeekPlanning = "Days of week planning",
                WatchesPlanning = "Watches planning",
                DaysOfMonthPlanning = "Days of month planning",
                DaysOfYearPlanning = "Days of year planning",
                Optimization = "Optimization",
                MainColor = "Main color",
                AditionalColor = "Aditional color",
                MainText = "Main text",
                AditionalText = "Aditional text",
                SelectedItem = "Selected item",
                SelectedItemInactive = "Selected item inactive",
                ChosenItem = "Chosen item",
                StandartSchema = "Standart schema",
                EnterCode = "Enter code",
                Send = "Send",
                Create = "Create",
                Purpose = "Purpose",
                PurposesGroup = "Purposes group",
                GroupName = "Group name",
                Note = "Note",
                NoteName = "Note name",
                SearchedTextNotFounded = "Searched text not founded",
                Error = "Error",
                FieldMustBeFilled = "Field must be filled",
                FieldsMustBeFilled = "Fields must be filled",
                CodeWasNotEntered = "Code was not entered",
                StringNotMatchColorHexFormat = "String not match color hex format",
                ConnectionStringNotMatchFormat = "Connection string not match format",
                CodeWasNotReceived = "Code was not received",
                EntityWithSameIdDontExist = "Entity with same id dont exist",
                CorrectValue = "Correct value",
                CorrectFormat = "Correct format",
                IncorrectValue = "Incorrect value",
                IncorrectNumberOfDays = "Incorrect number of days",
                IncorrectFormatOfDayOfMonth = "Incorrect format of day of month",
                IncorrectNumberOfMonth = "Incorrect number of month",
                IncorrectNumberOfDay = "Incorrect number of day",
                ThereAreFewerDaysInSpecifiedMonth = "There are fewer days in specified month",
                IncorrectDayOfTheWeek = "Incorrect day of the week",
                IncorrectFormat = "Incorrect format",
                IncorrectNumberOfArguments = "Incorrect number of arguments",
                Monday = "Monday",
                Tuesday = "Tuesday",
                Wednes­day = "Wednes­day",
                Thursday = "Thursday",
                Friday = "Friday",
                Saturday = "Saturday",
                Sunday = "Sunday",
                MondayAbbreviated = "Mon.",
                TuesdayAbbreviated = "Tue.",
                Wednes­dayAbbreviated = "Wed.",
                ThursdayAbbreviated = "Thur.",
                FridayAbbreviated = "Fri.",
                SaturdayAbbreviated = "Sat.",
                SundayAbbreviated = "Sun."
            };

            language.SaveToFile($"{StoragePath}\\Languages\\Englis.lng");
        }
    }
}
