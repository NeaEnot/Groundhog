﻿using Core;
using GroundhogMobile.Views.Settings;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GroundhogMobile.Views.Backups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BackupsMainPage : TabbedPage
    {
        private BackupsPage localBackupsPage;
        private BackupsPage cloudBackupsPage;

        public BackupsMainPage()
        {
            InitializeComponent();

            localBackupsPage = new BackupsPage(GroundhogContext.LocalBackupLogic);
            cloudBackupsPage = new BackupsPage(GroundhogContext.CloudBackupLogic);

            Children.Add(localBackupsPage);
            Children.Add(cloudBackupsPage);
            Children.Add(new BackupSettingsPage());

            Children[0].Title = GroundhogContext.Language.Backup.Local;
            Children[1].Title = GroundhogContext.Language.Backup.Cloud;
            Children[2].Title = GroundhogContext.Language.Settings.Settings;
        }
    }
}