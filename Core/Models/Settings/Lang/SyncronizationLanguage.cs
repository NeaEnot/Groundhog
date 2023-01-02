using System.Collections.Generic;

namespace Core.Models.Settings.Lang
{
    public class SyncronizationLanguage
    {
        public string Syncronization { get; set; }
        public string Download { get; set; }
        public string Upload { get; set; }
        public string EnterCode { get; set; }
        public string Send { get; set; }

        internal static SyncronizationLanguage Parse(Dictionary<string, string> dict)
        {
            SyncronizationLanguage language = new SyncronizationLanguage
            {
                Syncronization = dict["Syncronization"],
                Download = dict["Download"],
                Upload = dict["Upload"],
                EnterCode = dict["EnterCode"],
                Send = dict["Send"]
            };

            return language;
        }

        internal string Serialize()
        {
            string content = "";

            content += $"# Syncronization" + '\n';
            content += $"Syncronization={Syncronization}" + '\n';
            content += $"Download={Download}" + '\n';
            content += $"Upload={Upload}" + '\n';
            content += $"EnterCode={EnterCode}" + '\n';
            content += $"Send={Send}" + '\n';
            content += '\n';

            return content;
        }
    }
}
