using Core;
using System;
using System.Windows;

namespace GroundhogWindows.Views.Settings
{
    public partial class CodeWindow : Window
    {
        public string Code { get; private set; }

        public CodeWindow()
        {
            InitializeComponent();

            textBox.Focus();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                    throw new Exception($"{GroundhogContext.Language.ErrorsMessages.CodeWasNotEntered}.");

                Code = textBox.Text;

                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, GroundhogContext.Language.ErrorsMessages.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
