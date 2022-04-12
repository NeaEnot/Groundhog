using Core;
using System;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace GroundhogWindows
{
    public partial class MainWindow : Window
    {
        private TaskInstancesPage tiPage;
        private SelectDatePage sdPage;

        private SelectGroupPage sgPage;
        private PurposesPage pPage;

        internal DateTime SelectedDate => sdPage.SelectedDate;
        internal string SelectedGroupId => sgPage.SelectedGroup != null ? sgPage.SelectedGroup.Id : "";

        internal Action LoadTasks;
        internal Action LoadPurposes;

        public MainWindow()
        {
            InitializeComponent();

            sdPage = new SelectDatePage(this);
            tiPage = new TaskInstancesPage(this);
            sgPage = new SelectGroupPage(this);
            pPage = new PurposesPage(this);

            fDates.Content = sdPage;
            fInstances.Content = tiPage;
            fGroups.Content = sgPage;
            fPurposes.Content = pPage;

            LoadTasks = tiPage.LoadTasks;
            LoadPurposes = pPage.LoadPurposes;

            int minutes = 1;

            Timer timer = new Timer(minutes * 60 * 1000);

            timer.Elapsed += (sender, e) =>
            {
                if (DateTime.Now.Date > DateTime.Now.AddMinutes(-minutes).Date)
                    Dispatcher.BeginInvoke(DispatcherPriority.Input, new Action(LoadTasks));
            };

            timer.Start();
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

        private void MenuItemColorSchema_Click(object sender, RoutedEventArgs e)
        {
            ColorsWindow colorsWindow = new ColorsWindow();
            colorsWindow.ShowDialog();
        }

        private void MenuItemLoad_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ConnectIfNot();
                GroundhogContext.NetworkLogic.Load();

                LoadTasks();
                LoadPurposes();
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
                pPage.LoadPurposes();
            }
        }
    }
}
