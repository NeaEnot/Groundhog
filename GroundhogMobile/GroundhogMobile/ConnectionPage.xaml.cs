﻿using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GroundhogMobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConnectionPage : ContentPage
    {
        public ConnectionPage()
        {
            InitializeComponent();

            editor.Placeholder = GroundhogContext.NetworkLogic.ConnectionStringFormat;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!GroundhogContext.NetworkLogic.ConnectionStringExpr.IsMatch(editor.Text))
                    throw new Exception("Строка подключения не соответствует формату.");

                GroundhogContext.ConnectionString = editor.Text;

                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка", ex.Message, "Ок");
            }
        }
    }
}