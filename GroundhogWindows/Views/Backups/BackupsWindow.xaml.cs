using Core;
using System.Windows;

namespace GroundhogWindows.Views.Backups
{
    public partial class BackupsWindow : Window
    {
        private BackupsPage localBackupsPage;
        private BackupsPage cloudBackupsPage;

        public BackupsWindow()
        {
            InitializeComponent();

            localBackupsPage = new BackupsPage(GroundhogContext.LocalBackupLogic);
            cloudBackupsPage = new BackupsPage(GroundhogContext.CloudBackupLogic);

            fLocalBackups.Content = localBackupsPage;
            fCloudBackups.Content = cloudBackupsPage;
        }
    }
}
