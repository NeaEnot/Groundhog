using Core;
using Core.Enums;
using GroundhogMobile.Models;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GroundhogMobile.Views.Settings
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlanningPage : ContentPage
    {
        public PlanningPage()
        {
            InitializeComponent();

            PlanningSettings settings = new PlanningSettings
            {
                Days = GroundhogContext.Settings.PlanningRanges[RepeatMode.Дни],
                DaysOfWeek = GroundhogContext.Settings.PlanningRanges[RepeatMode.ДниНедели],
                Watches = GroundhogContext.Settings.PlanningRanges[RepeatMode.Вахты],
                DayOfMounth = GroundhogContext.Settings.PlanningRanges[RepeatMode.ЧислоМесяца],
                DayOfYear = GroundhogContext.Settings.PlanningRanges[RepeatMode.ДеньГода],
                Optimization = GroundhogContext.Settings.OptimizationRange,
            };

            BindingContext = settings;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                int days = int.Parse(daysEntry.Text);
                int daysOfWeek = int.Parse(daysOfWeekEntry.Text);
                int watches = int.Parse(watchesEntry.Text);
                int dayOfMounth = int.Parse(dayOfMounthEntry.Text);
                int dayOfYear = int.Parse(dayOfYearEntry.Text);

                int optimization = int.Parse(optimizationEntry.Text);

                Dictionary<RepeatMode, int> dict = new Dictionary<RepeatMode, int>()
                {
                    { RepeatMode.Дни, days },
                    { RepeatMode.ДниНедели, daysOfWeek },
                    { RepeatMode.Вахты, watches },
                    { RepeatMode.ЧислоМесяца, dayOfMounth },
                    { RepeatMode.ДеньГода, dayOfYear },
                };

                foreach (RepeatMode mode in dict.Keys)
                    GroundhogContext.Settings.PlanningRanges[mode] = dict[mode];
                GroundhogContext.Settings.OptimizationRange = optimization;
                GroundhogContext.SaveSettings();

                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка", ex.Message, "Ок");
            }
        }
    }
}