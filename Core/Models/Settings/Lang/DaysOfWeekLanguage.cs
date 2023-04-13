using System.Collections.Generic;

namespace Core.Models.Settings.Lang
{
    public class DaysOfWeekLanguage
    {
        public string MondayAbbreviated { get; set; }
        public string TuesdayAbbreviated { get; set; }
        public string Wednes­dayAbbreviated { get; set; }
        public string ThursdayAbbreviated { get; set; }
        public string FridayAbbreviated { get; set; }
        public string SaturdayAbbreviated { get; set; }
        public string SundayAbbreviated { get; set; }

        internal static DaysOfWeekLanguage Parse(Dictionary<string, string> dict)
        {
            DaysOfWeekLanguage language = new DaysOfWeekLanguage
            {
                MondayAbbreviated = dict["MondayAbbreviated"],
                TuesdayAbbreviated = dict["TuesdayAbbreviated"],
                Wednes­dayAbbreviated = dict["WednesdayAbbreviated"],
                ThursdayAbbreviated = dict["ThursdayAbbreviated"],
                FridayAbbreviated = dict["FridayAbbreviated"],
                SaturdayAbbreviated = dict["SaturdayAbbreviated"],
                SundayAbbreviated = dict["SundayAbbreviated"]
            };

            return language;
        }

        internal string Serialize()
        {
            string content = "";

            content += $"# Days of week" + '\n';
            content += $"MondayAbbreviated={MondayAbbreviated}" + '\n';
            content += $"TuesdayAbbreviated={TuesdayAbbreviated}" + '\n';
            content += $"WednesdayAbbreviated={WednesdayAbbreviated}" + '\n';
            content += $"ThursdayAbbreviated={ThursdayAbbreviated}" + '\n';
            content += $"FridayAbbreviated={FridayAbbreviated}" + '\n';
            content += $"SaturdayAbbreviated={SaturdayAbbreviated}" + '\n';
            content += $"SundayAbbreviated={SundayAbbreviated}" + '\n';

            return content;
        }
    }
}
