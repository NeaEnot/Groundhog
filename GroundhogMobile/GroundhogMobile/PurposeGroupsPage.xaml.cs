using Core;
using Core.Models;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GroundhogMobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PurposeGroupsPage : ContentPage
    {
        public PurposeGroupsPage()
        {
            InitializeComponent();

            LoadData();
        }

        private void LoadData()
        {
            List<PurposeGroup> groups =
                GroundhogContext.PurposeGroupLogic
                .Read()
                .OrderBy(req => req.Name)
                .ToList();

            groupsList.ItemsSource = null;
            groupsList.ItemsSource = groups;
        }

        private void groupsList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Navigation.PushAsync(new PurposesPage((PurposeGroup)e.Item));
        }
    }
}