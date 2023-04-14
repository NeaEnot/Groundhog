using Core;
using System;
using System.Windows;

namespace GroundhogWindows.Views.Settings
{
    public partial class ConnectionWindow : Window
    {
        public ConnectionWindow()
        {
            InitializeComponent();

            textBoxConnection.Text = GroundhogContext.Settings.ConnectionString;
            textBoxConnection.ToolTip = GroundhogContext.NetworkStorageLogic.ConnectionStringFormat;

            textBoxConnection.Focus();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!GroundhogContext.NetworkStorageLogic.ConnectionStringExpr.IsMatch(textBoxConnection.Text))
                    throw new Exception($"{GroundhogContext.Language.ErrorsMessages.ConnectionStringNotMatchFormat}.");

                GroundhogContext.Settings.ConnectionString = textBoxConnection.Text;
                GroundhogContext.SaveSettings();

                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, GroundhogContext.Language.ErrorsMessages.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
