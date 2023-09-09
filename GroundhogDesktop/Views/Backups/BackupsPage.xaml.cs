using Core.Interfaces.Network;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WindowsDesktop.Views.Backups
{
    public partial class BackupsPage : Page
    {
        IBackupLogic backupLogic;

        public BackupsPage(IBackupLogic backupLogic)
        {
            InitializeComponent();

            this.backupLogic = backupLogic;
        }

        public void LoadBackups()
        {
            List<string> backups = backupLogic.Backups.OrderBy(req => req).ToList();
            listBoxBackups.ItemsSource = backups;
        }

        private void ContextMenuRestore_Click(object sender, RoutedEventArgs e)
        {
            string key = (string)listBoxBackups.SelectedItem;
            backupLogic.RestoreBackup(key);
        }

        private void ContextMenuDelete_Click(object sender, RoutedEventArgs e)
        {
            string key = (string)listBoxBackups.SelectedItem;
            backupLogic.DeleteBackup(key);
            LoadBackups();
        }

        private void ButtonCreate_Click(object sender, RoutedEventArgs e)
        {
            CreateBackupWindow createBackupWindow = new CreateBackupWindow();
            if (createBackupWindow.ShowDialog() == true)
            {
                backupLogic.MakeBackup(createBackupWindow.Key);
                LoadBackups();
            }
        }
    }
}
