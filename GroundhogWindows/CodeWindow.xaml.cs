using System;
using System.Windows;

namespace GroundhogWindows
{
    public partial class CodeWindow : Window
    {
        public string Code { get; private set; }

        public CodeWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                    throw new Exception("Код не был введён.");

                Code = textBox.Text;

                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
