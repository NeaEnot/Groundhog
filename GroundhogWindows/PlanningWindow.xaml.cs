using Core;
using Core.Enums;
using System;
using System.Collections.Generic;
using System.Windows;

namespace GroundhogWindows
{
    public partial class PlanningWindow : Window
    {
        public PlanningWindow()
        {
            InitializeComponent();

            tbDays.Text = GroundhogContext.Settings.PlanningRanges[RepeatMode.Дни].ToString();
            tbDaysOfWeek.Text = GroundhogContext.Settings.PlanningRanges[RepeatMode.ДниНедели].ToString();
            tbWatches.Text = GroundhogContext.Settings.PlanningRanges[RepeatMode.Вахты].ToString();
            tbDayOfMounth.Text = GroundhogContext.Settings.PlanningRanges[RepeatMode.ЧислоМесяца].ToString();
            tbDayOfYear.Text = GroundhogContext.Settings.PlanningRanges[RepeatMode.ДеньГода].ToString();

            tbOptimization.Text = GroundhogContext.Settings.OptimizationRange.ToString();

            tbDays.Focus();
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

                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
