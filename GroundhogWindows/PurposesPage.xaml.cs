using Core;
using Core.Models;
using GroundhogWindows.Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GroundhogWindows
{
    public partial class PurposesPage : Page
    {
        private MainWindow windowContext;

        public PurposesPage(MainWindow windowContext)
        {
            InitializeComponent();

            this.windowContext = windowContext;
        }

        internal void LoadPurposes()
        {
            List<PurposeViewModel> purposes =
                GroundhogContext.PurposeLogic
                .Read(windowContext.SelectedGroupId)
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

            LoadPurposes();
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            Purpose model = new Purpose
            {
                GroupId = windowContext.SelectedGroupId,
                Text = "",
                Completed = false
            };

            PurposeWindow window = new PurposeWindow(model);

            if (window.ShowDialog() == true)
            {
                GroundhogContext.PurposeLogic.Create(window.Purpose);
                LoadPurposes();
            }
        }

        private void ContextMenuUpdate_Click(object sender, RoutedEventArgs e)
        {
            UpdatePurpose();
        }

        private void UpdatePurpose()
        {
            PurposeViewModel viewModel = (PurposeViewModel)listBoxPurposes.SelectedItem;
            Purpose model = viewModel.Convert();

            PurposeWindow window = new PurposeWindow(model);

            if (window.ShowDialog() == true)
            {
                GroundhogContext.PurposeLogic.Update(window.Purpose);
                LoadPurposes();
            }
        }

        private void ContextMenuDelete_Click(object sender, RoutedEventArgs e)
        {
            PurposeViewModel viewModel = (PurposeViewModel)listBoxPurposes.SelectedItem;

            if (viewModel != null)
            {
                GroundhogContext.PurposeLogic.Delete(viewModel.Id);
                LoadPurposes();
            }
        }
    }
}
