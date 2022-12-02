using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Core.Models
{
    public class Language
    {
        // Settings
        public string Settings { get; set; }
        public string ConnectionString { get; set; }
        public string PlanningAndOptimizationRange { get; set; }
        // Settings - Color schema
        public string ColorSchema { get; set; }
        public string StandartSchema { get; set; }
        public string MainColor { get; set; }
        public string AditionalColor { get; set; }
        public string MainText { get; set; }
        public string AditionalText { get; set; }
        public string SelectedItem { get; set; }
        public string SelectedItemInactive { get; set; }
        public string ChosenItem { get; set; }
        // Syncronization
        public string Syncronization { get; set; }
        public string Download { get; set; }
        public string Upload { get; set; }
        public string EnterCode { get; set; }
        public string Send { get; set; }
        // Tasks
        public string List { get; set; }
        public string Calendar { get; set; }
        public string Tasks { get; set; }
        public string Task { get; set; }
        public string RepeatMode { get; set; }
        public string TransferTaskToNextDay { get; set; }
        public string OffsetNextTasks { get; set; }
        public string PlanningRange { get; set; }
        public string OptimizationRange { get; set; }
        // Planning modes and optimization
        public string NonePlanning { get; set; }
        public string DaysPlanning { get; set; }
        public string DaysOfWeekPlanning { get; set; }
        public string WatchesPlanning { get; set; }
        public string DaysOfMonthPlanning { get; set; }
        public string DaysOfYearPlanning { get; set; }
        public string Optimization { get; set; }
        // Purposes
        public string Purposes { get; set; }
        public string Purpose { get; set; }
        public string PurposesGroup { get; set; }
        public string GroupName { get; set; }
        // Notes
        public string Notes { get; set; }
        public string Note { get; set; }
        public string NoteName { get; set; }
        // Control commands
        public string Save { get; set; }
        public string Duplicate { get; set; }
        public string Update { get; set; }
        public string Delete { get; set; }
        public string DeleteAllInstances { get; set; }
        public string Create { get; set; }

        internal static Language ReadFromFile(string path)
        {
            string[] lines = File.ReadAllLines(path);

            IEnumerable<KeyValuePair<string, string>> pairs =
                lines
                .Where(req => !req.StartsWith("#"))
                .Select(req => new KeyValuePair<string, string>(req.Split('=')[0], req.Split('=')[1]));

            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> pair in pairs)
                dict.Add(pair.Key, pair.Value);

            Language language = new Language
            {
                Settings = dict["Settings"],
                ConnectionString = dict["ConnectionString"],
                PlanningAndOptimizationRange = dict["PlanningAndOptimizationRange"],
                ColorSchema = dict["ColorSchema"],
                Syncronization = dict["Syncronization"],
                Download = dict["Download"],
                Upload = dict["Upload"],
                Tasks = dict["Tasks"],
                Purposes = dict["Purposes"],
                Notes = dict["Notes"],
                Task = dict["Task"],
                RepeatMode = dict["RepeatMode"],
                TransferTaskToNextDay = dict["TransferTaskToNextDay"],
                OffsetNextTasks = dict["OffsetNextTasks"],
                PlanningRange = dict["PlanningRange"],
                OptimizationRange = dict["OptimizationRange"],
                Save = dict["Save"],
                Duplicate = dict["Duplicate"],
                Update = dict["Update"],
                Delete = dict["Delete"],
                DeleteAllInstances = dict["DeleteAllInstances"],
                List = dict["List"],
                Calendar = dict["Calendar"],
                DaysPlanning = dict["DaysPlanning"],
                DaysOfWeekPlanning = dict["DaysOfWeekPlanning"],
                WatchesPlanning = dict["WatchesPlanning"],
                DaysOfMonthPlanning = dict["DaysOfMonthPlanning"],
                DaysOfYearPlanning = dict["DaysOfYearPlanning"],
                Optimization = dict["Optimization"],
                MainColor = dict["MainColor"],
                AditionalColor = dict["AditionalColor"],
                MainText = dict["MainText"],
                AditionalText = dict["AditionalText"],
                SelectedItem = dict["SelectedItem"],
                SelectedItemInactive = dict["SelectedItemInactive"],
                ChosenItem = dict["ChosenItem"],
                StandartSchema = dict["StandartSchema"],
                EnterCode = dict["EnterCode"],
                Send = dict["Send"],
                Create = dict["Create"],
                Purpose = dict["Purpose"],
                PurposesGroup = dict["PurposesGroup"],
                GroupName = dict["GroupName"],
                Note = dict["Note"],
                NoteName = dict["NoteName"]
            };

            return language;
        }
    }
}
