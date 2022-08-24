using Core;
using System;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GroundhogMobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public delegate void Processinisfed();
        public static event Processinisfed DownloadFinisfed;

        public SettingsPage()
        {
            InitializeComponent();
        }

        private async void ButtonConnectString_Clicked(object sender, EventArgs e)
        {
            ConnectionPage page = new ConnectionPage();
            await Navigation.PushAsync(page);
        }

        private async void ButtonLoad_Clicked(object sender, EventArgs e)
        {
            try
            {
                ConnectIfNot();
                GroundhogContext.NetworkLogic.Load();
                this.DisplayToastAsync("Данные загружены");
                DownloadFinisfed();

                App.LoadResources();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка", ex.Message, "Ок");
            }
        }

        private async void ButtonSend_Clicked(object sender, EventArgs e)
        {
            try
            {
                await System.Threading.Tasks.Task.Run(() =>
                {
                    ConnectIfNot();
                    GroundhogContext.NetworkLogic.Upload();
                    this.DisplayToastAsync("Данные отправлены");
                });
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка", ex.Message, "Ок");
            }
        }

        private void ConnectIfNot()
        {
            if (!GroundhogContext.NetworkLogic.IsConnected())
                GroundhogContext.NetworkLogic.Connect(() => "");
        }

        private async void ButtonPlanning_Clicked(object sender, EventArgs e)
        {
            PlanningPage page = new PlanningPage();
            await Navigation.PushAsync(page);
        }

        private async void ButtonColors_Clicked(object sender, EventArgs e)
        {
            ColorsPage page = new ColorsPage();
            await Navigation.PushAsync(page);
        }
    }
}