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
                Days = GroundhogContext.Settings.PlanningRanges[RepeatMode.Days],
                DaysOfWeek = GroundhogContext.Settings.PlanningRanges[RepeatMode.DaysOfWeek],
                Watches = GroundhogContext.Settings.PlanningRanges[RepeatMode.Wathes],
                DayOfMounth = GroundhogContext.Settings.PlanningRanges[RepeatMode.DayOfMonth],
                DayOfYear = GroundhogContext.Settings.PlanningRanges[RepeatMode.DayOfYear],
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
                    { RepeatMode.Days, days },
                    { RepeatMode.DaysOfWeek, daysOfWeek },
                    { RepeatMode.Wathes, watches },
                    { RepeatMode.DayOfMonth, dayOfMounth },
                    { RepeatMode.DayOfYear, dayOfYear },
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