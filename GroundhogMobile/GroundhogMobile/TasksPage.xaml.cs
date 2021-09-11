using GroundhogMobile.Models;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GroundhogMobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TasksPage : ContentPage
    {
        public TasksPage(DateTime date)
        {
            InitializeComponent();

            Resources.Add("MenuItemUpdate", MenuItemUpdate);
            Resources.Add("MenuItemDelete", MenuItemDelete);
            Resources.Add("MenuItemDeleteAll", MenuItemDeleteAll);

            LoadData();
        }

        private void LoadData()
        {
            List<TaskInstanceViewModel> list = new List<TaskInstanceViewModel>();
            for (int i = 0; i < 15; i++)
            {
                list.Add(new TaskInstanceViewModel
                {
                    Completed = i % 3 == 0,
                    Text = $"Задача номер {i}",
                    Repeated = i % 2 == 0
                });
            }
            list.Add(new TaskInstanceViewModel
            {
                Completed = false,
                Text = $"Задача номер Задача номер Задача номер Задача номер Задача номер Задача номер Задача номер Задача номер Задача номер Задача номер Задача номер "
            });

            tasksList.ItemsSource = list;
        }

        private async void WorkWithTask(TaskViewModel model)
        {
            TaskPage page = new TaskPage(model);
            page.Disappearing += (sender2, e2) =>
            {
                if (page.IsSuccess)
                    LoadData();
            };

            await Navigation.PushAsync(page);
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            WorkWithTask(new TaskViewModel());
        }

        public ICommand MenuItemUpdate =>
            new Command<TaskInstanceViewModel>((instanceModel) =>
                WorkWithTask(new TaskViewModel { Text = instanceModel.Text }));

        public ICommand MenuItemDelete =>
            new Command<TaskInstanceViewModel>(async (instanceModel) => { });

        public ICommand MenuItemDeleteAll =>
            new Command<TaskInstanceViewModel>(async (instanceModel) => { });
    }
}