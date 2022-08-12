using Core.Models;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GroundhogMobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotePage : ContentPage
    {
        public bool IsSuccess { get; private set; } = false;
        internal Note Note { get; private set; }

        public NotePage(Note note)
        {
            if (note == null)
                throw new ArgumentNullException("При создании новой заметки необходимо передавать новый объект с пустыми полями.");

            InitializeComponent();

            Note = note;
            BindingContext = note;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nameEntry.Text))
                {
                    throw new Exception("Имя должно быть заполнено.");
                }

                Note.Name = nameEntry.Text;
                Note.Text = textEditor.Text;
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