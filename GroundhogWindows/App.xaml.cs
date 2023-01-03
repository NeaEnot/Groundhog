using Core;
using System.Windows;
using StorageFile.Implements;
using YandexDisk;
using System.Collections.Generic;
using System.Linq;

namespace GroundhogWindows
{
    public partial class App : Application
    {
        public App()
        {
            GroundhogContext.TaskInstanceLogic = new TaskInstanceLogic();
            GroundhogContext.TaskLogic = new TaskLogic();
            GroundhogContext.PurposeLogic = new PurposeLogic();
            GroundhogContext.PurposeGroupLogic = new PurposeGroupLogic();
            GroundhogContext.NoteLogic = new NoteLogic();
            GroundhogContext.NetworkLogic = new NetworkLogic();

            Dictionary<string, string> colors = new Dictionary<string, string>()
            {
                { "Main color", "#FFFFFF" },
                { "Additional color", "#F0F0F0" },
                { "Main text", "#000000" },
                { "Additional text", "#818282" },
                { "Selected item", "#CBE8F6" },
                { "Selected item inactive", "#F6F6F6" },
                { "Select item", "#E5F3FB" }
            };

            bool isNeedSaveSettings = false;

            List<string> absentColors = GroundhogContext.Settings.ColorSchema.ColorSchemaAbsent(colors.Keys.ToList());

            if (absentColors.Count > 0)
            {
                foreach (string key in absentColors)
                    GroundhogContext.Settings.ColorSchema.Colors.Add(key, colors[key]);

                isNeedSaveSettings = true;
            }

            List<string> languages = GroundhogContext.Languages.ToList();
            if (languages.Contains(GroundhogContext.Settings.Language))
            {
                GroundhogContext.Language = GroundhogContext.LoadLanguage(GroundhogContext.Settings.Language);
            }
            else
            {
                GroundhogContext.Language = GroundhogContext.LoadLanguage(GroundhogContext.DefaultLanguage);
                isNeedSaveSettings = true;
            }

            if (isNeedSaveSettings)
                GroundhogContext.SaveSettings();
        }
    }
}
