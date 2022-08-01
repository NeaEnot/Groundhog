using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace GroundhogWindows
{
    public partial class NotePage : Page
    {
        private Stack<Label> labels;

        public NotePage()
        {
            InitializeComponent();
        }

        private void LoadText()
        {
            //tbNote.Text = windowContext.SelectedNote.Text;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            //windowContext.SelectedNote.Text = tbNote.Text;
            //GroundhogContext.NoteLogic.Update(windowContext.SelectedNote);
        }

        private void tbNote_TextChanged(object sender, TextChangedEventArgs e)
        {
            int lines = tbNote.Text.Split('\n').Length;

            while (labels.Count < lines)
            {
                Label label = new Label
                {
                    Content = labels.Count + 1,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Margin = new Thickness(0, 0, 4, 0)
                };

                labels.Push(label);
                spNumbers.Children.Add(label);
            }

            while (labels.Count > lines)
            {
                Label label = labels.Pop();
                spNumbers.Children.Remove(label);
            }
        }
    }
}
