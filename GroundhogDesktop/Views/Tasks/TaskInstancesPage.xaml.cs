using Core;
using Core.Logic.DateTimeHelpers;
using Core.Enums;
using Core.Models.Storage;
using GroundhogDesktop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace GroundhogDesktop.Views.Tasks
{
    public partial class TaskInstancesPage : Page
    {
        private DateTime selectedDate;

        public TaskInstancesPage()
        {
            InitializeComponent();
        }

        internal void LoadTasksInstances()
            => LoadTasksInstances(selectedDate);

        internal void LoadTasksInstances(DateTime date)
        {
            selectedDate = date;

            DateTimeHelper.ToDay(DateTime.Now);
            DateTimeHelper.DeleteOldTasks();
            DateTimeHelper.FillRepeatedTasks();

            List<Task> tasks =
                GroundhogContext.TaskLogic
                .Read()
                .ToList();

            listBoxTasks.ItemsSource = null;

            if (tasks != null && tasks.Count > 0)
            {
                List<TaskInstance> taskInstances = GroundhogContext.TaskInstanceLogic.Read(date);

                List<TaskInstance> errors = taskInstances.Where(req => tasks.FirstOrDefault(t => t.Id == req.TaskId) == null).ToList();

                if (errors.Count > 0)
                {
                    string errorMessage = $"{GroundhogContext.Language.ErrorsMessages.EntityWithSameIdDontExist}:\n";

                    foreach (TaskInstance taskInstance in errors)
                    {
                        errorMessage += taskInstance.TaskId + '\n';
                        taskInstances.Remove(taskInstance);
                    }

                    MessageBox.Show(errorMessage, GroundhogContext.Language.ErrorsMessages.Error, MessageBoxButton.OK, MessageBoxImage.Error);
                }

                List<TaskInstanceViewModel> models =
                    taskInstances
                    .OrderByDescending(req => DateTimeHelper.TaskRare(tasks.First(t => t.Id == req.TaskId)))
                    .ThenBy(req => tasks.First(t => t.Id == req.TaskId).Text)
                    .Select(req => new TaskInstanceViewModel(req, tasks.First(t => t.Id == req.TaskId)))
                    .ToList();

                listBoxTasks.ItemsSource = models;
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            TaskInstanceViewModel viewModel = (TaskInstanceViewModel)((CheckBox)sender).DataContext;
            TaskInstance model = viewModel.Convert();

            GroundhogContext.TaskInstanceLogic.Update(model);

            LoadTasksInstances(selectedDate);
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
                LoadTasksInstances(selectedDate);
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
                    ToNextDay = task.ToNextDay,
                    OffsetAll = task.OffsetAll,
                    OptimizationRange = task.OptimizationRange,
                    PlanningRange = task.PlanningRange
                };

                GroundhogContext.TaskLogic.Create(cloneTask);

                GroundhogContext.TaskInstanceLogic
                        .Create(new TaskInstance
                        {
                            TaskId = cloneTask.Id,
                            Completed = false,
                            Date = viewModel.Date
                        });
                LoadTasksInstances(selectedDate);
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
                        List<TaskInstance> instancesToDelete = instances.Where(req => req.Date.Date > selectedDate).ToList();

                        GroundhogContext.TaskInstanceLogic.Delete(instancesToDelete.Select(req => req.Id).ToList());
                        instances.RemoveAll(req => req.Date.Date > selectedDate);

                        DateTime date = DateTimeHelper.GetDateForTask(window.Task, selectedDate);

                        if (window.Task.RepeatMode == RepeatMode.DayOfMonth &&
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

                    LoadTasksInstances(selectedDate);
                }
            }
        }

        private void ContextMenuDelete_Click(object sender, RoutedEventArgs e)
        {
            TaskInstanceViewModel viewModel = (TaskInstanceViewModel)listBoxTasks.SelectedItem;

            if (viewModel != null)
            {
                GroundhogContext.TaskInstanceLogic.Delete(viewModel.Id);
                LoadTasksInstances(selectedDate);
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
                LoadTasksInstances(selectedDate);
            }
        }
    }
}
