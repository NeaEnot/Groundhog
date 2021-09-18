using Core;
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
        internal TaskViewModel Model { get; private set; }

        private bool isLoaded;

        internal TaskPage(TaskViewModel model)
        {
            isLoaded = true;

            if (model == null)
                throw new ArgumentNullException("При создании новой задачи необходимо передавать новый объект с пустыми полями.");

            InitializeComponent();

            Model = model;
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
                    Model.Text = textEntry.Text;
                    Model.RepeatMode = (RepeatMode)repeatModePicker.SelectedItem;
                    Model.RepeatValue = int.Parse(repeatValueEntry.Text);
                    Model.AccauntId = GroundhogContext.Accaunt.Id;
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