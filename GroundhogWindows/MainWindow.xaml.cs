using Core;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace GroundhogWindows
{
    public partial class MainWindow : Window
    {
        public DateTime selectedDate;

        private TaskInstancesPage tiPage;

        public MainWindow()
        {
            InitializeComponent();

            tiPage = new TaskInstancesPage(this);
            fInstances.Content = tiPage;

            LoadDates();
        }

        private void LoadDates()
        {
            List<DateTime> dates = new List<DateTime>();
            for (int i = 0; i < 20; i++)
            {
                dates.Add(DateTime.Now.AddDays(i));
            }
            listBoxDates.ItemsSource = dates;
        }

        private void DateSelected(object sender, RoutedEventArgs e)
        {
            if (sender is ListBox)
            {
                selectedDate = (DateTime)((ListBox)sender).SelectedItem;
            }
            if (sender is Calendar)
            {
                selectedDate = (DateTime)((Calendar)sender).SelectedDate;
            }

            tiPage.LoadTasks();
        }

        private void MenuItemConnection_Click(object sender, RoutedEventArgs e)
        {
            ConnectionWindow connectionWindow = new ConnectionWindow();
            connectionWindow.ShowDialog();
        }

        private void MenuItemPlanning_Click(object sender, RoutedEventArgs e)
        {
            PlanningWindow planningWindow = new PlanningWindow();
            planningWindow.ShowDialog();
        }

        private void MenuItemLoad_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ConnectIfNot();
                GroundhogContext.NetworkLogic.Load();
                tiPage.LoadTasks();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MenuItemUpload_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ConnectIfNot();
                GroundhogContext.NetworkLogic.Upload();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ConnectIfNot()
        {
            if (!GroundhogContext.NetworkLogic.IsConnected())
            {
                Func<string> f = () =>
                {
                    CodeWindow window = new CodeWindow();
                    if (window.ShowDialog() == true)
                        return window.Code;
                    throw new Exception("Код не получен.");
                };

                GroundhogContext.NetworkLogic.Connect(f);
            }
        }
    }
}
