using Core;
using System;
using System.Windows;

namespace GroundhogWindows
{
    public partial class MainWindow : Window
    {
        private TaskInstancesPage tiPage;
        private SelectDatePage sdPage;

        internal DateTime SelectedDate => sdPage.SelectedDate;
        internal Action LoadTasks;

        public MainWindow()
        {
            InitializeComponent();

            sdPage = new SelectDatePage(this);
            fDates.Content = sdPage;

            tiPage = new TaskInstancesPage(this);
            fInstances.Content = tiPage;

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
    }
}
