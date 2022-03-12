using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace GroundhogWindows
{
    public partial class SelectDatePage : Page
    {
        internal DateTime SelectedDate { get; private set; }

        private MainWindow contextWindow;

        public SelectDatePage(MainWindow contextWindow)
        {
            InitializeComponent();

            this.contextWindow = contextWindow;

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
                SelectedDate = (DateTime)((ListBox)sender).SelectedItem;
            }
            if (sender is Calendar)
            {
                SelectedDate = (DateTime)((Calendar)sender).SelectedDate;
            }

            contextWindow.LoadTasks();
        }
    }
}
