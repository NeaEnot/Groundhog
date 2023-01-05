using System.Collections.Generic;

namespace Core.Models.Settings.Lang
{
    public class NotesLanguage
    {
        public string Notes { get; set; }
        public string Note { get; set; }
        public string NoteName { get; set; }
        public string Find { get; set; }
        public string SearchedTextNotFounded { get; set; }

        internal static NotesLanguage Parse(Dictionary<string, string> dict)
        {
            NotesLanguage language = new NotesLanguage
            {
                Notes = dict["Notes"],
                Note = dict["Note"],
                NoteName = dict["NoteName"],
                Find = dict["Find"],
                SearchedTextNotFounded = dict["SearchedTextNotFounded"]
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
            content += $"Find={Find}" + '\n';
            content += $"SearchedTextNotFounded={SearchedTextNotFounded}" + '\n';
            content += '\n';

            return content;
        }
    }
}
