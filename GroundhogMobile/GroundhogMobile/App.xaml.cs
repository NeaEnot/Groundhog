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

            if (!GroundhogContext.IsColorSchemaExist)
            {
                Dictionary<string, string> colors = new Dictionary<string, string>()
                {
                    { "Main color", "#FFFFFF" },
                    { "Additional color", "#F0F0F0" },
                    { "Main text", "#000000" },
                    { "Additional text", "#818282" },
                    { "Selected item", "#CBE8F6" },
                    { "Select item", "#E5F3FB" }
                };

                GroundhogContext.SetColors(colors);
            }
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
