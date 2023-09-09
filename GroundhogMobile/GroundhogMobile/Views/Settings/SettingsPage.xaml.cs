using Core;
using GroundhogMobile.Views.Backups;
using System;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GroundhogMobile.Views.Settings
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
                if (GroundhogContext.Settings.BackupSettings.AutoLocalBackup)
                    GroundhogContext.LocalBackupLogic.MakeBackup(DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));

                ConnectIfNot();

                GroundhogContext.NetworkStorageLogic.Load();
                GroundhogContext.NetworkLanguageLogic.Load();

                this.DisplayToastAsync(GroundhogContext.Language.Syncronization.DataHasDownladed);
                DownloadFinisfed();

                GroundhogContext.Language = GroundhogContext.LoadLanguage(GroundhogContext.Settings.Language);

                App.ApplyColorSchema();
                App.ApplyLanguage();
            }
            catch (Exception ex)
            {
                await DisplayAlert(GroundhogContext.Language.ErrorsMessages.Error, ex.Message, "Ok");
            }
        }

        private async void ButtonSend_Clicked(object sender, EventArgs e)
        {
            try
            {
                await System.Threading.Tasks.Task.Run(() =>
                {
                    ConnectIfNot();

                    if (GroundhogContext.Settings.BackupSettings.AutoCloudBackup)
                        GroundhogContext.CloudBackupLogic.MakeBackup(DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));

                    GroundhogContext.NetworkStorageLogic.Upload();
                    GroundhogContext.NetworkLanguageLogic.Upload();

                    this.DisplayToastAsync(GroundhogContext.Language.Syncronization.DataHasUpladed);
                });
            }
            catch (Exception ex)
            {
                await DisplayAlert(GroundhogContext.Language.ErrorsMessages.Error, ex.Message, "Ok");
            }
        }

        private void ConnectIfNot()
        {
            if (!GroundhogContext.NetworkStorageLogic.IsConnected())
                GroundhogContext.NetworkStorageLogic.Connect(() => "");

            if (!GroundhogContext.NetworkLanguageLogic.IsConnected())
                GroundhogContext.NetworkLanguageLogic.Connect(() => "");
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

        private async void ButtonLanguage_Clicked(object sender, EventArgs e)
        {
            LanguagePage page = new LanguagePage();
            await Navigation.PushAsync(page);
        }

        private async void ButtonBackup_Clicked(object sender, EventArgs e)
        {
            BackupsMainPage page = new BackupsMainPage();
            await Navigation.PushAsync(page);
        }
    }
}