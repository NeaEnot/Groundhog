using Core;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GroundhogMobile.Views.Settings
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConnectionPage : ContentPage
    {
        public ConnectionPage()
        {
            InitializeComponent();

            editorStorage.Placeholder = GroundhogContext.NetworkStorageLogic.ConnectionStringFormat;
            editorStorage.Text = GroundhogContext.Settings.ConnectionStringStorage;

            editorLanguage.Placeholder = GroundhogContext.NetworkLanguageLogic.ConnectionStringFormat;
            editorLanguage.Text = GroundhogContext.Settings.ConnectionStringLanguage;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!GroundhogContext.NetworkStorageLogic.ConnectionStringExpr.IsMatch(editorStorage.Text))
                    throw new Exception($"{GroundhogContext.Language.ErrorsMessages.ConnectionStringNotMatchFormat}.");
                if (!GroundhogContext.NetworkLanguageLogic.ConnectionStringExpr.IsMatch(editorLanguage.Text))
                    throw new Exception($"{GroundhogContext.Language.ErrorsMessages.ConnectionStringNotMatchFormat}.");

                GroundhogContext.Settings.ConnectionStringStorage = editorStorage.Text;
                GroundhogContext.Settings.ConnectionStringLanguage = editorLanguage.Text;
                GroundhogContext.SaveSettings();

                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert(GroundhogContext.Language.ErrorsMessages.Error, ex.Message, "Ok");
            }
        }
    }
}