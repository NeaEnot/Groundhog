using Core;
using Core.Models.Storage;
using System;
using System.Windows;

namespace GroundhogDesktop.Views.Purposes
{
    public partial class PurposeGroupWindow : Window
    {
        public PurposeGroup Group { get; private set; }

        public PurposeGroupWindow(PurposeGroup group)
        {
            InitializeComponent();

            if (group != null)
            {
                Group = group;
                textBoxName.Text = group.Name;
            }
            else
            {
                Group = new PurposeGroup();
            }

            textBoxName.Focus();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textBoxName.Text))
                    throw new Exception($"{GroundhogContext.Language.ErrorsMessages.FieldMustBeFilled}.");

                Group.Name = textBoxName.Text;

                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, GroundhogContext.Language.ErrorsMessages.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
