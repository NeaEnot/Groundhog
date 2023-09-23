using Core;
using Core.Models.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace GroundhogDesktop.Views.Tasks
{
    public partial class SelectTaskGroupPage : Page
    {
        private DateTime selectedDate;
        private string selectedTaskId;

        private MainWindow windowContext;

        public SelectTaskGroupPage(MainWindow windowContext)
        {
            InitializeComponent();

            this.windowContext = windowContext;
            selectedDate = DateTime.Now;

            LoadDates();
        }

        internal void LoadDates()
        {
            List<DateTime> dates = new List<DateTime>();

            for (int i = 0; i < 20; i++)
                dates.Add(DateTime.Now.AddDays(i));

            listBoxDates.ItemsSource = dates;
        }

        private bool selectionChanged = false;
        private void DateSelected(object sender, RoutedEventArgs e)
        {
            if (selectionChanged)
                return;

            selectionChanged = true;

            if (sender is ListBox)
            {
                selectedDate = (DateTime)((ListBox)sender).SelectedItem;
                calendar.SelectedDate = null;
            }
            if (sender is Calendar)
            {
                selectedDate = (DateTime)((Calendar)sender).SelectedDate;
                listBoxDates.SelectedIndex = -1;
            }

            windowContext.LoadTasks(selectedDate);

            selectionChanged = false;
        }

        private void listBoxFindedTasks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selected = ((Task)e.AddedItems[0]).Id;

            if (selected != null)
                selectedTaskId = selected;

            windowContext.LoadFindedTasks(selectedTaskId);
        }

        private void btnFind_Click(object sender, RoutedEventArgs e)
        {
            string find = tbFind.Text.ToLower();

            List<Task> tasks = GroundhogContext.TaskLogic.Read();

            if (!string.IsNullOrEmpty(find))
                tasks =
                    tasks
                    .Where(req => req.Text.ToLower().Contains(find))
                    .OrderBy(req => req.Text)
                    .ToList();

            listBoxFindedTasks.ItemsSource = tasks;
        }
    }
}
