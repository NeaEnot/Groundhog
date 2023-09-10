using Core;
using System.Windows;

namespace GroundhogDesktop.Views.Backups
{
    public partial class BackupsWindow : Window
    {
        private BackupsPage localBackupsPage;
        private BackupsPage cloudBackupsPage;

        private MainWindow mainWindow;

        public BackupsWindow(MainWindow mainWindow)
        {
            InitializeComponent();

            this.mainWindow = mainWindow;

            localBackupsPage = new BackupsPage(GroundhogContext.LocalBackupLogic, this);
            cloudBackupsPage = new BackupsPage(GroundhogContext.CloudBackupLogic, this);

            fLocalBackups.Content = localBackupsPage;
            fCloudBackups.Content = cloudBackupsPage;
        }

        internal void LoadAfterRestore()
        {
            mainWindow.LoadPurposeGroups();
            mainWindow.LoadNotes();
        }
    }
}
