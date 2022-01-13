using Core;
using Core.Enums;
using Core.Models;
using GroundhogMobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GroundhogMobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TasksPage : ContentPage
    {
        private DateTime date;

        public TasksPage(DateTime date)
        {
            InitializeComponent();

            Resources.Add("MenuItemUpdate", MenuItemUpdate);
            Resources.Add("MenuItemDelete", MenuItemDelete);
            Resources.Add("MenuItemDeleteAll", MenuItemDeleteAll);

            this.date = date;

            LoadData();
        }

        private void LoadData()
        {
            DateTimeHelper.FillRepeatedTasks();

            List<string> tasksIds =
                GroundhogContext.TaskLogic
                .Read()
                .Select(req => req.Id)
                .ToList();

            List<TaskInstanceViewModel> list =
                GroundhogContext.TaskInstanceLogic
                .Read(date)
                .Where(req => tasksIds.Contains(req.TaskId))
                .Select(req => new TaskInstanceViewModel(req))
                .ToList();

            tasksList.ItemsSource = list;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            TaskPage page = new TaskPage(new TaskViewModel());
            page.Disappearing += (sender2, e2) =>
            {
                if (page.IsSuccess)
                {
                    Task task = page.Model.Task;
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
                        if (repeatMode != page.Model.Task.RepeatMode)
                        {
                            List<TaskInstance> instances = GroundhogContext.TaskInstanceLogic.Read(page.Model.Task.Id);
                            instances.Sort((a, b) => (a.Date - b.Date).Milliseconds);

                            for (int i = 1; i < instances.Count; i++)
                                GroundhogContext.TaskInstanceLogic.Delete(instances[i].Id);

                            DateTime computedDate = DateTimeHelper.GetDateForTask(page.Model.Task, date);

                            if (page.Model.Task.RepeatMode == RepeatMode.ЧислоМесяца &&
                                instances[0].Date.Date != date.Date)
                            {
                                GroundhogContext.TaskInstanceLogic.Delete(instances[0].Id);

                                GroundhogContext.TaskInstanceLogic
                                        .Create(new TaskInstance
                                        {
                                            TaskId = page.Model.Task.Id,
                                            Completed = false,
                                            Date = date
                                        });
                            }
                        }

                        GroundhogContext.TaskLogic.Update(page.Model.Task);
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
                foreach (TaskInstance instance in instances)
                    GroundhogContext.TaskInstanceLogic.Delete(instance.Id);
                GroundhogContext.TaskLogic.Delete(instanceModel.TaskId);
                LoadData();
            });

        private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            TaskInstanceViewModel viewModel = (TaskInstanceViewModel)((CheckBox)sender).BindingContext;
            TaskInstance model = new TaskInstance
            {
                Id = viewModel.Id,
                Completed = ((CheckBox)sender).IsChecked,
                Date = viewModel.Date,
                TaskId = viewModel.TaskId
            };

            GroundhogContext.TaskInstanceLogic.Update(model);

            LoadData();
        }
    }
}