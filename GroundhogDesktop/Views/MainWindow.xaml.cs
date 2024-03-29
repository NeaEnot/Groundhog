﻿using Core;
using GroundhogDesktop.Models;
using GroundhogDesktop.Views.Backups;
using GroundhogDesktop.Views.Notes;
using GroundhogDesktop.Views.Purposes;
using GroundhogDesktop.Views.Settings;
using GroundhogDesktop.Views.Tasks;
using System;
using System.Timers;
using System.Windows;
using System.Windows.Threading;

namespace GroundhogDesktop.Views
{
    public partial class MainWindow : Window
    {
        private TaskInstancesPage tiPage;
        private SelectTaskGroupPage stgPage;

        private SelectGroupPage sgPage;
        private PurposesPage pPage;

        private SelectNotePage snPage;
        private NotePage nPage;

        internal Action LoadFindedTasks;
        internal Action<DateTime> LoadTasksInstances;
        internal Action<string> LoadFindedTasksInstances;
        internal Action LoadPurposeGroups;
        internal Action<string> LoadPurposes;
        internal Action LoadNotes;
        internal Action<NoteViewModel> LoadNote;

        public MainWindow()
        {
            App.ApplyColorSchema();
            App.ApplyLanguage();

            InitializeComponent();

            stgPage = new SelectTaskGroupPage(this);
            tiPage = new TaskInstancesPage(this);
            sgPage = new SelectGroupPage(this);
            pPage = new PurposesPage();
            snPage = new SelectNotePage(this);
            nPage = new NotePage();

            fDates.Content = stgPage;
            fInstances.Content = tiPage;
            fGroups.Content = sgPage;
            fPurposes.Content = pPage;
            fNotes.Content = snPage;
            fNote.Content = nPage;

            LoadFindedTasks = () => stgPage.btnFind_Click(null, null);
            LoadTasksInstances = tiPage.LoadTasksInstances;
            LoadFindedTasksInstances = tiPage.LoadTasksInstances;
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
            {
                App.ApplyColorSchema();
                RestartWindow();
            }
        }

        private void MenuItemLanguage_Click(object sender, RoutedEventArgs e)
        {
            LanguageWindow lanaguageWindow = new LanguageWindow();
            if (lanaguageWindow.ShowDialog() == true)
            {
                RestartWindow();
            }
        }

        private void MenuItemBackupSettings_Click(object sender, RoutedEventArgs e)
        {
            BackupSettingsWindow backupSettingsWindow = new BackupSettingsWindow();
            backupSettingsWindow.ShowDialog();
        }

        private void MenuItemLoad_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (GroundhogContext.Settings.BackupSettings.AutoLocalBackup)
                    GroundhogContext.LocalBackupLogic.MakeBackup(DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));

                ConnectIfNot();

                GroundhogContext.NetworkStorageLogic.Load();
                GroundhogContext.NetworkLanguageLogic.Load();

                GroundhogContext.Language = GroundhogContext.LoadLanguage(GroundhogContext.Settings.Language);

                RestartWindow();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, GroundhogContext.Language.ErrorsMessages.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MenuItemUpload_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ConnectIfNot();

                if (GroundhogContext.Settings.BackupSettings.AutoCloudBackup)
                    GroundhogContext.CloudBackupLogic.MakeBackup(DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));

                GroundhogContext.NetworkStorageLogic.Upload();
                GroundhogContext.NetworkLanguageLogic.Upload();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, GroundhogContext.Language.ErrorsMessages.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MenuItemBackups_Click(object sender, RoutedEventArgs e)
        {
            BackupsWindow backupsWindow = new BackupsWindow(this);
            backupsWindow.ShowDialog();
        }

        private void ConnectIfNot()
        {
            Func<string> f = () =>
            {
                CodeWindow window = new CodeWindow();
                if (window.ShowDialog() == true)
                    return window.Code;
                throw new Exception(GroundhogContext.Language.ErrorsMessages.CodeWasNotReceived);
            };

            if (!GroundhogContext.NetworkStorageLogic.IsConnected())
                GroundhogContext.NetworkStorageLogic.Connect(f);

            if (!GroundhogContext.NetworkLanguageLogic.IsConnected())
                GroundhogContext.NetworkLanguageLogic.Connect(f);
        }
    }
}
