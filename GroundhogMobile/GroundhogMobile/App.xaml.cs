using Core;
using StorageFile.Implements;
using System.Collections.Generic;
using Xamarin.Forms;
using YandexDisk;

namespace GroundhogMobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            GroundhogContext.TaskInstanceLogic = new TaskInstanceLogic();
            GroundhogContext.TaskLogic = new TaskLogic();
            GroundhogContext.PurposeLogic = new PurposeLogic();
            GroundhogContext.PurposeGroupLogic = new PurposeGroupLogic();
            GroundhogContext.NetworkLogic = new NetworkLogic();

            MainPage = new NavigationPage(new MainPage());

            if (!GroundhogContext.Settings.ColorSchema.IsColorSchemaExist(new List<string> { "Main color", "Additional color", "Main text", "Additional text", "Selected item" }))
            {
                Dictionary<string, string> colors = new Dictionary<string, string>()
                {
                    { "Main color", "#FFFFFF" },
                    { "Additional color", "#F0F0F0" },
                    { "Main text", "#000000" },
                    { "Additional text", "#818282" },
                    { "Selected item", "#CBE8F6" }
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

            LoadResources();
        }

        public static void LoadResources()
        {
            App.Current.Resources["Main color"] = Color.FromHex(GroundhogContext.Settings.ColorSchema.Colors["Main color"]);
            App.Current.Resources["Additional color"] = Color.FromHex(GroundhogContext.Settings.ColorSchema.Colors["Additional color"]);
            App.Current.Resources["Main text"] = Color.FromHex(GroundhogContext.Settings.ColorSchema.Colors["Main text"]);
            App.Current.Resources["Additional text"] = Color.FromHex(GroundhogContext.Settings.ColorSchema.Colors["Additional text"]);
            App.Current.Resources["Selected item"] = Color.FromHex(GroundhogContext.Settings.ColorSchema.Colors["Selected item"]);
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
