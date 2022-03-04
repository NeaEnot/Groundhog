using Core;
using Core.Enums;
using GroundhogMobile.Models;
using System;

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

                GroundhogContext.SetPlanningRange(RepeatMode.Дни, days);
                GroundhogContext.SetPlanningRange(RepeatMode.ДниНедели, daysOfWeek);
                GroundhogContext.SetPlanningRange(RepeatMode.Вахты, watches);
                GroundhogContext.SetPlanningRange(RepeatMode.ЧислоМесяца, dayOfMounth);
                GroundhogContext.SetPlanningRange(RepeatMode.ДеньГода, dayOfYear);

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