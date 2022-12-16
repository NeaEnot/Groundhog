using Core;
using Core.DateTimeHelpers;
using Core.Enums;
using GroundhogMobile.Models;
using GroundhogMobile.Views.Services;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GroundhogMobile.Views.Tasks
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TaskPage : ContentPage
    {
        public bool IsSuccess { get; private set; } = false;
        internal TaskViewModel Model { get; private set; }

        private RepeatMode repeatMode;

        private Dictionary<RepeatMode, string> placeholders = new Dictionary<RepeatMode, string>()
        {
            { RepeatMode.None, "" },
            { RepeatMode.Days, "Количество дней" },
            { RepeatMode.DayOfMonth, "Число" },
            { RepeatMode.DayOfYear, "мм.дд" },
            { RepeatMode.DaysOfWeek, "Пн,Вт,..." },
            { RepeatMode.Wathes, "'xx-xx', 'xx-xx-xx-xx' ..." }
        };

        private Dictionary<string, RepeatMode> modes = new Dictionary<string, RepeatMode>()
        {
            { GroundhogContext.Settings.Language.NonePlanning, RepeatMode.None },
            { GroundhogContext.Settings.Language.DaysPlanning, RepeatMode.Days },
            { GroundhogContext.Settings.Language.DaysOfMonthPlanning, RepeatMode.DayOfMonth },
            { GroundhogContext.Settings.Language.DaysOfYearPlanning, RepeatMode.DayOfYear },
            { GroundhogContext.Settings.Language.DaysOfWeekPlanning, RepeatMode.DaysOfWeek },
            { GroundhogContext.Settings.Language.WatchesPlanning, RepeatMode.Wathes }
        };

        internal TaskPage(TaskViewModel model)
        {
            if (model == null)
                throw new ArgumentNullException("When creating must pass new object with empty fields.");

            InitializeComponent();

            Model = model;
            BindingContext = model;

            repeatMode = model.RepeatMode;

            buttonMode.Text = modes.First(req => req.Value == model.RepeatMode).Key;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textEntry.Text) ||
                    repeatMode != RepeatMode.None && string.IsNullOrWhiteSpace(repeatValueEntry.Text))
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
            CommandPage page = new CommandPage("Режим повторения", modes.Keys);
            Device.BeginInvokeOnMainThread(async () => await PopupNavigation.Instance.PushAsync(page));

            object obj = await page.Result;
            if (obj != null)
            {
                string result = obj as string;

                repeatMode = modes[result];
                buttonMode.Text = result;
                repeatValueEntry.IsVisible = repeatMode != RepeatMode.None;
                repeatValueEntry.Placeholder = placeholders[repeatMode];

                planningRangeLabel.IsVisible = repeatMode != RepeatMode.None;
                planningRangeEntry.IsVisible = repeatMode != RepeatMode.None;
                planningRangeEntry.Text = GroundhogContext.Settings.PlanningRanges[repeatMode].ToString();

                ChangeOffsetVisible();
            }
        }

        private void chbToNextDay_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            ChangeOffsetVisible();
        }

        private void ChangeOffsetVisible()
        {
            slOffset.IsVisible = chbToNextDay.IsChecked && repeatMode != RepeatMode.None;
        }
    }
}