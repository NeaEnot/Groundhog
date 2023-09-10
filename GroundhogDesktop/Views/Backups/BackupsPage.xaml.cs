using Core.Interfaces.Network;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WindowsDesktop.Views.Backups
{
    public partial class BackupsPage : Page
    {
        private IBackupLogic backupLogic;
        private BackupsWindow backupsWindow;

        public BackupsPage(IBackupLogic backupLogic, BackupsWindow backupsWindow)
        {
            InitializeComponent();

            this.backupLogic = backupLogic;
            this.backupsWindow = backupsWindow;

            LoadBackups();
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
            backupsWindow.LoadAfterRestore();
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
