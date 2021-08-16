using Core.Models;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace GroundhogWindows
{
    public partial class AccauntsWindow : Window
    {
        private List<Accaunt> accaunts;

        public AccauntsWindow()
        {
            InitializeComponent();

            accaunts = new List<Accaunt>();

            accaunts.Add(new Accaunt { Id = "001", Name = "Тест 1", ConnetionString = "connection" });
            accaunts.Add(new Accaunt { Id = "002", Name = "Тест 2", ConnetionString = "connection" });

            LoadData();
        }

        private void LoadData()
        {
            comboBox.ItemsSource = null;
            comboBox.ItemsSource = accaunts;
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
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
    }
}
