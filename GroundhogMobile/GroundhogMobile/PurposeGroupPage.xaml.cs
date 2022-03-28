using Core.Models;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GroundhogMobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PurposeGroupPage : ContentPage
    {
        public bool IsSuccess { get; private set; } = false;
        internal PurposeGroup Group { get; private set; }

        internal PurposeGroupPage(PurposeGroup group)
        {
            if (group == null)
                throw new ArgumentNullException("При создании новой группы целей необходимо передавать новый объект с пустыми полями.");

            InitializeComponent();

            Group = group;
            BindingContext = group;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textEntryGroup.Text))
                    throw new Exception("Поле должно быть заполнено.");

                Group.Name = textEntryGroup.Text;
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