using Core.Models.Storage;
using System;
using System.Windows;

namespace GroundhogWindows.Views.Purposes
{
    public partial class PurposeWindow : Window
    {
        public Purpose Purpose { get; private set; }

        public PurposeWindow(Purpose purpose)
        {
            InitializeComponent();

            if (purpose != null)
            {
                Purpose = purpose;
                textBox.Text = purpose.Text;
            }
            else
            {
                Purpose = new Purpose();
            }

            textBox.Focus();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                    throw new Exception("Поле должно быть заполнено.");

                Purpose.Text = textBox.Text;

                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
