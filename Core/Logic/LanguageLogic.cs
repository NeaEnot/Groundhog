using Core.Models.Settings.Lang;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Core.Logic
{
    internal static class LanguageLogic
    {
        internal static readonly string DefaultLanguage = "English";

        internal static void CreateDefault()
        {
            Language language = new Language
            {
                Culture = "en-US",
                ControlCommands = new ControlCommandsLanguage
                {
                    Save = "Save",
                    Duplicate = "Duplicate",
                    Edit = "Edit",
                    Delete = "Delete",
                    DeleteAllInstances = "Delete all instances",
                    Create = "Create",
                    Cancel = "Cancel"
                },
                DaysOfWeek = new DaysOfWeekLanguage
                {
                    MondayAbbreviated = "Mon.",
                    TuesdayAbbreviated = "Tue.",
                    Wednes­dayAbbreviated = "Wed.",
                    ThursdayAbbreviated = "Thur.",
                    FridayAbbreviated = "Fri.",
                    SaturdayAbbreviated = "Sat.",
                    SundayAbbreviated = "Sun."
                },
                ErrorsMessages = new ErrorsMessagesLanguage
                {
                    Error = "Error",
                    FieldMustBeFilled = "Field must be filled",
                    FieldsMustBeFilled = "Fields must be filled",
                    StringNotMatchColorHexFormat = "String not match color hex format",
                    CodeWasNotEntered = "Code was not entered",
                    ConnectionStringNotMatchFormat = "Connection string not match format",
                    CodeWasNotReceived = "Code was not received",
                    FailedToConnect = "Failed to connect",
                    ConnectionFailed = "Connection failed",
                    FailedToDownloadData = "Failed to download data",
                    FailedToUploadData = "Failed to upload data",
                    EntityWithSameIdDontExist = "Entity with same id dont exist",
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
                    Or = "Or"
                },
                Notes = new NotesLanguage
                {
                    Notes = "Notes",
                    Note = "Note",
                    NoteName = "Note name",
                    Find = "Find",
                    SearchedTextNotFounded = "Searched text not founded"
                },
                PlanningAndOptimization = new PlanningAndOptimizationLanguage
                {
                    NonePlanning = "None planning",
                    DaysPlanning = "Days planning",
                    DaysOfWeekPlanning = "Days of week planning",
                    WatchesPlanning = "Watches planning",
                    DaysOfMonthPlanning = "Days of month planning",
                    DaysOfYearPlanning = "Days of year planning",
                    Optimization = "Optimization",
                    DaysToolTip = "Number",
                    DayOfMonthToolTip = "Number",
                    DayOfYearToolTip = "mm.dd",
                    DaysOfWeekToolTip = "Mon,Wed,..."
                },
                Purposes = new PurposesLanguage
                {
                    Purposes = "Purposes",
                    Purpose = "Purpose",
                    PurposesGroup = "Purposes group",
                    GroupName = "Group name"
                },
                Settings = new SettingsLanguage
                {
                    Settings = "Settings",
                    ConnectionString = "Connection string",
                    SelectLanguage = "Select language",
                    PlanningAndOptimizationRange = "Planning and optimization range",
                    ColorSchema = "Color schema",
                    MainColor = "Main color",
                    AditionalColor = "Aditional color",
                    MainText = "Main text",
                    AditionalText = "Aditional text",
                    SelectedItem = "Selected item",
                    SelectedItemInactive = "Selected item inactive",
                    ChosenItem = "Chosen item",
                    StandartSchema = "Standart schema"
                },
                Syncronization = new SyncronizationLanguage
                {
                    Syncronization = "Syncronization",
                    Download = "Download",
                    Upload = "Upload",
                    EnterCode = "Enter code",
                    Send = "Send",
                    DataHasDownladed = "Data has downladed",
                    DataHasUpladed = "Data has upladed"
                },
                Tasks = new TasksLanguage
                {
                    Tasks = "Tasks",
                    Task = "Task",
                    RepeatMode = "Repeat mode",
                    TransferTaskToNextDay = "Transfer task to next day",
                    OffsetNextTasks = "Offset next tasks",
                    PlanningRange = "Planning range",
                    OptimizationRange = "Optimization range",
                    List = "List",
                    Calendar = "Calendar",
                }
            };

            Save(language, $"{GroundhogContext.StoragePath}\\Languages\\{DefaultLanguage}.lng");
        }

        internal static void Save(Language language, string path)
        {
            string content = language.Serialize();
            File.WriteAllText(path, content);
        }

        internal static Language Load(string path)
        {
            string[] lines = File.ReadAllLines(path);

            IEnumerable<KeyValuePair<string, string>> pairs =
                lines
                .Where(req => !req.StartsWith("#") && !string.IsNullOrWhiteSpace(req))
                .Select(req => new KeyValuePair<string, string>(req.Split('=')[0], req.Split('=')[1]));

            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> pair in pairs)
                dict.Add(pair.Key, pair.Value);

            return Language.Parse(dict);
        }
    }
}
