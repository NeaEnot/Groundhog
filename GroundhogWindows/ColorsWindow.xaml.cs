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
        private static Regex reg = new Regex(@"^#[0-9a-fA-F]$");

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
            tbSelectItem.Text = GroundhogContext.GetColor("Selected item");

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
                tbCurrent.Text = colorPicker.Color.ToString();

            colorChanged = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
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
                    { "Main color", tbMainColor.Text },
                    { "Additional color", tbAdditionalColor.Text },
                    { "Main text", tbMainText.Text },
                    { "Additional text", tbAdditionalText.Text },
                    { "Selected item", tbSelectedItem.Text },
                    { "Selected item inactive", tbSelectedItemInactive.Text },
                    { "Selected item", tbSelectItem.Text },
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
