using Core;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GroundhogMobile.Views.Settings
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BackupSettingsPage : ContentPage
    {
        public BackupSettingsPage()
        {
            InitializeComponent();

            chbAutoCloudBackup.IsChecked = GroundhogContext.Settings.BackupSettings.AutoCloudBackup;
            chbAutoLocalBackup.IsChecked = GroundhogContext.Settings.BackupSettings.AutoLocalBackup;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            GroundhogContext.Settings.BackupSettings.AutoCloudBackup = chbAutoCloudBackup.IsChecked;
            GroundhogContext.Settings.BackupSettings.AutoLocalBackup = chbAutoLocalBackup.IsChecked;

            GroundhogContext.SaveSettings();
        }
    }
}