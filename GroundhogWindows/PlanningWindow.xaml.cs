using Core;
using Core.Enums;
using System;
using System.Windows;

namespace GroundhogWindows
{
    public partial class PlanningWindow : Window
    {
        public PlanningWindow()
        {
            InitializeComponent();

            tbDays.Text = GroundhogContext.GetPlanningRange(RepeatMode.Дни).ToString();
            tbDaysOfWeek.Text = GroundhogContext.GetPlanningRange(RepeatMode.ДниНедели).ToString();
            tbWatches.Text = GroundhogContext.GetPlanningRange(RepeatMode.Вахты).ToString();
            tbDayOfMounth.Text = GroundhogContext.GetPlanningRange(RepeatMode.ЧислоМесяца).ToString();
            tbDayOfYear.Text = GroundhogContext.GetPlanningRange(RepeatMode.ДеньГода).ToString();

            tbOptimization.Text = GroundhogContext.OptimizationRange.ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int days = int.Parse(tbDays.Text);
                int daysOfWeek = int.Parse(tbDaysOfWeek.Text);
                int watches = int.Parse(tbWatches.Text);
                int dayOfMounth = int.Parse(tbDayOfMounth.Text);
                int dayOfYear = int.Parse(tbDayOfYear.Text);

                int optimization = int.Parse(tbOptimization.Text);

                GroundhogContext.SetPlanningRange(RepeatMode.Дни, days);
                GroundhogContext.SetPlanningRange(RepeatMode.ДниНедели, daysOfWeek);
                GroundhogContext.SetPlanningRange(RepeatMode.Вахты, watches);
                GroundhogContext.SetPlanningRange(RepeatMode.ЧислоМесяца, dayOfMounth);
                GroundhogContext.SetPlanningRange(RepeatMode.ДеньГода, dayOfYear);

                GroundhogContext.OptimizationRange = optimization;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
