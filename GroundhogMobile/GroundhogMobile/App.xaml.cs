using Core;
using GroundhogMobile.Views;
using StorageFile.Implements;
using System.Collections.Generic;
using System.Linq;
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
            GroundhogContext.NoteLogic = new NoteLogic();
            GroundhogContext.NetworkLogic = new NetworkLogic();

            MainPage = new NavigationPage(new MainPage());

            Dictionary<string, string> colors = new Dictionary<string, string>()
            {
                { "Main color", "#FFFFFF" },
                { "Additional color", "#F0F0F0" },
                { "Main text", "#000000" },
                { "Additional text", "#818282" },
                { "Selected item", "#CBE8F6" }
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

            ApplyColorSchema();
        }

        public static void ApplyColorSchema()
        {
            App.Current.Resources["Main color"] = Color.FromHex(GroundhogContext.Settings.ColorSchema.Colors["Main color"]);
            App.Current.Resources["Additional color"] = Color.FromHex(GroundhogContext.Settings.ColorSchema.Colors["Additional color"]);
            App.Current.Resources["Main text"] = Color.FromHex(GroundhogContext.Settings.ColorSchema.Colors["Main text"]);
            App.Current.Resources["Additional text"] = Color.FromHex(GroundhogContext.Settings.ColorSchema.Colors["Additional text"]);
            App.Current.Resources["Selected item"] = Color.FromHex(GroundhogContext.Settings.ColorSchema.Colors["Selected item"]);
        }

        public static void ApplyLanguage()
        {

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
