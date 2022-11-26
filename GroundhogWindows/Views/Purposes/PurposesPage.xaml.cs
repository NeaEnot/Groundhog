using Core;
using Core.Models;
using GroundhogWindows.Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace GroundhogWindows.Views.Purposes
{
    public partial class PurposesPage : Page
    {
        private MainWindow windowContext;
        private string groupId;

        public PurposesPage(MainWindow windowContext)
        {
            InitializeComponent();

            this.windowContext = windowContext;
        }

        internal void LoadPurposes(string groupId)
        {
            this.groupId = groupId;

            List<PurposeViewModel> purposes =
                GroundhogContext.PurposeLogic
                .Read(groupId)
                .OrderBy(req => req.Text)
                .Select(req => new PurposeViewModel(req))
                .ToList();

            listBoxPurposes.ItemsSource = null;
            listBoxPurposes.ItemsSource = purposes;

            btnCreate.IsEnabled = true;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            PurposeViewModel viewModel = (PurposeViewModel)((CheckBox)sender).DataContext;
            Purpose model = viewModel.Convert();

            GroundhogContext.PurposeLogic.Update(model);

            LoadPurposes(groupId);
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            Purpose model = new Purpose
            {
                GroupId = groupId,
                Text = "",
                Completed = false
            };

            PurposeWindow window = new PurposeWindow(model);

            if (window.ShowDialog() == true)
            {
                GroundhogContext.PurposeLogic.Create(window.Purpose);
                LoadPurposes(groupId);
            }
        }

        private void ContextMenuClone_Click(object sender, RoutedEventArgs e)
        {
            PurposeViewModel viewModel = (PurposeViewModel)listBoxPurposes.SelectedItem;
            Purpose model = viewModel.Convert();

            Purpose clone = new Purpose
            {
                Id = null,
                GroupId = model.GroupId,
                Text = model.Text,
                Completed = false
            };

            GroundhogContext.PurposeLogic.Create(clone);
            LoadPurposes(groupId);
        }

        private void ContextMenuUpdate_Click(object sender, RoutedEventArgs e)
        {
            PurposeViewModel viewModel = (PurposeViewModel)listBoxPurposes.SelectedItem;
            Purpose model = viewModel.Convert();

            PurposeWindow window = new PurposeWindow(model);

            if (window.ShowDialog() == true)
            {
                GroundhogContext.PurposeLogic.Update(window.Purpose);
                LoadPurposes(groupId);
            }
        }

        private void ContextMenuDelete_Click(object sender, RoutedEventArgs e)
        {
            PurposeViewModel viewModel = (PurposeViewModel)listBoxPurposes.SelectedItem;

            if (viewModel != null)
            {
                GroundhogContext.PurposeLogic.Delete(viewModel.Id);
                LoadPurposes(groupId);
            }
        }
    }
}
