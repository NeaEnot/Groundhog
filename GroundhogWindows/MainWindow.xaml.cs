using GroundhogWindows.Models;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace GroundhogWindows
{
    public partial class MainWindow : Window
    {
        private List<TaskInstanceViewModel> list;

        public MainWindow()
        {
            InitializeComponent();

            List<DateTime> dates = new List<DateTime>();
            for (int i = 0; i < 20; i++)
            {
                dates.Add(DateTime.Now.AddDays(i));
            }

            listBoxDates.ItemsSource = dates;
        }

        private void DateSelected(object sender, RoutedEventArgs e)
        {
            DateTime date = DateTime.MinValue;

            if (sender is ListBox)
            {
                date = (DateTime)((ListBox)sender).SelectedItem;
            }
            if (sender is Calendar)
            {
                date = (DateTime)((Calendar)sender).SelectedDate;
            }

            List<TaskInstanceViewModel> taskInstances = new List<TaskInstanceViewModel>();
            list = taskInstances;

            for (int i = 0; i < 10; i++)
            {
                taskInstances.Add(new TaskInstanceViewModel { Completed = false, Text = $"{i} задача" });
            }

            listBoxTasks.ItemsSource = list;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ((TaskInstanceViewModel)((CheckBox)sender).DataContext).Completed = ((CheckBox)sender).IsChecked.Value;
            listBoxTasks.ItemsSource = null;
            listBoxTasks.ItemsSource = list;
        }

        private void MenuItemAccaunts_Click(object sender, RoutedEventArgs e)
        {
            AccauntsWindow nodeWindow = new AccauntsWindow();
            nodeWindow.ShowDialog();
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            TaskWindow window = new TaskWindow();
            if (window.ShowDialog() == true)
            {
                
            }
        }
    }
}
