using Core.Models.Storage;
using System;
using System.Windows;

namespace GroundhogWindows.Views.Notes
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
                    throw new Exception("Поле должно быть заполнено.");

                Note.Name = textBoxName.Text;

                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
