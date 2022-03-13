using Core;
using System;
using System.Windows;
using System.Windows.Controls;

namespace GroundhogWindows
{
    public partial class MainWindow : Window
    {
        private TaskInstancesPage tiPage;
        private SelectDatePage sdPage;

        private SelectGroupPage sgPage;

        internal DateTime SelectedDate => sdPage.SelectedDate;
        internal Action LoadTasks;
        internal Action LoadPurposes;

        public MainWindow()
        {
            InitializeComponent();

            sdPage = new SelectDatePage(this);
            fDates.Content = sdPage;

            tiPage = new TaskInstancesPage(this);
            fInstances.Content = tiPage;

            sgPage = new SelectGroupPage(this);
            fGroups.Content = sgPage;

            LoadTasks = tiPage.LoadTasks;
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

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((tc.SelectedItem as TabItem).Header == "Задачи")
            {
                sdPage.LoadDates();
                tiPage.LoadTasks();
            }
            if ((tc.SelectedItem as TabItem).Header == "Цели")
            {
                sgPage.LoadGroups();
            }
        }
    }
}
