using Core;
using TelegramImplement.Implements;
using Xamarin.Forms;

namespace GroundhogMobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            GroundhogContext.AccauntLogic = new AccauntLogic();
            GroundhogContext.TaskInstanceLogic = new TaskInstanceLogic();
            GroundhogContext.TaskLogic = new TaskLogic();
            GroundhogContext.NetworkLogic = new NetworkLogic();

            MainPage = new NavigationPage(new MainPage());
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
