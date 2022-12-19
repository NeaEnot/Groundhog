using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Core.Models.Settings
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

        internal void SaveToFile(string path)
        {
            string content = "";

            content += $"# Settings" + '\n';
            content += $"Settings={Settings}" + '\n';
            content += $"ConnectionString={ConnectionString}" + '\n';
            content += $"PlanningAndOptimizationRange={PlanningAndOptimizationRange}" + '\n';
            content += '\n';

            content += $"# Settings - Color schema" + '\n';
            content += $"ColorSchema={ColorSchema}" + '\n';
            content += $"StandartSchema={StandartSchema}" + '\n';
            content += $"MainColor={MainColor}" + '\n';
            content += $"AditionalColor={AditionalColor}" + '\n';
            content += $"MainText={MainText}" + '\n';
            content += $"AditionalText={AditionalText}" + '\n';
            content += $"SelectedItem={SelectedItem}" + '\n';
            content += $"SelectedItemInactive={SelectedItemInactive}" + '\n';
            content += $"ChosenItem={ChosenItem}" + '\n';
            content += '\n';

            content += $"# Syncronization" + '\n';
            content += $"Syncronization={Syncronization}" + '\n';
            content += $"Download={Download}" + '\n';
            content += $"Upload={Upload}" + '\n';
            content += $"EnterCode={EnterCode}" + '\n';
            content += $"Send={Send}" + '\n';
            content += '\n';

            content += $"# Tasks" + '\n';
            content += $"List={List}" + '\n';
            content += $"Calendar={Calendar}" + '\n';
            content += $"Tasks={Tasks}" + '\n';
            content += $"Task={Task}" + '\n';
            content += $"RepeatMode={RepeatMode}" + '\n';
            content += $"TransferTaskToNextDay={TransferTaskToNextDay}" + '\n';
            content += $"OffsetNextTasks={OffsetNextTasks}" + '\n';
            content += $"PlanningRange={PlanningRange}" + '\n';
            content += $"OptimizationRange={OptimizationRange}" + '\n';
            content += '\n';

            content += $"# Planning modes and optimization" + '\n';
            content += $"NonePlanning={NonePlanning}" + '\n';
            content += $"DaysPlanning={DaysPlanning}" + '\n';
            content += $"DaysOfWeekPlanning={DaysOfWeekPlanning}" + '\n';
            content += $"WatchesPlanning={WatchesPlanning}" + '\n';
            content += $"DaysOfMonthPlanning={DaysOfMonthPlanning}" + '\n';
            content += $"DaysOfYearPlanning={DaysOfYearPlanning}" + '\n';
            content += $"Optimization={Optimization}" + '\n';
            content += '\n';

            content += $"# Purposes" + '\n';
            content += $"Purposes={Purposes}" + '\n';
            content += $"Purpose={Purpose}" + '\n';
            content += $"PurposesGroup={PurposesGroup}" + '\n';
            content += $"GroupName={GroupName}" + '\n';
            content += '\n';

            content += $"# Notes" + '\n';
            content += $"Notes={Notes}" + '\n';
            content += $"Note={Note}" + '\n';
            content += $"NoteName={NoteName}" + '\n';
            content += $"SearchedTextNotFounded={SearchedTextNotFounded}" + '\n';
            content += '\n';

            content += $"# Control commands" + '\n';
            content += $"Save={Save}" + '\n';
            content += $"Duplicate={Duplicate}" + '\n';
            content += $"Update={Update}" + '\n';
            content += $"Delete={Delete}" + '\n';
            content += $"DeleteAllInstances={DeleteAllInstances}" + '\n';
            content += $"Create={Create}" + '\n';
            content += '\n';

            content += $"# Errors messages" + '\n';
            content += $"Error={Error}" + '\n';
            content += $"FieldMustBeFilled={FieldMustBeFilled}" + '\n';
            content += $"FieldsMustBeFilled={FieldsMustBeFilled}" + '\n';
            content += $"CodeWasNotEntered={CodeWasNotEntered}" + '\n';
            content += $"StringNotMatchColorHexFormat={StringNotMatchColorHexFormat}" + '\n';
            content += $"ConnectionStringNotMatchFormat={ConnectionStringNotMatchFormat}" + '\n';
            content += $"CodeWasNotReceived={CodeWasNotReceived}" + '\n';
            content += $"EntityWithSameIdDontExist={EntityWithSameIdDontExist}" + '\n';
            content += $"CorrectValue={CorrectValue}" + '\n';
            content += $"CorrectFormat={CorrectFormat}" + '\n';
            content += $"IncorrectValue={IncorrectValue}" + '\n';
            content += $"IncorrectNumberOfDays={IncorrectNumberOfDays}" + '\n';
            content += $"IncorrectFormatOfDayOfMonth={IncorrectFormatOfDayOfMonth}" + '\n';
            content += $"IncorrectNumberOfMonth={IncorrectNumberOfMonth}" + '\n';
            content += $"IncorrectNumberOfDay={IncorrectNumberOfDay}" + '\n';
            content += $"ThereAreFewerDaysInSpecifiedMonth={ThereAreFewerDaysInSpecifiedMonth}" + '\n';
            content += $"IncorrectDayOfTheWeek={IncorrectDayOfTheWeek}" + '\n';
            content += $"IncorrectFormat={IncorrectFormat}" + '\n';
            content += $"IncorrectNumberOfArguments={IncorrectNumberOfArguments}" + '\n';
            content += '\n';

            content += $"# Days of week" + '\n';
            content += $"Monday={Monday}" + '\n';
            content += $"Tuesday={Tuesday}" + '\n';
            content += $"Wednesday={Wednesday}" + '\n';
            content += $"Thursday={Thursday}" + '\n';
            content += $"Friday={Friday}" + '\n';
            content += $"Saturday={Saturday}" + '\n';
            content += $"Sunday={Sunday}" + '\n';
            content += $"MondayAbbreviated={MondayAbbreviated}" + '\n';
            content += $"TuesdayAbbreviated={TuesdayAbbreviated}" + '\n';
            content += $"WednesdayAbbreviated={WednesdayAbbreviated}" + '\n';
            content += $"ThursdayAbbreviated={ThursdayAbbreviated}" + '\n';
            content += $"FridayAbbreviated={FridayAbbreviated}" + '\n';
            content += $"SaturdayAbbreviated={SaturdayAbbreviated}" + '\n';
            content += $"SundayAbbreviated={SundayAbbreviated}" + '\n';

            File.WriteAllText(path, content);
        }
    }
}
