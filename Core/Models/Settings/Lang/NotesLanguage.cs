using System.Collections.Generic;

namespace Core.Models.Settings.Lang
{
    public class NotesLanguage
    {
        public string Notes { get; set; }
        public string Note { get; set; }
        public string NoteName { get; set; }

        internal static NotesLanguage Parse(Dictionary<string, string> dict)
        {
            NotesLanguage language = new NotesLanguage
            {
                Notes = dict["Notes"],
                Note = dict["Note"],
                NoteName = dict["NoteName"]
            };

            return language;
        }

        internal string Serialize()
        {
            string content = "";

            content += $"# Notes" + '\n';
            content += $"Notes={Notes}" + '\n';
            content += $"Note={Note}" + '\n';
            content += $"NoteName={NoteName}" + '\n';
            content += '\n';

            return content;
        }
    }
}
