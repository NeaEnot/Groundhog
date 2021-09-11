using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GroundhogMobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DatesListPage : ContentPage
    {
        public DatesListPage()
        {
            InitializeComponent();

            LoadDates();
        }

        private void LoadDates()
        {
            List<DateTime> dates = new List<DateTime>();
            for (int i = 0; i < 20; i++)
            {
                dates.Add(DateTime.Now.AddDays(i));
            }
            datesList.ItemsSource = dates;
        }

        private void datesList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Navigation.PushAsync(new TasksPage((DateTime)e.Item));
        }
    }
}