using Core;
using Core.DateTimeHelpers;
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
            DateTimeHelper.ToDay(DateTime.Now);
            DateTimeHelper.DeleteOldTasks();

            List<Task> tasks =
                GroundhogContext.TaskLogic
                .Read()
                .ToList();

            List<TaskInstanceViewModel> list =
                GroundhogContext.TaskInstanceLogic
                .Read(date)
                .OrderByDescending(req => DateTimeHelper.TaskRare(tasks.First(t => t.Id == req.TaskId)))
                .ThenBy(req => tasks.First(t => t.Id == req.TaskId).Text)
                .Select(req => new TaskInstanceViewModel(req, tasks.First(t => t.Id == req.TaskId)))
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

                            if (page.Model.RepeatMode == RepeatMode.ЧислоМесяца &&
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
    }
}