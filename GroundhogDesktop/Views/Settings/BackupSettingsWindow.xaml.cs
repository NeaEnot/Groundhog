﻿using Core;
using System.Windows;

namespace GroundhogDesktop.Views.Settings
{
    public partial class BackupSettingsWindow : Window
    {
        public BackupSettingsWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GroundhogContext.Settings.BackupSettings.AutoCloudBackup = chbAutoCloudBackup.IsChecked.Value;
            GroundhogContext.Settings.BackupSettings.AutoLocalBackup = chbAutoLocalBackup.IsChecked.Value;

            GroundhogContext.SaveSettings();
            DialogResult = true;
        }
    }
}
