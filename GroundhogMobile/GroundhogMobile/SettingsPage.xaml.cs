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
                await System.Threading.Tasks.Task.Run(() =>
                {
                    ConnectIfNot();
                    GroundhogContext.NetworkLogic.Load();
                    this.DisplayToastAsync("Данные загружены");
                });
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
    }
}