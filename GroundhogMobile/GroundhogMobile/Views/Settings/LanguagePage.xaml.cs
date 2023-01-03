using Core;
using GroundhogMobile.Views.Services;
using Rg.Plugins.Popup.Services;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GroundhogMobile.Views.Settings
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LanguagePage : ContentPage
    {
        private string language;

        public LanguagePage()
        {
            InitializeComponent();
            language = GroundhogContext.Settings.Language;
            btnLanguage.Text = language;
        }

        private async void btnLanguage_Clicked(object sender, EventArgs e)
        {
            CommandPage page = new CommandPage("Язык", GroundhogContext.Languages);
            Device.BeginInvokeOnMainThread(async () => await PopupNavigation.Instance.PushAsync(page));

            object obj = await page.Result;
            if (obj != null)
            {
                string result = obj as string;

                language = result;
                btnLanguage.Text = language;
            }
        }

        private async void btnSave_Clicked(object sender, EventArgs e)
        {
            GroundhogContext.Settings.Language = language;
            GroundhogContext.Language = GroundhogContext.LoadLanguage(GroundhogContext.Settings.Language);

            await Navigation.PopAsync();
        }
    }
}