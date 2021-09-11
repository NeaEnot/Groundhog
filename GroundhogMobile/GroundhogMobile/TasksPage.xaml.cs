using GroundhogMobile.Models;
using System;
using System.Collections.Generic;
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

            LoadData();
        }

        private void LoadData()
        {
            List<TaskInstanceViewModel> list = new List<TaskInstanceViewModel>();
            for (int i = 0; i < 10; i++)
            {
                list.Add(new TaskInstanceViewModel
                {
                    Completed = i % 3 == 0,
                    Text = $"Задача номер {i}"
                });
            }
            list.Add(new TaskInstanceViewModel
            {
                Completed = false,
                Text = $"Задача номер Задача номер Задача номер Задача номер Задача номер Задача номер Задача номер Задача номер Задача номер Задача номер Задача номер "
            });

            tasksList.ItemsSource = list;
        }
    }
}