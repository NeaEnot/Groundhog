using Core;
using Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace GroundhogWindows
{
    public partial class SelectGroupPage : Page
    {
        private MainWindow contextWindow;

        internal PurposeGroup SelectedGroup { get; private set; }

        public SelectGroupPage(MainWindow contextWindow)
        {
            InitializeComponent();

            this.contextWindow = contextWindow;

            LoadGroups();
        }

        internal void LoadGroups()
        {
            List<PurposeGroup> groups =
                GroundhogContext.PurposeGroupLogic
                .Read()
                .OrderBy(req => req.Name)
                .ToList();

            listBoxGroups.ItemsSource = null;
            listBoxGroups.ItemsSource = groups;
        }

        private void GroupSelected(object sender, SelectionChangedEventArgs e)
        {
            SelectedGroup = (PurposeGroup)((ListBox)e.Source).SelectedItem;
            contextWindow.LoadPurposes();
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
            UpdateTask();
        }

        private void ContextMenuUpdate_Click(object sender, RoutedEventArgs e)
        {
            UpdateTask();
        }

        private void UpdateTask()
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

                LoadGroups();
            }
        }
    }
}
