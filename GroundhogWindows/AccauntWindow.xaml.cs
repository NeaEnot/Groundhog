using Core.Models;
using System;
using System.Windows;

namespace GroundhogWindows
{
    public partial class AccauntWindow : Window
    {
        public Accaunt Accaunt { get; private set; }

        public AccauntWindow(Accaunt accaunt)
        {
            InitializeComponent();

            Accaunt = accaunt;

            if (Accaunt != null)
            {
                textBoxName.Text = accaunt.Name;
                textBoxConnection.Text = accaunt.ConnectionString;
            }
            else
            {
                Accaunt = new Accaunt();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textBoxName.Text) || string.IsNullOrWhiteSpace(textBoxConnection.Text))
                    throw new Exception("Поля должны быть заполнены.");

                Accaunt.Name = textBoxName.Text;
                Accaunt.ConnectionString = textBoxConnection.Text;

                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
