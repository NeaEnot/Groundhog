using Core;
using Core.Logic.DateTimeHelpers;
using Core.Enums;
using Core.Models.Storage;
using GroundhogMobile.Models;
using GroundhogMobile.Views.Services;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GroundhogMobile.Views.Tasks
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TasksPage : ContentPage
    {
        private DateTime date;

        public TasksPage(DateTime date)
        {
            InitializeComponent();

            this.date = date;
            lblDate.Text = date.ToString("dddd, dd.MM.yyyy");

            LoadData();
        }

        private async void LoadData()
        {
            DateTimeHelper.FillRepeatedTasks();
            DateTimeHelper.ToDay(DateTime.Now);
            DateTimeHelper.DeleteOldTasks();

            List<Task> tasks =
                GroundhogContext.TaskLogic
                .Read()
                .ToList();

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

                await DisplayAlert(GroundhogContext.Language.ErrorsMessages.Error, errorMessage, "Ok");
            }

            List<TaskInstanceViewModel> models =
                taskInstances
                .OrderByDescending(req => DateTimeHelper.TaskRare(tasks.First(t => t.Id == req.TaskId)))
                .ThenBy(req => tasks.First(t => t.Id == req.TaskId).Text)
                .Select(req => new TaskInstanceViewModel(req, tasks.First(t => t.Id == req.TaskId)))
                .ToList();

            tasksList.ItemsSource = models;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            TaskPage page = new TaskPage(new TaskViewModel());
            page.Disappearing += (sender2, e2) =>
            {
                if (page.IsSuccess)
                {
                    Task task = page.Model.Convert();
                    GroundhogContext.TaskLogic.Create(task);
                    GroundhogContext.TaskInstanceLogic
                            .Create(new TaskInstance
                            {
                                TaskId = task.Id,
                                Completed = false,
                                Date = DateTimeHelper.GetDateForTask(task, date)
                            });
                    LoadData();
                }
            };

            await Navigation.PushAsync(page);
        }

        public ICommand MenuItemClone =>
            new Command<TaskInstanceViewModel>((instanceModel) =>
            {
                Task task = GroundhogContext.TaskLogic.Read(instanceModel.TaskId);
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
                            Date = instanceModel.Date
                        });

                LoadData();
            });

        public ICommand MenuItemUpdate =>
            new Command<TaskInstanceViewModel>(async (instanceModel) =>
            {
                Task task = GroundhogContext.TaskLogic.Read(instanceModel.TaskId);
                TaskPage page = new TaskPage(new TaskViewModel(task));
                RepeatMode repeatMode = task.RepeatMode;

                page.Disappearing += (sender2, e2) =>
                {
                    if (page.IsSuccess)
                    {
                        if (repeatMode != page.Model.Convert().RepeatMode)
                        {
                            List<TaskInstance> instances = GroundhogContext.TaskInstanceLogic.Read(page.Model.Id);
                            instances.Sort((a, b) => (a.Date - b.Date).Milliseconds);
                            List<TaskInstance> instancesToDelete = instances.Where(req => req.Date.Date > date.Date).ToList();

                            GroundhogContext.TaskInstanceLogic.Delete(instancesToDelete.Select(req => req.Id).ToList());
                            instances.RemoveAll(req => req.Date.Date > date.Date);

                            DateTime computedDate = DateTimeHelper.GetDateForTask(page.Model.Convert(), date);

                            if (page.Model.RepeatMode == RepeatMode.DayOfMonth &&
                                instances[0].Date.Date != date.Date)
                            {
                                GroundhogContext.TaskInstanceLogic.Delete(instances[0].Id);

                                GroundhogContext.TaskInstanceLogic
                                        .Create(new TaskInstance
                                        {
                                            TaskId = page.Model.Convert().Id,
                                            Completed = false,
                                            Date = date
                                        });
                            }
                        }

                        GroundhogContext.TaskLogic.Update(page.Model.Convert());
                        LoadData();
                    }
                };

                await Navigation.PushAsync(page);
            });

        public ICommand MenuItemDelete =>
            new Command<TaskInstanceViewModel>((instanceModel) => 
            {
                GroundhogContext.TaskInstanceLogic.Delete(instanceModel.Id);
                LoadData();
            });

        public ICommand MenuItemDeleteAll =>
            new Command<TaskInstanceViewModel>((instanceModel) => 
            {
                List<TaskInstance> instances = GroundhogContext.TaskInstanceLogic.Read(instanceModel.TaskId);
                GroundhogContext.TaskInstanceLogic.Delete(instances.Select(req => req.Id).ToList());
                GroundhogContext.TaskLogic.Delete(instanceModel.TaskId);
                LoadData();
            });

        private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            TaskInstanceViewModel viewModel = (TaskInstanceViewModel)((CheckBox)sender).BindingContext;
            TaskInstance model = viewModel.Convert();

            GroundhogContext.TaskInstanceLogic.Update(model);
        }

        private async void tasksList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Dictionary<string, ICommand> commands =
                new Dictionary<string, ICommand>
                {
                    { GroundhogContext.Language.ControlCommands.Edit, MenuItemUpdate },
                    { GroundhogContext.Language.ControlCommands.Delete, MenuItemDelete },
                    { GroundhogContext.Language.ControlCommands.DeleteAllInstances, MenuItemDeleteAll }
                };

            CommandPage page = new CommandPage((e.Item as TaskInstanceViewModel).Text, commands.Keys);
            Device.BeginInvokeOnMainThread(async () => await PopupNavigation.Instance.PushAsync(page));

            object obj = await page.Result;
            if (obj != null)
                commands[obj as string].Execute(e.Item as TaskInstanceViewModel);
        }
    }
}