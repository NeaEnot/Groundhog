﻿using System.Collections.Generic;
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
        public string SearchedTextNotFounded { get; set; }
        // Control commands
        public string Save { get; set; }
        public string Duplicate { get; set; }
        public string Update { get; set; }
        public string Delete { get; set; }
        public string DeleteAllInstances { get; set; }
        public string Create { get; set; }
        // Errors messages
        public string Error { get; set; }
        public string FieldMustBeFilled { get; set; }
        public string FieldsMustBeFilled { get; set; }
        public string CodeWasNotEntered { get; set; }
        public string StringNotMatchColorHexFormat { get; set; }
        public string ConnectionStringNotMatchFormat { get; set; }
        public string CodeWasNotReceived { get; set; }
        public string WhenCreatingMustPassNewObjectWithEmptyFields { get; set; }
        public string EntityWithSameIdDontExist { get; set; }
        public string CorrectValue { get; set; }
        public string CorrectFormat { get; set; }
        public string IncorrectValue { get; set; }
        public string IncorrectNumberOfDays { get; set; }
        public string IncorrectFormatOfDayOfMonth { get; set; }
        public string IncorrectNumberOfMonth { get; set; }
        public string IncorrectNumberOfDay { get; set; }
        public string ThereAreFewerDaysInSpecifiedMonth { get; set; }
        public string IncorrectDayOfTheWeek { get; set; }
        public string IncorrectFormat { get; set; }
        public string IncorrectNumberOfArguments { get; set; }
        // Days of week
        public string Monday { get; set; }
        public string Tuesday { get; set; }
        public string Wednes­day { get; set; }
        public string Thursday { get; set; }
        public string Friday { get; set; }
        public string Saturday { get; set; }
        public string Sunday { get; set; }
        public string MondayAbbreviated { get; set; }
        public string TuesdayAbbreviated { get; set; }
        public string Wednes­dayAbbreviated { get; set; }
        public string ThursdayAbbreviated { get; set; }
        public string FridayAbbreviated { get; set; }
        public string SaturdayAbbreviated { get; set; }
        public string SundayAbbreviated { get; set; }

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
                NoteName = dict["NoteName"],
                SearchedTextNotFounded = dict["SearchedTextNotFounded"],
                Error = dict["Error"],
                FieldMustBeFilled = dict["FieldMustBeFilled"],
                FieldsMustBeFilled = dict["FieldsMustBeFilled"],
                CodeWasNotEntered = dict["CodeWasNotEntered"],
                StringNotMatchColorHexFormat = dict["StringNotMatchColorHexFormat"],
                ConnectionStringNotMatchFormat = dict["ConnectionStringNotMatchFormat"],
                CodeWasNotReceived = dict["CodeWasNotReceived"],
                WhenCreatingMustPassNewObjectWithEmptyFields = dict["WhenCreatingMustPassNewObjectWithEmptyFields"],
                EntityWithSameIdDontExist = dict["EntityWithSameIdDontExist"],
                CorrectValue = dict["CorrectValue"],
                CorrectFormat = dict["CorrectFormat"],
                IncorrectValue = dict["IncorrectValue"],
                IncorrectNumberOfDays = dict["IncorrectNumberOfDays"],
                IncorrectFormatOfDayOfMonth = dict["IncorrectFormatOfDayOfMonth"],
                IncorrectNumberOfMonth = dict["IncorrectNumberOfMonth"],
                IncorrectNumberOfDay = dict["IncorrectNumberOfDay"],
                ThereAreFewerDaysInSpecifiedMonth = dict["ThereAreFewerDaysInSpecifiedMonth"],
                IncorrectDayOfTheWeek = dict["IncorrectDayOfTheWeek"],
                IncorrectFormat = dict["IncorrectFormat"],
                IncorrectNumberOfArguments = dict["IncorrectNumberOfArguments"],
                Monday = dict["Monday"],
                Tuesday = dict["Tuesday"],
                Wednes­day = dict["Wednes­day"],
                Thursday = dict["Thursday"],
                Friday = dict["Friday"],
                Saturday = dict["Saturday"],
                Sunday = dict["Sunday"],
                MondayAbbreviated = dict["MondayAbbreviated"],
                TuesdayAbbreviated = dict["TuesdayAbbreviated"],
                Wednes­dayAbbreviated = dict["Wednes­dayAbbreviated"],
                ThursdayAbbreviated = dict["ThursdayAbbreviated"],
                FridayAbbreviated = dict["FridayAbbreviated"],
                SaturdayAbbreviated = dict["SaturdayAbbreviated"],
                SundayAbbreviated = dict["SundayAbbreviated"]
            };

            return language;
        }
    }
}
