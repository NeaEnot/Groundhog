using Core;
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
                .Read(windowContext.SelectedDate)
                .OrderByDescending(req => DateTimeHelper.TaskRare(tasks.First(t => t.Id == req.TaskId)))
                .ThenBy(req => tasks.First(t => t.Id == req.TaskId).Text)
                .Select(req => new TaskInstanceViewModel(req, tasks.First(t => t.Id == req.TaskId)))
                .ToList();

            listBoxTasks.ItemsSource = null;
            listBoxTasks.ItemsSource = taskInstances;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            TaskInstanceViewModel viewModel = (TaskInstanceViewModel)((CheckBox)sender).DataContext;
            TaskInstance model = viewModel.Convert();

            GroundhogContext.TaskInstanceLogic.Update(model);

            LoadTasks();
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
                            Date = DateTimeHelper.GetDateForTask(window.Task, windowContext.SelectedDate)
                        });
                LoadTasks();
            }
        }

        private void ContextMenuClone_Click(object sender, RoutedEventArgs e)
        {
            TaskInstanceViewModel viewModel = (TaskInstanceViewModel)listBoxTasks.SelectedItem;

            if (viewModel != null)
            {
                Task task = GroundhogContext.TaskLogic.Read(viewModel.TaskId);
                Task cloneTask = new Task
                {
                    Id = null,
                    Text = task.Text,
                    RepeatMode = task.RepeatMode,
                    RepeatValue = task.RepeatValue,
                    ToNextDay = task.ToNextDay
                };

                GroundhogContext.TaskLogic.Create(cloneTask);

                GroundhogContext.TaskInstanceLogic
                        .Create(new TaskInstance
                        {
                            TaskId = cloneTask.Id,
                            Completed = false,
                            Date = viewModel.Date
                        });
                LoadTasks();
            }
        }

        private void ContextMenuUpdate_Click(object sender, RoutedEventArgs e)
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
                        List<TaskInstance> instancesToDelete = instances.Where(req => req.Date.Date > windowContext.SelectedDate.Date).ToList();

                        GroundhogContext.TaskInstanceLogic.Delete(instancesToDelete.Select(req => req.Id).ToList());
                        instances.RemoveAll(req => req.Date.Date > windowContext.SelectedDate.Date);

                        DateTime date = DateTimeHelper.GetDateForTask(window.Task, windowContext.SelectedDate);

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

        private void UpdateTask()
        {
            
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
                GroundhogContext.TaskInstanceLogic.Delete(instances.Select(req => req.Id).ToList());
                GroundhogContext.TaskLogic.Delete(viewModel.TaskId);
                LoadTasks();
            }
        }
    }
}
