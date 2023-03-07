using Amporis.Xamarin.Forms.ColorPicker;
using Core;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GroundhogMobile.Views.Settings
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ColorsPage : ContentPage
    {
        private Dictionary<Button, string> btns;
        private Dictionary<string, string> names;

        public ColorsPage()
        {
            InitializeComponent();

            Resources["Main color page"] = Color.FromHex(GroundhogContext.Settings.ColorSchema.Colors["Main color"]);
            Resources["Additional color page"] = Color.FromHex(GroundhogContext.Settings.ColorSchema.Colors["Additional color"]);
            Resources["Main text page"] = Color.FromHex(GroundhogContext.Settings.ColorSchema.Colors["Main text"]);
            Resources["Additional text page"] = Color.FromHex(GroundhogContext.Settings.ColorSchema.Colors["Additional text"]);
            Resources["Selected item page"] = Color.FromHex(GroundhogContext.Settings.ColorSchema.Colors["Selected item"]);

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
                { "Main color", GroundhogContext.Language.Settings.MainColor },
                { "Additional color", GroundhogContext.Language.Settings.AditionalColor },
                { "Main text", GroundhogContext.Language.Settings.MainText },
                { "Additional text", GroundhogContext.Language.Settings.AditionalText },
                { "Selected item", GroundhogContext.Language.Settings.SelectedItem }
            };
        }

        private bool iscolorDialogOpen = false;

        private async void ButtonColor_Clicked(object sender, EventArgs e)
        {
            if (iscolorDialogOpen)
                return;

            string schemaColor = btns[sender as Button];

            Color currentColor = Color.FromHex(GroundhogContext.Settings.ColorSchema.Colors[schemaColor]);
            ColorDialogSettings settings =
                new ColorDialogSettings
                {
                    EditAlfa = false,
                    OkButtonText = "Принять",
                    CancelButtonText = "Отмена",
                    BackgroundColor = Color.FromHex(GroundhogContext.Settings.ColorSchema.Colors["Additional color"]),
                    EditorsColor = Color.FromHex(GroundhogContext.Settings.ColorSchema.Colors["Main color"]),
                    TextColor = Color.FromHex(GroundhogContext.Settings.ColorSchema.Colors["Main text"]),
                    DialogColor = Color.FromHex(GroundhogContext.Settings.ColorSchema.Colors["Main color"]),
                    ColorPreviewBorderColor = Color.FromHex(GroundhogContext.Settings.ColorSchema.Colors["Main color"]),
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

            foreach (string key in colors.Keys)
            {
                if (GroundhogContext.Settings.ColorSchema.Colors.ContainsKey(key))
                    GroundhogContext.Settings.ColorSchema.Colors[key] = colors[key];
                else
                    GroundhogContext.Settings.ColorSchema.Colors.Add(key, colors[key]);
            }

            GroundhogContext.SaveSettings();

            App.ApplyColorSchema();
        }
    }
}