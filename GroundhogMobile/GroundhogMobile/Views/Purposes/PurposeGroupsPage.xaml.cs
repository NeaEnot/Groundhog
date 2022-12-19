using Core;
using Core.Models.Storage;
using GroundhogMobile.Views.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GroundhogMobile.Views.Purposes
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PurposeGroupsPage : ContentPage
    {
        public PurposeGroupsPage()
        {
            InitializeComponent();

            Resources.Add("MenuItemUpdate", MenuItemUpdate);
            Resources.Add("MenuItemDelete", MenuItemDelete);

            SettingsPage.DownloadFinisfed += LoadData;

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

        private async void Button_Clicked(object sender, EventArgs e)
        {
            PurposeGroupPage page = new PurposeGroupPage(new PurposeGroup());
            page.Disappearing += (sender2, e2) =>
            {
                if (page.IsSuccess)
                {
                    PurposeGroup group = page.Group;
                    GroundhogContext.PurposeGroupLogic.Create(group);
                    LoadData();
                }
            };

            await Navigation.PushAsync(page);
        }

        public ICommand MenuItemUpdate =>
            new Command<PurposeGroup>(async (group) =>
            {
                PurposeGroupPage page = new PurposeGroupPage(group);

                page.Disappearing += (sender2, e2) =>
                {
                    if (page.IsSuccess)
                    {
                        GroundhogContext.PurposeGroupLogic.Update(page.Group);
                        LoadData();
                    }
                };

                await Navigation.PushAsync(page);
            });

        public ICommand MenuItemDelete =>
            new Command<PurposeGroup>((group) =>
            {
                GroundhogContext.PurposeGroupLogic.Delete(group.Id);
                LoadData();
            });
    }
}