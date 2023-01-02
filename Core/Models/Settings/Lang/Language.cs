using System.Collections.Generic;

namespace Core.Models.Settings.Lang
{
    public class Language
    {
        public ControlCommandsLanguage ControlCommands { get; set; }
        public DaysOfWeekLanguage DaysOfWeek { get; set; }
        public ErrorsMessagesLanguage ErrorsMessages { get; set; }
        public NotesLanguage Notes { get; set; }
        public PlanningAndOptimizationLanguage PlanningAndOptimization { get; set; }
        public PurposesLanguage Purposes { get; set; }
        public SettingsLanguage Settings { get; set; }
        public SyncronizationLanguage Syncronization { get; set; }
        public TasksLanguage Tasks { get; set; }

        internal static Language Parse(Dictionary<string, string> dict)
        {
            Language language = new Language
            {
                ControlCommands = ControlCommandsLanguage.Parse(dict),
                DaysOfWeek = DaysOfWeekLanguage.Parse(dict),
                ErrorsMessages = ErrorsMessagesLanguage.Parse(dict),
                Notes = NotesLanguage.Parse(dict),
                PlanningAndOptimization = PlanningAndOptimizationLanguage.Parse(dict),
                Purposes = PurposesLanguage.Parse(dict),
                Settings = SettingsLanguage.Parse(dict),
                Syncronization = SyncronizationLanguage.Parse(dict),
                Tasks = TasksLanguage.Parse(dict),
            };

            return language;
        }

        internal string Serialize()
        {
            string content = "";

            content += ControlCommands.Serialize();
            content += DaysOfWeek.Serialize();
            content += ErrorsMessages.Serialize();
            content += Notes.Serialize();
            content += PlanningAndOptimization.Serialize();
            content += Purposes.Serialize();
            content += Settings.Serialize();
            content += Syncronization.Serialize();
            content += Tasks.Serialize();

            return content;
        }
    }
}
