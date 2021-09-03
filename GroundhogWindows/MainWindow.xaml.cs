using Core.Enums;
using Core.Models;
using GroundhogWindows.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace GroundhogWindows
{
    public partial class MainWindow : Window
    {
        private DateTime selectedDate;

        public MainWindow()
        {
            InitializeComponent();
            LoadDates();
        }

        private void LoadDates()
        {
            List<DateTime> dates = new List<DateTime>();
            for (int i = 0; i < 20; i++)
            {
                dates.Add(DateTime.Now.AddDays(i));
            }
            listBoxDates.ItemsSource = dates;
        }

        private void LoadTasks()
        {
            List<TaskInstanceViewModel> taskInstances =
                App.TaskInstanceLogic.Read(selectedDate)
                .Select(req => new TaskInstanceViewModel
                {
                    Id = req.Id,
                    Date = req.Date,
                    Completed = req.Completed,
                    TaskId = req.TaskId
                })
                .ToList();

            listBoxTasks.ItemsSource = null;
            listBoxTasks.ItemsSource = taskInstances;
        }

        private void DateSelected(object sender, RoutedEventArgs e)
        {
            if (sender is ListBox)
            {
                selectedDate = (DateTime)((ListBox)sender).SelectedItem;
            }
            if (sender is Calendar)
            {
                selectedDate = (DateTime)((Calendar)sender).SelectedDate;
            }

            LoadTasks();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            TaskInstanceViewModel viewModel = (TaskInstanceViewModel)((CheckBox)sender).DataContext;
            TaskInstance model = new TaskInstance
            {
                Id = viewModel.Id,
                Completed = ((CheckBox)sender).IsChecked.Value,
                Date = viewModel.Date,
                TaskId = viewModel.TaskId
            };

            App.TaskInstanceLogic.Update(model);

            LoadTasks();
        }

        private void MenuItemAccaunts_Click(object sender, RoutedEventArgs e)
        {
            AccauntsWindow nodeWindow = new AccauntsWindow();
            nodeWindow.ShowDialog();
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            TaskWindow window = new TaskWindow(null);
            if (window.ShowDialog() == true)
            {
                App.TaskLogic.Create(window.Task);

                if (window.Task.RepeatMode == RepeatMode.Нет)
                {
                    App.TaskInstanceLogic
                        .Create(new TaskInstance
                        {
                            TaskId = window.Task.Id,
                            Completed = false,
                            Date = selectedDate
                        });
                }

                LoadTasks();
            }
        }

        private void listBoxTasks_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            TaskInstanceViewModel viewModel = (TaskInstanceViewModel)((ListBox)sender).SelectedItem;
            if (viewModel != null)
            {
                Task task = App.TaskLogic.Read(viewModel.TaskId);

                TaskWindow window = new TaskWindow(task);
                if (window.ShowDialog() == true)
                {
                    App.TaskLogic.Update(window.Task);

                    LoadTasks();
                }
            }
        }
    }
}
