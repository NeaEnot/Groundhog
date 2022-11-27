﻿using Core;
using GroundhogWindows.Models;
using GroundhogWindows.Views.Notes;
using GroundhogWindows.Views.Purposes;
using GroundhogWindows.Views.Settings;
using GroundhogWindows.Views.Tasks;
using System;
using System.Timers;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace GroundhogWindows.Views
{
    public partial class MainWindow : Window
    {
        private TaskInstancesPage tiPage;
        private SelectDatePage sdPage;

        private SelectGroupPage sgPage;
        private PurposesPage pPage;

        private SelectNotePage snPage;
        private NotePage nPage;

        internal Action<DateTime> LoadTasks;
        internal Action LoadPurposeGroups;
        internal Action<string> LoadPurposes;
        internal Action LoadNotes;
        internal Action<NoteViewModel> LoadNote;

        public MainWindow()
        {
            InitializeComponent();

            LoadResources();

            sdPage = new SelectDatePage(this);
            tiPage = new TaskInstancesPage();
            sgPage = new SelectGroupPage(this);
            pPage = new PurposesPage(this);
            snPage = new SelectNotePage(this);
            nPage = new NotePage();

            fDates.Content = sdPage;
            fInstances.Content = tiPage;
            fGroups.Content = sgPage;
            fPurposes.Content = pPage;
            fNotes.Content = snPage;
            fNote.Content = nPage;

            LoadTasks = tiPage.LoadTasksInstances;
            LoadPurposeGroups = sgPage.LoadGroups;
            LoadPurposes = pPage.LoadPurposes;
            LoadNotes = snPage.LoadNotes;
            LoadNote = nPage.LoadText;

            int minutes = 1;

            Timer timer = new Timer(minutes * 60 * 1000);

            timer.Elapsed += (sender, e) =>
            {
                if (DateTime.Now.Date > DateTime.Now.AddMinutes(-minutes).Date)
                    Dispatcher.BeginInvoke(DispatcherPriority.Input, new Action(tiPage.LoadTasksInstances));
            };

            timer.Start();
        }

        private void LoadResources()
        {
            App.Current.Resources["Main color"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString(GroundhogContext.Settings.ColorSchema.Colors["Main color"]));
            App.Current.Resources["Additional color"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString(GroundhogContext.Settings.ColorSchema.Colors["Additional color"]));
            App.Current.Resources["Main text"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString(GroundhogContext.Settings.ColorSchema.Colors["Main text"]));
            App.Current.Resources["Additional text"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString(GroundhogContext.Settings.ColorSchema.Colors["Additional text"]));
            App.Current.Resources["Selected item"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString(GroundhogContext.Settings.ColorSchema.Colors["Selected item"]));
            App.Current.Resources["Selected item inactive"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString(GroundhogContext.Settings.ColorSchema.Colors["Selected item inactive"]));
            App.Current.Resources["Select item"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString(GroundhogContext.Settings.ColorSchema.Colors["Select item"]));
        }

        private void RestartWindow()
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
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
            if (colorsWindow.ShowDialog() == true)
                RestartWindow();
        }

        private void MenuItemLoad_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ConnectIfNot();
                GroundhogContext.NetworkLogic.Load();

                RestartWindow();
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