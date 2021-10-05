using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Threading.Tasks;
using Xamarin.Forms.Xaml;

namespace GroundhogMobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CodePage : PopupPage
    {
        internal Task<string> Code { get => tcs.Task; }

        private TaskCompletionSource<string> tcs;

        public CodePage()
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