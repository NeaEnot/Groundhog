using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Threading.Tasks;

using Xamarin.Forms.Xaml;

namespace GroundhogMobile.Views.Backups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateBackupPage : PopupPage
    {
        internal Task<string> Key { get => tcs.Task; }

        private TaskCompletionSource<string> tcs;

        public CreateBackupPage()
        {
            InitializeComponent();
            tcs = new TaskCompletionSource<string>();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textEntry.Text))
            {
                tcs.SetResult(textEntry.Text);
                await PopupNavigation.Instance.PopAsync();
            }
        }
    }
}