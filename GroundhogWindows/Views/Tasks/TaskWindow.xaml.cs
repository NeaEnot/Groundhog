﻿using Core;
using Core.DateTimeHelpers;
using Core.Enums;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Windows;

namespace GroundhogWindows.Views.Tasks
{
    public partial class TaskWindow : Window
    {
        public Task Task { get; private set; }

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

            comboBox.ItemsSource = Enum.GetValues(typeof(RepeatMode));

            if (task != null)
            {
                Task = task;

                textBoxText.Text = task.Text;
                comboBox.SelectedItem = task.RepeatMode;
                textBoxValue.Text = task.RepeatValue;
                checkBoxToNextDay.IsChecked = task.ToNextDay;
                checkBoxOffsetAll.IsEnabled = task.ToNextDay == true;
                checkBoxOffsetAll.IsChecked = task.ToNextDay && task.OffsetAll;
                textBoxOptimizationRange.Text = task.OptimizationRange.ToString();
            }
            else
            {
                comboBox.SelectedItem = RepeatMode.None;
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
                    (RepeatMode)comboBox.SelectedItem != RepeatMode.None && string.IsNullOrWhiteSpace(textBoxValue.Text) ||
                    (RepeatMode)comboBox.SelectedItem != RepeatMode.None && string.IsNullOrWhiteSpace(textBoxPlanningRange.Text) ||
                    string.IsNullOrWhiteSpace(textBoxOptimizationRange.Text))
                    throw new Exception("Поля должны быть заполнены.");

                DateTimeHelper.CheckIsValueCorrect(textBoxValue.Text, (RepeatMode)comboBox.SelectedItem);

                Task.Text = textBoxText.Text;
                Task.RepeatMode = (RepeatMode)comboBox.SelectedItem;
                Task.RepeatValue = textBoxValue.Text;
                Task.ToNextDay = checkBoxToNextDay.IsChecked.Value;
                Task.OffsetAll = checkBoxOffsetAll.IsChecked.Value;
                Task.PlanningRange = (RepeatMode)comboBox.SelectedItem == RepeatMode.None ? 0 : int.Parse(textBoxPlanningRange.Text);
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
            RepeatMode selected = (RepeatMode)comboBox.SelectedItem;

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
            checkBoxOffsetAll.IsEnabled = checkBoxToNextDay.IsChecked == true && (RepeatMode)comboBox.SelectedItem != RepeatMode.None;
            if (!checkBoxOffsetAll.IsEnabled)
                checkBoxOffsetAll.IsChecked = false;
        }
    }
}
