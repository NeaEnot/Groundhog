using Core;
using System;
using System.Windows;

namespace GroundhogWindows
{
    public partial class ConnectionWindow : Window
    {
        public ConnectionWindow()
        {
            InitializeComponent();

            textBoxConnection.ToolTip = GroundhogContext.NetworkLogic.ConnectionStringFormat;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!GroundhogContext.NetworkLogic.ConnectionStringExpr.IsMatch(textBoxConnection.Text))
                    throw new Exception("Строка подключения не соответствует формату.");

                GroundhogContext.ConnectionString = textBoxConnection.Text;

                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
