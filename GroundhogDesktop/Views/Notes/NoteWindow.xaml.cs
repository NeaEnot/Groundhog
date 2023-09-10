using Core;
using Core.Models.Storage;
using System;
using System.Windows;

namespace GroundhogDesktop.Views.Notes
{
    public partial class NoteWindow : Window
    {
        public Note Note { get; private set; }

        public NoteWindow(Note note)
        {
            InitializeComponent();

            if (note != null)
            {
                Note = note;
                textBoxName.Text = note.Name;
            }
            else
            {
                Note = new Note { Text = "" };
            }

            textBoxName.Focus();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textBoxName.Text))
                    throw new Exception($"{GroundhogContext.Language.ErrorsMessages.FieldMustBeFilled}.");

                Note.Name = textBoxName.Text;

                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, GroundhogContext.Language.ErrorsMessages.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
