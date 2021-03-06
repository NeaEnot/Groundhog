using Amporis.Xamarin.Forms.ColorPicker;
using Core;
using System;
using System.Collections.Generic;
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

            Resources["Main color page"] = Color.FromHex(GroundhogContext.GetColor("Main color"));
            Resources["Additional color page"] = Color.FromHex(GroundhogContext.GetColor("Additional color"));
            Resources["Main text page"] = Color.FromHex(GroundhogContext.GetColor("Main text"));
            Resources["Additional text page"] = Color.FromHex(GroundhogContext.GetColor("Additional text"));
            Resources["Selected item page"] = Color.FromHex(GroundhogContext.GetColor("Selected item"));

            btns = new Dictionary<Button, string>
            {
                { btnMainColor, "Main color" },
                { btnAdditionalColor, "Additional color" },
                { btnMainText, "Main text" },
                { btnAdditionalText, "Additional text" },
                { btnSelectedItem, "Selected item" }
            };

            names = new Dictionary<string, string>
            {
                { "Main color", "Основной цвет" },
                { "Additional color", "Дополнительный цвет" },
                { "Main text", "Основной текст" },
                { "Additional text", "Дополнительный текст" },
                { "Selected item", "Выделенный элемент" }
            };
        }

        private bool iscolorDialogOpen = false;

        private async void ButtonColor_Clicked(object sender, EventArgs e)
        {
            if (iscolorDialogOpen)
                return;

            string schemaColor = btns[sender as Button];

            Color currentColor = Color.FromHex(GroundhogContext.GetColor(schemaColor));
            ColorDialogSettings settings =
                new ColorDialogSettings
                {
                    EditAlfa = false,
                    OkButtonText = "Принять",
                    CancelButtonText = "Отмена",
                    BackgroundColor = Color.FromHex(GroundhogContext.GetColor("Additional color")),
                    EditorsColor = Color.FromHex(GroundhogContext.GetColor("Main color")),
                    TextColor = Color.FromHex(GroundhogContext.GetColor("Main text")),
                    DialogColor = Color.FromHex(GroundhogContext.GetColor("Main color")),
                    ColorPreviewBorderColor = Color.FromHex(GroundhogContext.GetColor("Main color")),
                    ARGBEditorsWidth = 50,
                    SliderWidth = 200
                };

            iscolorDialogOpen = true;
            Color color = await ColorPickerDialog.Show(stc, names[schemaColor], currentColor, settings);
            iscolorDialogOpen = false;

            Resources[schemaColor + " page"] = color;
        }

        private void ButtonStandart_Clicked(object sender, EventArgs e)
        {
            Resources["Main color page"] = Color.FromHex("#ffffff");
            Resources["Additional color page"] = Color.FromHex("#f0f0f0");
            Resources["Main text page"] = Color.FromHex("#000000");
            Resources["Additional text page"] = Color.FromHex("#818282");
            Resources["Selected item page"] = Color.FromHex("#cbe8f6");
        }

        private void ButtonSave_Clicked(object sender, EventArgs e)
        {
            Dictionary<string, string> colors = new Dictionary<string, string>
            {
                { "Main color", ((Color)Resources["Main color page"]).ToHex() },
                { "Additional color", ((Color)Resources["Additional color page"]).ToHex() },
                { "Main text", ((Color)Resources["Main text page"]).ToHex() },
                { "Additional text", ((Color)Resources["Additional text page"]).ToHex() },
                { "Selected item", ((Color)Resources["Selected item page"]).ToHex() }
            };

            GroundhogContext.SetColors(colors);

            App.LoadResources();
        }
    }
}