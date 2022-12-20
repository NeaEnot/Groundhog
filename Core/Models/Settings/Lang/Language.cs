using System.Collections.Generic;
using System.IO;
using System.Linq;

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

        internal static Language ReadFromFile(string path)
        {
            string[] lines = File.ReadAllLines(path);

            IEnumerable<KeyValuePair<string, string>> pairs =
                lines
                .Where(req => !req.StartsWith("#") && !string.IsNullOrWhiteSpace(req))
                .Select(req => new KeyValuePair<string, string>(req.Split('=')[0], req.Split('=')[1]));

            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> pair in pairs)
                dict.Add(pair.Key, pair.Value);

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

        internal void SaveToFile(string path)
        {
            string content = "";

            content += ControlCommands.Serrialize();
            content += DaysOfWeek.Serrialize();
            content += ErrorsMessages.Serrialize();
            content += Notes.Serrialize();
            content += PlanningAndOptimization.Serrialize();
            content += Purposes.Serrialize();
            content += Settings.Serrialize();
            content += Syncronization.Serrialize();
            content += Tasks.Serrialize();

            File.WriteAllText(path, content);
        }
    }
}
