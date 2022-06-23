using Amporis.Xamarin.Forms.ColorPicker;
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

        private ColorConverter colorConverter;

        public ColorsPage()
        {
            InitializeComponent();

            Resources["Main color page"] = GroundhogContext.GetColor("Main color");
            Resources["Additional color page"] = GroundhogContext.GetColor("Additional color");
            Resources["Main text page"] = GroundhogContext.GetColor("Main text");
            Resources["Additional text page"] = GroundhogContext.GetColor("Additional text");
            Resources["Selected item page"] = GroundhogContext.GetColor("Selected item");
            Resources["Select item page"] = GroundhogContext.GetColor("Select item");

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

            colorConverter = new ColorConverter();
        }

        private async void ButtonColor_Clicked(object sender, EventArgs e)
        {
            string schemaColor = btns[sender as Button];

            Xamarin.Forms.Color color = 
                await ColorPickerDialog.Show(
                    stc,
                    names[schemaColor],
                    (Xamarin.Forms.Color)colorConverter.ConvertFromString(GroundhogContext.GetColor(schemaColor)),
                    null);

            Resources[schemaColor + " page"] = color.ToHex();
        }

        private void ButtonStandart_Clicked(object sender, EventArgs e)
        {
            Resources["Main color page"] = "#ffffff";
            Resources["Additional color page"] = "#f0f0f0";
            Resources["Main text page"] = "#000000";
            Resources["Additional text page"] = "#818282";
            Resources["Selected item page"] = "#cbe8f6";
            Resources["Select item page"] = "#e5f3fb";
        }

        private void ButtonSave_Clicked(object sender, EventArgs e)
        {
            Dictionary<string, string> colors = new Dictionary<string, string>
            {
                { "Main color", Resources["Main color page"].ToString() },
                { "Additional color", Resources["Additional color page"].ToString() },
                { "Main text", Resources["Main text page"].ToString() },
                { "Additional text", Resources["Additional text page"].ToString() },
                { "Selected item", Resources["Selected item page"].ToString() },
                { "Select item", Resources["Select item page"].ToString() },
            };
        }
    }
}