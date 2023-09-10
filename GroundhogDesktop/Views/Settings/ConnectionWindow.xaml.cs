using Core;
using System;
using System.Windows;

namespace GroundhogDesktop.Views.Settings
{
    public partial class ConnectionWindow : Window
    {
        public ConnectionWindow()
        {
            InitializeComponent();

            textBoxConnectionStorage.Text = GroundhogContext.Settings.ConnectionStringStorage;
            textBoxConnectionStorage.ToolTip = GroundhogContext.NetworkStorageLogic.ConnectionStringFormat;

            textBoxConnectionLanguage.Text = GroundhogContext.Settings.ConnectionStringLanguage;
            textBoxConnectionLanguage.ToolTip = GroundhogContext.NetworkLanguageLogic.ConnectionStringFormat;

            textBoxConnectionStorage.Focus();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!GroundhogContext.NetworkStorageLogic.ConnectionStringExpr.IsMatch(textBoxConnectionStorage.Text))
                    throw new Exception($"{GroundhogContext.Language.ErrorsMessages.ConnectionStringNotMatchFormat}.");
                if (!GroundhogContext.NetworkLanguageLogic.ConnectionStringExpr.IsMatch(textBoxConnectionLanguage.Text))
                    throw new Exception($"{GroundhogContext.Language.ErrorsMessages.ConnectionStringNotMatchFormat}.");

                GroundhogContext.Settings.ConnectionStringStorage = textBoxConnectionStorage.Text;
                GroundhogContext.Settings.ConnectionStringLanguage = textBoxConnectionLanguage.Text;
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
