using Core;
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

            Children[0].Title = "Home";
            Children[1].Title = "Home";
            Children[2].Title = "Home";
        }
    }
}