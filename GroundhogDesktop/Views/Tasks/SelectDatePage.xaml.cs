﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace WindowsDesktop.Views.Tasks
{
    public partial class SelectDatePage : Page
    {
        internal DateTime selectedDate;

        private MainWindow contextWindow;

        public SelectDatePage(MainWindow contextWindow)
        {
            InitializeComponent();

            this.contextWindow = contextWindow;
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

            contextWindow.LoadTasks(selectedDate);

            selectionChanged = false;
        }
    }
}