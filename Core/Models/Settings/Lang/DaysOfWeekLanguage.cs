using System.Collections.Generic;

namespace Core.Models.Settings.Lang
{
    public class DaysOfWeekLanguage
    {
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

        internal static DaysOfWeekLanguage Parse(Dictionary<string, string> dict)
        {
            DaysOfWeekLanguage language = new DaysOfWeekLanguage
            {
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

        internal string Serrialize()
        {
            string content = "";

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

            return content;
        }
    }
}
