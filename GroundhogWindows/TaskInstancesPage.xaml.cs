﻿using Core;
using Core.DateTimeHelpers;
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
    /// <summary>
    /// Логика взаимодействия для TaskInstancesPage.xaml
    /// </summary>
    public partial class TaskInstancesPage : Page
    {
        private MainWindow windowContext;

        public TaskInstancesPage(MainWindow windowContext)
        {
            InitializeComponent();

            this.windowContext = windowContext;
        }

        internal void LoadTasks()
        {
            DateTimeHelper.FillRepeatedTasks();
            DateTimeHelper.ToDay(DateTime.Now);
            DateTimeHelper.DeleteOldTasks();

            List<Task> tasks =
                GroundhogContext.TaskLogic
                .Read()
                .ToList();

            List<TaskInstanceViewModel> taskInstances =
                GroundhogContext.TaskInstanceLogic
                .Read(windowContext.selectedDate)
                .OrderByDescending(req => DateTimeHelper.TaskRare(tasks.First(t => t.Id == req.TaskId)))
                .ThenBy(req => tasks.First(t => t.Id == req.TaskId).Text)
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
                            Date = DateTimeHelper.GetDateForTask(window.Task, windowContext.selectedDate)
                        });
                LoadTasks();
            }
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
                string repeatValue = task.RepeatValue;

                TaskWindow window = new TaskWindow(task);
                if (window.ShowDialog() == true)
                {
                    if (repeatMode != window.Task.RepeatMode || repeatValue != window.Task.RepeatValue)
                    {
                        List<TaskInstance> instances = GroundhogContext.TaskInstanceLogic.Read(window.Task.Id);
                        instances.Sort((a, b) => (a.Date - b.Date).Milliseconds);
                        List<TaskInstance> instancesToDelete = instances.Where(req => req.Date.Date > windowContext.selectedDate.Date).ToList();

                        GroundhogContext.TaskInstanceLogic.Delete(instancesToDelete.Select(req => req.Id).ToList());
                        instances.RemoveAll(req => req.Date.Date > windowContext.selectedDate.Date);

                        DateTime date = DateTimeHelper.GetDateForTask(window.Task, windowContext.selectedDate);

                        if (window.Task.RepeatMode == RepeatMode.ЧислоМесяца &&
                            instances[0].Date.Date != date.Date)
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
    }
}
