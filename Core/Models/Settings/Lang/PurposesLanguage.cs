using System.Collections.Generic;

namespace Core.Models.Settings.Lang
{
    /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="PurposesLanguage"]/PurposesLanguage/*'/>
    public class PurposesLanguage
    {
        public string Purposes { get; set; }
        public string Purpose { get; set; }
        public string PurposesGroup { get; set; }
        public string GroupName { get; set; }

        internal static PurposesLanguage Parse(Dictionary<string, string> dict)
        {
            PurposesLanguage language = new PurposesLanguage
            {
                Purpose = dict["Purpose"],
                Purposes = dict["Purposes"],
                PurposesGroup = dict["PurposesGroup"],
                GroupName = dict["GroupName"],
            };

            return language;
        }

        internal string Serialize()
        {
            string content = "";

            content += $"# Purposes" + '\n';
            content += $"Purposes={Purposes}" + '\n';
            content += $"Purpose={Purpose}" + '\n';
            content += $"PurposesGroup={PurposesGroup}" + '\n';
            content += $"GroupName={GroupName}" + '\n';
            content += '\n';

            return content;
        }
    }
}
