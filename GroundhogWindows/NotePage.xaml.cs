using Core;
using GroundhogWindows.Models;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace GroundhogWindows
{
    public partial class NotePage : Page
    {
        private NoteViewModel note;
        private Stack<Label> labels;

        private string originalText = "";

        internal NotePage()
        {
            InitializeComponent();

            labels = new Stack<Label>();
        }

        internal void LoadText(NoteViewModel note)
        {
            this.note = note;
            originalText = note.Text;

            if (note != null)
            {
                tbNote.Text = note.Text;
                tbNote.IsEnabled = true;
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
            note.Text = tbNote.Text;
            GroundhogContext.NoteLogic.Update(note.Source);

            note.Name = note.Source.Name;
            btnSave.IsEnabled = false;
            originalText = note.Text;
        }

        private void tbNote_TextChanged(object sender, TextChangedEventArgs e)
        {
            System.Threading.Tasks.Task.Run(() =>
            {
                Dispatcher.BeginInvoke(DispatcherPriority.Input, new ThreadStart(() =>
                {
                    int n = 0;
                    for (int i = tbNote.Text.Split('\n').Length; i > 0; i /= 10)
                        n++;

                    numbersColumn.Width = new GridLength(n > 1 ? n * 8 : 16);

                    while (labels.Count < tbNote.Text.Split('\n').Length)
                    {
                        Label label = new Label
                        {
                            Content = labels.Count + 1,
                            HorizontalAlignment = HorizontalAlignment.Right,
                            Padding = new Thickness(0, 0.66335, 4, 0),
                            FontSize = 11.5
                        };

                        labels.Push(label);
                        spNumbers.Children.Add(label);
                    }

                    while (labels.Count > tbNote.Text.Split('\n').Length)
                    {
                        Label label = labels.Pop();
                        spNumbers.Children.Remove(label);
                    }
                }));
            });

            btnSave.IsEnabled = tbNote.Text != originalText;
            note.Name = tbNote.Text == originalText ? note.Source.Name : note.Source.Name + "*";
        }
    }
}
