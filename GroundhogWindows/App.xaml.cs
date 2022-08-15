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

            if (!GroundhogContext.IsColorSchemaExist(new List<string> { "Main color", "Additional color", "Main text", "Additional text", "Selected item inactive", "Selected item" }))
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

                GroundhogContext.SetColors(colors);
            }
        }
    }
}
