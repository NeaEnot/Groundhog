using Core.Models;
using GroundhogMobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GroundhogMobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PurposePage : ContentPage
    {
        public bool IsSuccess { get; private set; } = false;
        internal PurposeViewModel Model { get; private set; }

        internal PurposePage(PurposeViewModel model)
        {
            InitializeComponent();

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
                if (string.IsNullOrWhiteSpace(textEntry.Text))
                    throw new Exception("Поле должно быть заполнено.");

                Model.Text = textEntry.Text;
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