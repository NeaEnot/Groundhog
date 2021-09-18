using Core;
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

        private bool loading;

        private void LoadData()
        {
            loading = true;

            List<Accaunt> list = GroundhogContext.AccauntLogic.Read();
            accauntPicker.ItemsSource = list;

            if (GroundhogContext.Accaunt != null)
            {
                foreach (Accaunt acc in list)
                {
                    if (acc.Id == GroundhogContext.Accaunt.Id)
                    {
                        accauntPicker.SelectedItem = acc;
                        break;
                    }
                }
            }

            loading = false;
        }

        private void accauntPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            Accaunt accaunt = accauntPicker.SelectedItem as Accaunt;
            if (accaunt != null && !loading)
                GroundhogContext.Accaunt = accaunt;
        }

        private async void ButtonCreate_Clicked(object sender, EventArgs e)
        {
            Accaunt accaunt = new Accaunt();

            AccauntPage page = new AccauntPage(accaunt);
            page.Disappearing += (sender2, e2) => 
            {
                if (page.IsSuccess)
                {
                    GroundhogContext.AccauntLogic.Create(page.Model);
                    LoadData();
                }
            };

            await Navigation.PushAsync(page);
        }

        private async void ButtonUpdate_Clicked(object sender, EventArgs e)
        {
            Accaunt accaunt = accauntPicker.SelectedItem as Accaunt;

            AccauntPage page = new AccauntPage(accaunt);
            page.Disappearing += (sender2, e2) => 
            { 
                if (page.IsSuccess)
                {
                    GroundhogContext.AccauntLogic.Update(page.Model);
                    LoadData();
                }
            };

            await Navigation.PushAsync(page);
        }

        private void ButtonDelete_Clicked(object sender, EventArgs e)
        {
            Accaunt accaunt = accauntPicker.SelectedItem as Accaunt;
            if (accaunt != null)
            {
                GroundhogContext.AccauntLogic.Delete(accaunt.Id);
                LoadData();
            }
        }
    }
}