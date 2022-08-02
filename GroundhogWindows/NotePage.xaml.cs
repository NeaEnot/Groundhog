using Core;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace GroundhogWindows
{
    public partial class NotePage : Page
    {
        private MainWindow windowContext;
        private Stack<Label> labels;

        public NotePage(MainWindow windowContext)
        {
            InitializeComponent();

            this.windowContext = windowContext;
        }

        public void LoadText()
        {
            if (windowContext.SelectedNote != null)
            {
                tbNote.Text = windowContext.SelectedNote.Text;
                tbNote.IsEnabled = true;
                btnSave.IsEnabled = true;
            }
            else
            {
                tbNote.Text = "";
                tbNote.IsEnabled = false;
                btnSave.IsEnabled = false;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            windowContext.SelectedNote.Text = tbNote.Text;
            GroundhogContext.NoteLogic.Update(windowContext.SelectedNote);
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
