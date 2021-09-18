using Core;
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
            GroundhogContext.FillRepeatedTasks();

            List<string> tasksIds =
                GroundhogContext.TaskLogic
                .Read(GroundhogContext.Accaunt)
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
            if (GroundhogContext.Accaunt == null)
                await DisplayAlert("Ошибка", "Пользователь не авторизирован.", "Ок");
            else
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
                                    Date = date
                                });
                        LoadData();
                    }
                };

                await Navigation.PushAsync(page);
            }
        }

        public ICommand MenuItemUpdate =>
            new Command<TaskInstanceViewModel>(async (instanceModel) =>
            {
                TaskPage page = new TaskPage(new TaskViewModel(GroundhogContext.TaskLogic.Read(instanceModel.TaskId)));

                page.Disappearing += (sender2, e2) =>
                {
                    if (page.IsSuccess)
                    {
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