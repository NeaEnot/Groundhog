﻿using Amporis.Xamarin.Forms.ColorPicker;
using Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GroundhogMobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ColorsPage : ContentPage
    {
        private Dictionary<Button, string> btns;
        private Dictionary<string, string> names;

        public ColorsPage()
        {
            InitializeComponent();

            Resources["Main color page"] = Xamarin.Forms.Color.FromHex(GroundhogContext.GetColor("Main color"));
            Resources["Additional color page"] = Xamarin.Forms.Color.FromHex(GroundhogContext.GetColor("Additional color"));
            Resources["Main text page"] = Xamarin.Forms.Color.FromHex(GroundhogContext.GetColor("Main text"));
            Resources["Additional text page"] = Xamarin.Forms.Color.FromHex(GroundhogContext.GetColor("Additional text"));
            Resources["Selected item page"] = Xamarin.Forms.Color.FromHex(GroundhogContext.GetColor("Selected item"));
            Resources["Select item page"] = Xamarin.Forms.Color.FromHex(GroundhogContext.GetColor("Select item"));

            btns = new Dictionary<Button, string>
            {
                { btnMainColor, "Main color" },
                { btnAdditionalColor, "Additional color" },
                { btnMainText, "Main text" },
                { btnAdditionalText, "Additional text" },
                { btnSelectedItem, "Selected item" },
                { btnSelectItem, "Select item" }
            };

            names = new Dictionary<string, string>
            {
                { "Main color", "Основной цвет" },
                { "Additional color", "Дополнительный цвет" },
                { "Main text", "Основной текст" },
                { "Additional text", "Дополнительный текст" },
                { "Selected item", "Выделенный элемент" },
                { "Select item", "Выбор элемента" }
            };
        }

        private async void ButtonColor_Clicked(object sender, EventArgs e)
        {
            string schemaColor = btns[sender as Button];

            Xamarin.Forms.Color currentColor = Xamarin.Forms.Color.FromHex(GroundhogContext.GetColor(schemaColor));
            ColorDialogSettings settings =
                new ColorDialogSettings
                {
                    EditAlfa = false,
                    OkButtonText = "Принять",
                    CancelButtonText = "Отмена",
                    BackgroundColor = Xamarin.Forms.Color.FromHex(GroundhogContext.GetColor("Main color")),
                    EditorsColor = Xamarin.Forms.Color.FromHex(GroundhogContext.GetColor("Main color")),
                    TextColor = Xamarin.Forms.Color.FromHex(GroundhogContext.GetColor("Main text")),
                    ARGBEditorsWidth = 50,
                    SliderWidth = 200
                };

            Xamarin.Forms.Color color = 
                await ColorPickerDialog.Show(stc, names[schemaColor], currentColor, settings);

            Resources[schemaColor + " page"] = color;
        }

        private void ButtonStandart_Clicked(object sender, EventArgs e)
        {
            Resources["Main color page"] = Xamarin.Forms.Color.FromHex("#ffffff");
            Resources["Additional color page"] = Xamarin.Forms.Color.FromHex("#f0f0f0");
            Resources["Main text page"] = Xamarin.Forms.Color.FromHex("#000000");
            Resources["Additional text page"] = Xamarin.Forms.Color.FromHex("#818282");
            Resources["Selected item page"] = Xamarin.Forms.Color.FromHex("#cbe8f6");
            Resources["Select item page"] = Xamarin.Forms.Color.FromHex("#e5f3fb");
        }

        private void ButtonSave_Clicked(object sender, EventArgs e)
        {
            Dictionary<string, string> colors = new Dictionary<string, string>
            {
                { "Main color", ((Xamarin.Forms.Color)Resources["Main color page"]).ToHex() },
                { "Additional color", ((Xamarin.Forms.Color)Resources["Additional color page"]).ToHex() },
                { "Main text", ((Xamarin.Forms.Color)Resources["Main text page"]).ToHex() },
                { "Additional text", ((Xamarin.Forms.Color)Resources["Additional text page"]).ToHex() },
                { "Selected item", ((Xamarin.Forms.Color)Resources["Selected item page"]).ToHex() },
                { "Select item", ((Xamarin.Forms.Color)Resources["Select item page"]).ToHex() },
            };

            GroundhogContext.SetColors(colors);
        }
    }
}