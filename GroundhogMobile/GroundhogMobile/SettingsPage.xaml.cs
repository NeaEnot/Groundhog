using Core;
using Core.Models;
using Rg.Plugins.Popup.Services;
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

            //List<Accaunt> list = new List<Accaunt>();// GroundhogContext.AccauntLogic.Read();
            //accauntPicker.ItemsSource = list;

            //if (GroundhogContext.Accaunt != null)
            //{
            //    foreach (Accaunt acc in list)
            //    {
            //        if (acc.Id == GroundhogContext.Accaunt.Id)
            //        {
            //            accauntPicker.SelectedItem = acc;
            //            break;
            //        }
            //    }
            //}

            loading = false;
        }

        private void accauntPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Accaunt accaunt = accauntPicker.SelectedItem as Accaunt;
            //if (accaunt != null && !loading)
            //    GroundhogContext.Accaunt = accaunt;
        }

        private async void ButtonCreate_Clicked(object sender, EventArgs e)
        {
            
        }

        private async void ButtonUpdate_Clicked(object sender, EventArgs e)
        {
            
        }

        private void ButtonDelete_Clicked(object sender, EventArgs e)
        {
            
        }

        private async void ButtonLoad_Clicked(object sender, EventArgs e)
        {
            try
            {
                await System.Threading.Tasks.Task.Run(() =>
                {
                    ConnectIfNot();
                    GroundhogContext.NetworkLogic.Load();
                });
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка", ex.Message, "Ок");
            }
        }

        private async void ButtonSend_Clicked(object sender, EventArgs e)
        {
            try
            {
                await System.Threading.Tasks.Task.Run(() =>
                {
                    ConnectIfNot();
                    GroundhogContext.NetworkLogic.Upload();
                });
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка", ex.Message, "Ок");
            }
        }

        private async void ConnectIfNot()
        {
            if (!GroundhogContext.NetworkLogic.IsConnected())
            {
                Func<string> f = () =>
                {
                    CodePage page = new CodePage();
                    Device.BeginInvokeOnMainThread(async () => await PopupNavigation.Instance.PushAsync(page));
                    string code = "";
                    System.Threading.Tasks.Task.Run(async () => code = await page.Code).Wait();
                    return code;
                };

                GroundhogContext.NetworkLogic.Connect(f);
            }
        }
    }
}