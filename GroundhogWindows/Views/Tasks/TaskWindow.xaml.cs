using Core;
using Core.DateTimeHelpers;
using Core.Enums;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace GroundhogWindows.Views.Tasks
{
    public partial class TaskWindow : Window
    {
        public Task Task { get; private set; }

        private Dictionary<string, RepeatMode> modes = new Dictionary<string, RepeatMode>()
        {
            { GroundhogContext.Settings.Language.NonePlanning, RepeatMode.None },
            { GroundhogContext.Settings.Language.DaysPlanning, RepeatMode.Days },
            { GroundhogContext.Settings.Language.DaysOfMonthPlanning, RepeatMode.DayOfMonth },
            { GroundhogContext.Settings.Language.DaysOfYearPlanning, RepeatMode.DayOfYear },
            { GroundhogContext.Settings.Language.DaysOfWeekPlanning, RepeatMode.DaysOfWeek },
            { GroundhogContext.Settings.Language.WatchesPlanning, RepeatMode.Wathes },
        };

        private Dictionary<RepeatMode, string> toolTips = new Dictionary<RepeatMode, string>()
        {
            { RepeatMode.None, "" },
            { RepeatMode.Days, "Число" },
            { RepeatMode.DayOfMonth, "Число" },
            { RepeatMode.DayOfYear, "мм.дд" },
            { RepeatMode.DaysOfWeek, "Пн,Вт,..." },
            { RepeatMode.Wathes, "'xx-xx', 'xx-xx-xx-xx' ..." },
        };

        public TaskWindow(Task task)
        {
            InitializeComponent();

            comboBox.ItemsSource = modes.Keys;

            if (task != null)
            {
                Task = task;

                textBoxText.Text = task.Text;
                comboBox.SelectedItem = modes.First(req => req.Value == task.RepeatMode).Key;
                textBoxValue.Text = task.RepeatValue;
                checkBoxToNextDay.IsChecked = task.ToNextDay;
                checkBoxOffsetAll.IsEnabled = task.ToNextDay == true;
                checkBoxOffsetAll.IsChecked = task.ToNextDay && task.OffsetAll;
                textBoxOptimizationRange.Text = task.OptimizationRange.ToString();
            }
            else
            {
                comboBox.SelectedItem = GroundhogContext.Settings.Language.NonePlanning;
                textBoxOptimizationRange.Text = GroundhogContext.Settings.OptimizationRange.ToString();
                Task = new Task();
            }

            textBoxText.Focus();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textBoxText.Text) ||
                    modes[comboBox.SelectedItem.ToString()] != RepeatMode.None &&
                    (string.IsNullOrWhiteSpace(textBoxValue.Text) || string.IsNullOrWhiteSpace(textBoxPlanningRange.Text)) ||
                    string.IsNullOrWhiteSpace(textBoxOptimizationRange.Text))
                    throw new Exception("Поля должны быть заполнены.");

                DateTimeHelper.CheckIsValueCorrect(textBoxValue.Text, (RepeatMode)comboBox.SelectedItem);

                Task.Text = textBoxText.Text;
                Task.RepeatMode = modes[comboBox.SelectedItem.ToString()];
                Task.RepeatValue = textBoxValue.Text;
                Task.ToNextDay = checkBoxToNextDay.IsChecked.Value;
                Task.OffsetAll = checkBoxOffsetAll.IsChecked.Value;
                Task.PlanningRange = modes[comboBox.SelectedItem.ToString()] == RepeatMode.None ? 0 : int.Parse(textBoxPlanningRange.Text);
                Task.OptimizationRange = int.Parse(textBoxOptimizationRange.Text);

                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void comboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            RepeatMode selected = modes[comboBox.SelectedItem.ToString()];

            textBoxValue.IsEnabled = selected != RepeatMode.None;
            textBoxValue.ToolTip = toolTips[selected];

            textBoxPlanningRange.IsEnabled = selected != RepeatMode.None;
            if (textBoxPlanningRange.IsEnabled)
                textBoxPlanningRange.Text = GroundhogContext.Settings.PlanningRanges[selected].ToString();
            else
                textBoxPlanningRange.Text = "";

            EnableOffset();
        }

        private void checkBoxToNextDay_Checked(object sender, RoutedEventArgs e)
        {
            EnableOffset();
        }

        private void EnableOffset()
        {
            checkBoxOffsetAll.IsEnabled = checkBoxToNextDay.IsChecked == true && modes[comboBox.SelectedItem.ToString()] != RepeatMode.None;
            if (!checkBoxOffsetAll.IsEnabled)
                checkBoxOffsetAll.IsChecked = false;
        }
    }
}
