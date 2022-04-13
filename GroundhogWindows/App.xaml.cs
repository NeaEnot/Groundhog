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
            GroundhogContext.NetworkLogic = new NetworkLogic();

            if (!GroundhogContext.IsColorSchemaExist)
            {
                Dictionary<string, string> colors = new Dictionary<string, string>()
                {
                    { "Main color", "#ffffff" },
                    { "Additional color", "#f0f0f0" },
                    { "Main text", "#000000" },
                    { "Additional text", "#818282" },
                    { "Selected item", "#cbe8f6" },
                    { "Selected item inactive", "#f6f6f6" },
                    { "Select item", "#e5f3fb" },
                };

                GroundhogContext.SetColors(colors);
            }
        }
    }
}
