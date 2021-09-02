using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace GroundhogWindows
{
    public partial class AccauntsWindow : Window
    {
        public AccauntsWindow()
        {
            InitializeComponent();

            LoadData();
        }

        private void LoadData()
        {
            List<Accaunt> list = App.AccauntLogic.Read();

            comboBox.ItemsSource = null;
            comboBox.ItemsSource = list;

            if (App.Accaunt != null)
            {
                foreach (Accaunt acc in list)
                {
                    if (acc.Id == App.Accaunt.Id)
                    {
                        comboBox.SelectedItem = acc;
                        break;
                    }
                }
            }
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBox.SelectedItem != null)
                App.Accaunt = (Accaunt)comboBox.SelectedItem;
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AccauntWindow window = new AccauntWindow(null);
                if (window.ShowDialog() == true)
                {
                    App.AccauntLogic.Create(window.Accaunt);
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Accaunt model = (Accaunt)comboBox.SelectedItem;
                if (model == null)
                    throw new Exception("Не выбран аккаунт для изменения.");

                AccauntWindow window = new AccauntWindow(model);
                if (window.ShowDialog() == true)
                {
                    App.AccauntLogic.Update(window.Accaunt);
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Accaunt model = (Accaunt)comboBox.SelectedItem;
                if (model == null)
                    throw new Exception("Не выбран аккаунт для изменения.");

                App.Accaunt = null;
                App.AccauntLogic.Delete(model.Id);

                List<Task> tasks = App.TaskLogic.Read(model);

                foreach (Task task in tasks)
                {
                    List<TaskInstance> instances = App.TaskInstanceLogic.Read(task.Id);

                    foreach (TaskInstance instance in instances)
                    {
                        App.TaskInstanceLogic.Delete(instance.Id);
                    }

                    App.TaskLogic.Delete(task.Id);
                }

                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
