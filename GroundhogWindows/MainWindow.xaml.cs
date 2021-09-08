using Core;
using Core.Enums;
using Core.Models;
using GroundhogWindows.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
            GroundhogContext.FillRepeatedTasks();

            List<TaskInstanceViewModel> taskInstances =
                GroundhogContext.TaskInstanceLogic.Read(selectedDate)
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

            GroundhogContext.TaskInstanceLogic.Update(model);

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
                GroundhogContext.TaskLogic.Create(window.Task);

                DateTime date = selectedDate;
                if (window.Task.RepeatMode == RepeatMode.ЧислоМесяца)
                {
                    int days = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
                    if (days < window.Task.RepeatValue)
                        date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, days);
                    else
                        date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, window.Task.RepeatValue);
                }

                GroundhogContext.TaskInstanceLogic
                        .Create(new TaskInstance
                        {
                            TaskId = window.Task.Id,
                            Completed = false,
                            Date = date
                        });

                LoadTasks();
            }
        }

        private void listBoxTasks_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            UpdateTask();
        }

        private void ContextMenuUpdate_Click(object sender, RoutedEventArgs e)
        {
            UpdateTask();
        }

        private void UpdateTask()
        {
            TaskInstanceViewModel viewModel = (TaskInstanceViewModel)listBoxTasks.SelectedItem;

            if (viewModel != null)
            {
                Task task = GroundhogContext.TaskLogic.Read(viewModel.TaskId);

                TaskWindow window = new TaskWindow(task);
                if (window.ShowDialog() == true)
                {
                    GroundhogContext.TaskLogic.Update(window.Task);

                    LoadTasks();
                }
            }
        }

        private void ContextMenuDelete_Click(object sender, RoutedEventArgs e)
        {
            TaskInstanceViewModel viewModel = (TaskInstanceViewModel)listBoxTasks.SelectedItem;

            if (viewModel != null)
            {
                GroundhogContext.TaskInstanceLogic.Delete(viewModel.Id);
                LoadTasks();
            }
        }

        private void ContextMenuDeleteAll_Click(object sender, RoutedEventArgs e)
        {
            TaskInstanceViewModel viewModel = (TaskInstanceViewModel)listBoxTasks.SelectedItem;

            if (viewModel != null)
            {
                List<TaskInstance> instances = GroundhogContext.TaskInstanceLogic.Read(viewModel.TaskId);
                foreach (TaskInstance instance in instances)
                {
                    GroundhogContext.TaskInstanceLogic.Delete(instance.Id);
                }
                GroundhogContext.TaskLogic.Delete(viewModel.TaskId);
                LoadTasks();
            }
        }
    }
}
