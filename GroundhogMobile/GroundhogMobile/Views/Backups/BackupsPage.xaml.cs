﻿using Core.Interfaces.Network;
using GroundhogMobile.Views.Services;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GroundhogMobile.Views.Backups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BackupsPage : ContentPage
    {
        IBackupLogic backupLogic;

        public BackupsPage(IBackupLogic backupLogic)
        {
            InitializeComponent();

            this.backupLogic = backupLogic;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            CreateBackupPage createBackupPage = new CreateBackupPage();
            Device.BeginInvokeOnMainThread(async () => await PopupNavigation.Instance.PushAsync(createBackupPage));
            string key = "";
            key = await createBackupPage.Key;

            if (!string.IsNullOrEmpty(key))
                backupLogic.MakeBackup(key);
        }

        private async void backupsList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            CommandPage page = new CommandPage((string)e.Item, new List<string> { "", "" });
            Device.BeginInvokeOnMainThread(async () => await PopupNavigation.Instance.PushAsync(page));

            object obj = await page.Result;
            string cmd = obj as string;

            if (cmd == null)
                return;
            if (cmd == "")
                backupLogic.RestoreBackup((string)e.Item);
            if (cmd == "")
                backupLogic.DeleteBackup((string)e.Item);
        }
    }
}