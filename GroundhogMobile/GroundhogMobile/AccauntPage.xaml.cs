using Core.Models;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GroundhogMobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AccauntPage : ContentPage
    {
        public bool IsSuccess { get; private set; } = false;
        private Accaunt accaunt;

        public AccauntPage(Accaunt accaunt)
        {
            if (accaunt == null)
                throw new ArgumentNullException("При создании нового аккаунта необходимо передавать новый объект с пустыми полями.");

            InitializeComponent();

            this.accaunt = accaunt;
            BindingContext = accaunt;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(nameEntry.Text) || string.IsNullOrWhiteSpace(connectionEntry.Text))
            {
                await DisplayAlert("Ошибка", "Одно из полей не заполнено.", "Ок");
            }
            else
            {
                accaunt.Name = nameEntry.Text;
                accaunt.ConnectionString = connectionEntry.Text;
                IsSuccess = true;

                await Navigation.PopAsync();
            }
        }
    }
}