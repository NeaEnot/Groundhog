using Core;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GroundhogDesktop.Views.Settings
{
    public partial class ColorsWindow : Window
    {
        private static Regex reg = new Regex(@"^#[0-9a-fA-F]{6}$");

        private TextBox tbCurrent;
        private bool colorChanged;

        public ColorsWindow()
        {
            InitializeComponent();

            colorChanged = true;

            tbMainColor.Text = GroundhogContext.Settings.ColorSchema.Colors["Main color"];
            tbAdditionalColor.Text = GroundhogContext.Settings.ColorSchema.Colors["Additional color"];
            tbMainText.Text = GroundhogContext.Settings.ColorSchema.Colors["Main text"];
            tbAdditionalText.Text = GroundhogContext.Settings.ColorSchema.Colors["Additional text"];
            tbSelectedItem.Text = GroundhogContext.Settings.ColorSchema.Colors["Selected item"];
            tbSelectedItemInactive.Text = GroundhogContext.Settings.ColorSchema.Colors["Selected item inactive"];
            tbSelectItem.Text = GroundhogContext.Settings.ColorSchema.Colors["Select item"];

            colorChanged = false;

            tbMainColor.Focus();
        }

        private void tb_GotFocus(object sender, RoutedEventArgs e)
        {
            tbCurrent = sender as TextBox;
            colorPicker.SelectedColor = (Color)ColorConverter.ConvertFromString(tbCurrent.Text);
        }

        private void tb_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (colorChanged)
                return;

            colorChanged = true;

            if (reg.IsMatch(tbCurrent.Text))
                colorPicker.SelectedColor = (Color)ColorConverter.ConvertFromString(tbCurrent.Text);

            colorChanged = false;
        }

        private void colorPicker_ColorChanged(object sender, RoutedEventArgs e)
        {
            if (colorChanged)
                return;

            colorChanged = true;

            if (tbCurrent != null)
                tbCurrent.Text = colorPicker.SelectedColor.ToString().Remove(1, 2);

            colorChanged = false;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<TextBox> tbs = new List<TextBox>()
                {
                    tbMainColor,
                    tbAdditionalColor,
                    tbMainText,
                    tbAdditionalText,
                    tbSelectedItem,
                    tbSelectedItemInactive,
                    tbSelectItem
                };

                foreach (TextBox tb in tbs)
                    if (!reg.IsMatch(tb.Text))
                        throw new Exception($"{GroundhogContext.Language.ErrorsMessages.StringNotMatchColorHexFormat}: {tb.Text}");

                Dictionary<string, string> colors = new Dictionary<string, string>()
                {
                    { "Main color", tbMainColor.Text.ToUpper() },
                    { "Additional color", tbAdditionalColor.Text.ToUpper() },
                    { "Main text", tbMainText.Text.ToUpper() },
                    { "Additional text", tbAdditionalText.Text.ToUpper() },
                    { "Selected item", tbSelectedItem.Text.ToUpper() },
                    { "Selected item inactive", tbSelectedItemInactive.Text.ToUpper() },
                    { "Select item", tbSelectItem.Text.ToUpper() },
                };

                foreach (string key in colors.Keys)
                {
                    if (GroundhogContext.Settings.ColorSchema.Colors.ContainsKey(key))
                        GroundhogContext.Settings.ColorSchema.Colors[key] = colors[key];
                    else
                        GroundhogContext.Settings.ColorSchema.Colors.Add(key, colors[key]);
                }

                GroundhogContext.SaveSettings();

                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, GroundhogContext.Language.ErrorsMessages.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonStandart_Click(object sender, RoutedEventArgs e)
        {
            tbMainColor.Text = "#ffffff";
            tbAdditionalColor.Text = "#f0f0f0";
            tbMainText.Text = "#000000";
            tbAdditionalText.Text = "#818282";
            tbSelectedItem.Text = "#cbe8f6";
            tbSelectedItemInactive.Text = "#f6f6f6";
            tbSelectItem.Text = "#e5f3fb";
        }
    }
}
