using Core;
using Core.Enums;
using Core.Models;
using GroundhogWindows.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
            DateTimeHelper.FillRepeatedTasks();

            List<string> tasksIds =
                GroundhogContext.TaskLogic
                .Read()
                .Select(req => req.Id)
                .ToList();

            List <TaskInstanceViewModel> taskInstances =
                GroundhogContext.TaskInstanceLogic
                .Read(selectedDate)
                .Where(req => tasksIds.Contains(req.TaskId))
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

        private void MenuItemConnection_Click(object sender, RoutedEventArgs e)
        {
            ConnectionWindow connectionWindow = new ConnectionWindow();
            connectionWindow.ShowDialog();
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            TaskWindow window = new TaskWindow(null);
            if (window.ShowDialog() == true)
            {
                GroundhogContext.TaskLogic.Create(window.Task);

                GroundhogContext.TaskInstanceLogic
                        .Create(new TaskInstance
                        {
                            TaskId = window.Task.Id,
                            Completed = false,
                            Date = DateTimeHelper.GetDateForTask(window.Task, selectedDate)
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
                RepeatMode repeatMode = task.RepeatMode;

                TaskWindow window = new TaskWindow(task);
                if (window.ShowDialog() == true)
                {
                    if (repeatMode != window.Task.RepeatMode)
                    {
                        List<TaskInstance> instances = GroundhogContext.TaskInstanceLogic.Read(window.Task.Id);
                        instances.Sort((a, b) => (a.Date - b.Date).Milliseconds);

                        for (int i = 1; i < instances.Count; i++)
                            GroundhogContext.TaskInstanceLogic.Delete(instances[i].Id);

                        DateTime date = DateTimeHelper.GetDateForTask(window.Task, selectedDate);

                        if (window.Task.RepeatMode == RepeatMode.ЧислоМесяца &&
                            instances[0].Date.ToString("yyyy.MM.dd") != date.ToString("yyyy.MM.dd"))
                        {
                            GroundhogContext.TaskInstanceLogic.Delete(instances[0].Id);

                            GroundhogContext.TaskInstanceLogic
                                    .Create(new TaskInstance
                                    {
                                        TaskId = window.Task.Id,
                                        Completed = false,
                                        Date = date
                                    });
                        }
                    }

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

        private void MenuItemLoad_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ConnectIfNot();
                GroundhogContext.NetworkLogic.Load();
                LoadTasks();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MenuItemUpload_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ConnectIfNot();
                GroundhogContext.NetworkLogic.Upload();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ConnectIfNot()
        {
            if (!GroundhogContext.NetworkLogic.IsConnected())
            {
                Func<string> f = () =>
                {
                    CodeWindow window = new CodeWindow();
                    if (window.ShowDialog() == true)
                        return window.Code;
                    throw new Exception("Код не получен.");
                };

                GroundhogContext.NetworkLogic.Connect(f);
            }
        }
    }
}
