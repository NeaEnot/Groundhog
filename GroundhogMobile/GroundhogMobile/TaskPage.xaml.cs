using Core.Enums;
using GroundhogMobile.Models;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GroundhogMobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TaskPage : ContentPage
    {
        public bool IsSuccess { get; private set; } = false;
        private TaskViewModel model;

        private bool isLoaded;

        internal TaskPage(TaskViewModel model)
        {
            isLoaded = true;

            if (model == null)
                throw new ArgumentNullException("При создании новой задачи необходимо передавать новый объект с пустыми полями.");

            InitializeComponent();

            this.model = model;
            BindingContext = model;

            repeatModePicker.ItemsSource = Enum.GetValues(typeof(RepeatMode));
            repeatModePicker.SelectedItem = model.RepeatMode;

            isLoaded = false;
        }

        private void repeatModePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isLoaded)
            {
                repeatValueEntry.IsVisible = (RepeatMode)repeatModePicker.SelectedItem != RepeatMode.Нет;
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textEntry.Text) ||
                    (RepeatMode)repeatModePicker.SelectedItem != RepeatMode.Нет && string.IsNullOrWhiteSpace(repeatValueEntry.Text))
                {
                    throw new Exception("Одно из полей не заполнено.");
                }
                else
                {
                    model.Text = textEntry.Text;
                    model.RepeatMode = (RepeatMode)repeatModePicker.SelectedItem;
                    model.RepeatValue = int.Parse(repeatValueEntry.Text);
                    IsSuccess = true;

                    await Navigation.PopAsync();
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка", ex.Message, "Ок");
            }
        }
    }
}