using Core;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GroundhogWindows
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

            tbMainColor.Text = GroundhogContext.GetColor("Main color");
            tbAdditionalColor.Text = GroundhogContext.GetColor("Additional color");
            tbMainText.Text = GroundhogContext.GetColor("Main text");
            tbAdditionalText.Text = GroundhogContext.GetColor("Additional text");
            tbSelectedItem.Text = GroundhogContext.GetColor("Selected item");
            tbSelectedItemInactive.Text = GroundhogContext.GetColor("Selected item inactive");
            tbSelectItem.Text = GroundhogContext.GetColor("Select item");

            colorChanged = false;

            tbMainColor.Focus();
        }

        private void tb_GotFocus(object sender, RoutedEventArgs e)
        {
            tbCurrent = sender as TextBox;
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

        private void Button_Click(object sender, RoutedEventArgs e)
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
                        throw new Exception($"Строка {tb.Text} не соответствует формату ColorHex.");

                if (!reg.IsMatch(tbMainColor.Text) ||
                    !reg.IsMatch(tbAdditionalColor.Text) ||
                    !reg.IsMatch(tbMainText.Text) ||
                    !reg.IsMatch(tbAdditionalText.Text) ||
                    !reg.IsMatch(tbSelectedItem.Text) ||
                    !reg.IsMatch(tbSelectedItemInactive.Text) ||
                    !reg.IsMatch(tbSelectItem.Text))
                {
                    throw new Exception("Один из аргументов не соответствует формату ColorHex.");
                }

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

                GroundhogContext.SetColors(colors);

                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
