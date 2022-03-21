using Core.Models;
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
    public partial class PurposeGroupPage : ContentPage
    {
        public bool IsSuccess { get; private set; } = false;
        internal PurposeGroup Group { get; private set; }

        public PurposeGroupPage(PurposeGroup group)
        {
            InitializeComponent();

            if (group == null)
                throw new ArgumentNullException("При создании новой задачи необходимо передавать новый объект с пустыми полями.");

            InitializeComponent();

            Group = group;
            BindingContext = group;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textEntry.Text))
                    throw new Exception("Поле должно быть заполнено.");

                Group.Name = textEntry.Text;
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