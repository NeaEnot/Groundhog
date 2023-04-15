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

            editor.Placeholder = GroundhogContext.NetworkStorageLogic.ConnectionStringFormat;
            editor.Text = GroundhogContext.Settings.ConnectionStringStorage;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!GroundhogContext.NetworkStorageLogic.ConnectionStringExpr.IsMatch(editor.Text))
                    throw new Exception($"{GroundhogContext.Language.ErrorsMessages.ConnectionStringNotMatchFormat}.");

                GroundhogContext.Settings.ConnectionStringStorage = editor.Text;
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