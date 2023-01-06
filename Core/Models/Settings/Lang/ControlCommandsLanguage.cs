using System.Collections.Generic;

namespace Core.Models.Settings.Lang
{
    public class ControlCommandsLanguage
    {
        public string Save { get; set; }
        public string Duplicate { get; set; }
        public string Edit { get; set; }
        public string Delete { get; set; }
        public string DeleteAllInstances { get; set; }
        public string Create { get; set; }

        internal static ControlCommandsLanguage Parse(Dictionary<string, string> dict)
        {
            ControlCommandsLanguage language = new ControlCommandsLanguage
            {
                Save = dict["Save"],
                Duplicate = dict["Duplicate"],
                Edit = dict["Edit"],
                Delete = dict["Delete"],
                DeleteAllInstances = dict["DeleteAllInstances"]
            };

            return language;
        }

        internal string Serialize()
        {
            string content = "";

            content += $"# Control commands" + '\n';
            content += $"Save={Save}" + '\n';
            content += $"Duplicate={Duplicate}" + '\n';
            content += $"Edit={Edit}" + '\n';
            content += $"Delete={Delete}" + '\n';
            content += $"DeleteAllInstances={DeleteAllInstances}" + '\n';
            content += $"Create={Create}" + '\n';
            content += '\n';

            return content;
        }
    }
}
