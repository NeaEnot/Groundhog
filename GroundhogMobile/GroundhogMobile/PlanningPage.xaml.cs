using Core;
using Core.Enums;
using GroundhogMobile.Models;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GroundhogMobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlanningPage : ContentPage
    {
        public PlanningPage()
        {
            InitializeComponent();

            PlanningSettings settings = new PlanningSettings
            {
                Days = GroundhogContext.GetPlanningRange(RepeatMode.Дни),
                DaysOfWeek = GroundhogContext.GetPlanningRange(RepeatMode.ДниНедели),
                Watches = GroundhogContext.GetPlanningRange(RepeatMode.Вахты),
                DayOfMounth = GroundhogContext.GetPlanningRange(RepeatMode.ЧислоМесяца),
                DayOfYear = GroundhogContext.GetPlanningRange(RepeatMode.ДеньГода),
                Optimization = GroundhogContext.OptimizationRange,
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

                GroundhogContext.SetPlanningRanges(dict);
                GroundhogContext.OptimizationRange = optimization;

                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка", ex.Message, "Ок");
            }
        }
    }
}