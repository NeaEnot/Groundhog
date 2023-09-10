using Core;
using System;
using System.Windows;

namespace WindowsDesktop.Views.Backups
{
    public partial class CreateBackupWindow : Window
    {
        public string Key { get; private set; }

        public CreateBackupWindow()
        {
            InitializeComponent();
            textBox.Focus();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                    throw new Exception($"{GroundhogContext.Language.ErrorsMessages.FieldMustBeFilled}.");

                Key = textBox.Text;

                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, GroundhogContext.Language.ErrorsMessages.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
