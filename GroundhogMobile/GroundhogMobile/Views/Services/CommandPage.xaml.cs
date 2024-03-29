﻿using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GroundhogMobile.Views.Services
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CommandPage : PopupPage
    {
        internal Task<object> Result { get => tcs.Task; }
        private TaskCompletionSource<object> tcs;

        public CommandPage(string title, IEnumerable objects)
        {
            InitializeComponent();
            tcs = new TaskCompletionSource<object>();

            lblTitle.Text = title;
            list.ItemsSource = objects;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            tcs.SetResult(null);
            await PopupNavigation.Instance.PopAsync();
        }

        private async void list_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            tcs.SetResult(e.Item);
            await PopupNavigation.Instance.PopAsync();
        }
    }
}