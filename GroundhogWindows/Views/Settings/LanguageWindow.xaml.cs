using Core;
using System.Windows;

namespace GroundhogWindows.Views.Settings
{
    public partial class LanguageWindow : Window
    {
        public LanguageWindow()
        {
            InitializeComponent();

            cbLanguage.ItemsSource = GroundhogContext.Languages;
            cbLanguage.SelectedItem = GroundhogContext.Settings.Language;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string lang = cbLanguage.SelectedItem as string;
            GroundhogContext.Language = GroundhogContext.LoadLanguage(lang);
            GroundhogContext.SaveSettings();
            DialogResult = true;
        }
    }
}
