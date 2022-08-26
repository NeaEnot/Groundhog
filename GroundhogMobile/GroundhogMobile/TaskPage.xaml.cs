using Core.DateTimeHelpers;
using Core.Enums;
using GroundhogMobile.Models;
using Rg.Plugins.Popup.Services;
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

        private RepeatMode repeatMode;

        private Dictionary<RepeatMode, string> placeholders = new Dictionary<RepeatMode, string>()
        {
            { RepeatMode.Нет, "" },
            { RepeatMode.Дни, "Количество дней" },
            { RepeatMode.ЧислоМесяца, "Число" },
            { RepeatMode.ДеньГода, "мм.дд" },
            { RepeatMode.ДниНедели, "Пн,Вт,..." },
            { RepeatMode.Вахты, "'xx-xx', 'xx-xx-xx-xx' ..." },
        };

        private Dictionary<RepeatMode, string> buttonText = new Dictionary<RepeatMode, string>()
        {
            { RepeatMode.Нет, "Нет" },
            { RepeatMode.Дни, "Дни" },
            { RepeatMode.ЧислоМесяца, "Число месяца" },
            { RepeatMode.ДеньГода, "День года" },
            { RepeatMode.ДниНедели, "Дни недели" },
            { RepeatMode.Вахты, "Вахты" },
        };

        internal TaskPage(TaskViewModel model)
        {
            if (model == null)
                throw new ArgumentNullException("При создании новой задачи необходимо передавать новый объект с пустыми полями.");

            InitializeComponent();

            Model = model;
            BindingContext = model;

            repeatMode = model.RepeatMode;

            buttonMode.Text = buttonText[Model.RepeatMode];
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textEntry.Text) ||
                    repeatMode != RepeatMode.Нет && string.IsNullOrWhiteSpace(repeatValueEntry.Text))
                {
                    throw new Exception("Поля должны быть заполнены.");
                }

                DateTimeHelper.CheckIsValueCorrect(repeatValueEntry.Text, repeatMode);

                Model.Text = textEntry.Text;
                Model.RepeatMode = repeatMode;
                Model.RepeatValue = repeatValueEntry.Text;

                if (!Model.ToNextDay)
                    Model.OffsetAll = false;

                IsSuccess = true;

                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка", ex.Message, "Ок");
            }
        }

        private async void buttonMode_Clicked(object sender, EventArgs e)
        {
            CommandPage page = new CommandPage("Режим повторения", Enum.GetValues(typeof(RepeatMode)));
            Device.BeginInvokeOnMainThread(async () => await PopupNavigation.Instance.PushAsync(page));

            object obj = await page.Result;
            if (obj != null)
            {
                repeatMode = (RepeatMode)obj;
                buttonMode.Text = buttonText[repeatMode];
                repeatValueEntry.IsVisible = repeatMode != RepeatMode.Нет;
                repeatValueEntry.Placeholder = placeholders[repeatMode];
            }
        }
    }
}