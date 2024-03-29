﻿using Core;
using Core.Models.Storage;
using GroundhogDesktop.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace GroundhogDesktop.Views.Purposes
{
    public partial class SelectGroupPage : Page
    {
        private MainWindow windowContext;
        private bool loaded = false;
        private string selectedGroupId;

        public SelectGroupPage(MainWindow windowContext)
        {
            InitializeComponent();

            this.windowContext = windowContext;

            LoadGroups();
        }

        internal void LoadGroups()
        {
            loaded = true;

            List<PurposeGroup> groups =
                GroundhogContext.PurposeGroupLogic
                .Read()
                .OrderBy(req => req.Name)
                .ToList();

            listBoxGroups.ItemsSource = groups;

            loaded = false;

            if (selectedGroupId != null)
                listBoxGroups.SelectedItem = groups.First(req => req.Id == selectedGroupId);
        }

        private void GroupSelected(object sender, SelectionChangedEventArgs e)
        {
            if (loaded)
                return;

            PurposeGroup selected = (PurposeGroup)e.AddedItems[0];
            selectedGroupId = selected.Id;
            windowContext.LoadPurposes(selectedGroupId);
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            PurposeGroupWindow window = new PurposeGroupWindow(null);
            if (window.ShowDialog() == true)
            {
                GroundhogContext.PurposeGroupLogic.Create(window.Group);
                LoadGroups();
            }
        }

        private void listBoxGroups_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            UpdateGroup();
        }

        private void ContextMenuUpdate_Click(object sender, RoutedEventArgs e)
        {
            UpdateGroup();
        }

        private void UpdateGroup()
        {
            PurposeGroup group = (PurposeGroup)listBoxGroups.SelectedItem;

            if (group != null)
            {
                PurposeGroupWindow window = new PurposeGroupWindow(group);
                if (window.ShowDialog() == true)
                {
                    GroundhogContext.PurposeGroupLogic.Update(window.Group);
                    LoadGroups();
                }
            }
        }

        private void ContextMenuDelete_Click(object sender, RoutedEventArgs e)
        {
            PurposeGroup group = (PurposeGroup)listBoxGroups.SelectedItem;

            if (group != null)
            {
                GroundhogContext.PurposeGroupLogic.Delete(group.Id);

                List<string> purposesIds =
                    GroundhogContext.PurposeLogic
                    .Read(group.Id)
                    .Select(req => req.Id)
                    .ToList();

                GroundhogContext.PurposeLogic.Delete(purposesIds);

                if (group.Id == selectedGroupId)
                {
                    windowContext.LoadPurposes("");
                    selectedGroupId = null;
                }

                LoadGroups();
            }
        }

        private void ContextMenuComment_Click(object sender, RoutedEventArgs e)
        {
            PurposeGroup group = (PurposeGroup)listBoxGroups.SelectedItem;

            if (group != null)
            {
                CommentWindow window = new CommentWindow(group);

                window.ShowDialog();

                if (window.IsChanged)
                {
                    GroundhogContext.PurposeGroupLogic.Update(group);
                    LoadGroups();
                }
            }
        }
    }
}
