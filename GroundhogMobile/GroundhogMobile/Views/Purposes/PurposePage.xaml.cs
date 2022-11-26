using GroundhogMobile.Models;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GroundhogMobile.Views.Purposes
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PurposePage : ContentPage
    {
        public bool IsSuccess { get; private set; } = false;
        internal PurposeViewModel Model { get; private set; }

        internal PurposePage(PurposeViewModel model)
        {
            if (model == null)
                throw new ArgumentNullException("При создании новой цели необходимо передавать новый объект с пустыми полями.");

            InitializeComponent();

            Model = model;
            BindingContext = model;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textEntryPurpose.Text))
                    throw new Exception("Поле должно быть заполнено.");

                Model.Text = textEntryPurpose.Text;
                IsSuccess = true;

                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка", ex.Message, "Ок");
            }
        }
    }
}