using Core;
using System.Windows;
using StorageFile.Implements;
using YandexDisk;
using System.Collections.Generic;

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

            if (!GroundhogContext.Settings.ColorSchema.IsColorSchemaExist(new List<string> { "Main color", "Additional color", "Main text", "Additional text", "Selected item inactive", "Selected item" }))
            {
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

                foreach (string key in colors.Keys)
                {
                    if (GroundhogContext.Settings.ColorSchema.Colors.ContainsKey(key))
                        GroundhogContext.Settings.ColorSchema.Colors[key] = colors[key];
                    else
                        GroundhogContext.Settings.ColorSchema.Colors.Add(key, colors[key]);
                }

                GroundhogContext.SaveSettings();
            }
        }
    }
}
