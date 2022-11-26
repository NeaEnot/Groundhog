using Core;
using GroundhogWindows.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Threading;

namespace GroundhogWindows
{
    public partial class NotePage : Page
    {
        private NoteViewModel note;
        private Stack<Label> labels;
        private Dictionary<string, NoteCell> buffer;

        private bool doundo = false;

        internal NotePage()
        {
            InitializeComponent();

            labels = new Stack<Label>();
            buffer = new Dictionary<string, NoteCell>();

            Action<TextBox> redirectUndoRedo = new Action<TextBox>((element) =>
            {
                CommandManager.AddPreviewCanExecuteHandler(
                    element,
                    new CanExecuteRoutedEventHandler((sender, eventArgs) => {
                        if (eventArgs.Command == ApplicationCommands.Undo ||
                            eventArgs.Command == ApplicationCommands.Redo)
                        {
                            eventArgs.CanExecute = true;
                        }
                    }));
                CommandManager.AddPreviewExecutedHandler(
                    element,
                    new ExecutedRoutedEventHandler((sender, eventArgs) => {
                        if (note != null)
                        {
                            doundo = true;

                            if (eventArgs.Command == ApplicationCommands.Undo ||
                                eventArgs.Command == ApplicationCommands.Redo)
                                eventArgs.Handled = true;
                            if (eventArgs.Command == ApplicationCommands.Undo)
                            {
                                if (buffer[note.Id].CanUndo)
                                {
                                    int carretIndex = element.CaretIndex;

                                    buffer[note.Id].Redo();
                                    tbNote.Text = buffer[note.Id].CurrentText;

                                    element.CaretIndex = carretIndex;
                                }

                                eventArgs.Handled = true;
                            }
                            else if (eventArgs.Command == ApplicationCommands.Redo)
                            {
                                if (buffer[note.Id].CanDo)
                                {
                                    int carretIndex = element.CaretIndex;

                                    buffer[note.Id].Do();
                                    tbNote.Text = buffer[note.Id].CurrentText;

                                    element.CaretIndex = carretIndex;
                                }
                            }

                            doundo = false;
                        }
                    }));
            });

            redirectUndoRedo(tbNote);
        }

        internal void LoadText(NoteViewModel note)
        {
            if (this.note != null)
            {
                if (buffer[this.note.Id].IsSaved)
                    buffer.Remove(this.note.Id);
                else
                    buffer[this.note.Id].Position = svText.VerticalOffset;
            }

            if (note != null)
            {
                if (buffer.ContainsKey(note.Id))
                {
                    this.note = buffer[note.Id].Note;
                    tbNote.Text = buffer[note.Id].CurrentText;

                    svText.ScrollToVerticalOffset(buffer[note.Id].Position);
                }
                else
                {
                    buffer.Add(note.Id, new NoteCell(note));

                    this.note = note;
                    tbNote.Text = this.note.Text;

                    svText.ScrollToHome();
                }

                tbNote.IsEnabled = true;

                tbFind.IsEnabled = true;
                btnFind.IsEnabled = true;

                btnSave.IsEnabled = tbNote.Text != buffer[note.Id].Note.Text;
            }
            else
            {
                tbNote.Text = "";
                tbNote.IsEnabled = false;

                tbFind.IsEnabled = false;
                btnFind.IsEnabled = false;

                btnSave.IsEnabled = false;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            note.Text = tbNote.Text;
            GroundhogContext.NoteLogic.Update(note.Source);

            note.Name = note.Source.Name;
            btnSave.IsEnabled = false;

            buffer[note.Id].Save();
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

            if (!doundo)
                buffer[note.Id].CurrentText = tbNote.Text;

            btnSave.IsEnabled = !buffer[note.Id].IsSaved;
            note.Name = buffer[note.Id].IsSaved ? note.Source.Name : note.Source.Name + "*";
        }

        private void btnFind_Click(object sender, RoutedEventArgs e)
        {
            string find = tbFind.Text;
            string text = tbNote.Text;

            int index = text.IndexOf(find, tbNote.CaretIndex + 1);

            if (index == -1)
                index = text.IndexOf(find);

            if (index != -1)
            {
                tbNote.Focus();
                tbNote.CaretIndex = index;
                tbNote.Select(index, find.Length);
            }
            else
            {
                MessageBox.Show("Указанный текст не найден");
            }
        }
    }
}
