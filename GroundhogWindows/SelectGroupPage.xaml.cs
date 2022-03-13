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
            SelectedGroup = (PurposeGroup)e.Source;
            contextWindow.LoadPurposes();
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
