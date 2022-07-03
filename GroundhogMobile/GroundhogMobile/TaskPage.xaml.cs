using Core.DateTimeHelpers;
using Core.Enums;
using GroundhogMobile.Models;
using System;
using System.Collections.Generic;
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

        private Dictionary<RepeatMode, string> placeholders = new Dictionary<RepeatMode, string>()
        {
            { RepeatMode.Нет, "" },
            { RepeatMode.Дни, "Количество дней" },
            { RepeatMode.ЧислоМесяца, "Число" },
            { RepeatMode.ДеньГода, "мм.дд" },
            { RepeatMode.ДниНедели, "Пн,Вт,..." },
            { RepeatMode.Вахты, "'xx-xx', 'xx-xx-xx-xx' ..." },
        };

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
                RepeatMode selected = (RepeatMode)repeatModePicker.SelectedItem;
                repeatValueEntry.IsVisible = selected != RepeatMode.Нет;
                repeatValueEntry.Placeholder = placeholders[selected];
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textEntry.Text) ||
                    (RepeatMode)repeatModePicker.SelectedItem != RepeatMode.Нет && string.IsNullOrWhiteSpace(repeatValueEntry.Text))
                {
                    throw new Exception("Поля должны быть заполнены.");
                }

                DateTimeHelper.CheckIsValueCorrect(repeatValueEntry.Text, (RepeatMode)repeatModePicker.SelectedItem);

                Model.Text = textEntry.Text;
                Model.RepeatMode = (RepeatMode)repeatModePicker.SelectedItem;
                Model.RepeatValue = repeatValueEntry.Text;
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