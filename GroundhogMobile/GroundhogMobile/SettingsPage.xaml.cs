using Core.Models;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GroundhogMobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();

            LoadData();
        }

        private void LoadData()
        {
            List<Accaunt> list = new List<Accaunt>();

            for (int i = 0; i < 5; i++)
            {
                list.Add(new Accaunt
                {
                    Id = i.ToString(),
                    Name = $"Аккаунт {i}",
                    ConnectionString = $"Аккаунт {i}"
                });
            }

            accauntPicker.ItemsSource = list;
        }

        private void accauntPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            Accaunt accaunt = accauntPicker.SelectedItem as Accaunt;
            // TODO
        }

        private async void ButtonCreate_Clicked(object sender, EventArgs e)
        {
            Accaunt accaunt = new Accaunt();

            AccauntPage page = new AccauntPage(accaunt);
            page.Disappearing += (sender2, e2) => { if (page.IsSuccess) LoadData(); };

            await Navigation.PushAsync(page);
        }

        private async void ButtonUpdate_Clicked(object sender, EventArgs e)
        {
            Accaunt accaunt = accauntPicker.SelectedItem as Accaunt;

            AccauntPage page = new AccauntPage(accaunt);
            page.Disappearing += (sender2, e2) => { if (page.IsSuccess) LoadData(); };

            await Navigation.PushAsync(page);
        }

        private void ButtonDelete_Clicked(object sender, EventArgs e)
        {

        }
    }
}