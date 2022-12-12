using Core;
using GroundhogWindows.Models;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GroundhogWindows.Views.Notes
{
    public partial class NotePage : Page
    {
        private NoteViewModel note;
        private Dictionary<string, NoteCell> buffer;

        private bool doundo = false;

        internal NotePage()
        {
            InitializeComponent();
            frNumbers.Content = new LineNumberingPage();

            buffer = new Dictionary<string, NoteCell>();

            CommandManager.AddPreviewCanExecuteHandler(
                    tbNote,
                    new CanExecuteRoutedEventHandler((sender, eventArgs) => {
                        if (eventArgs.Command == ApplicationCommands.Undo ||
                            eventArgs.Command == ApplicationCommands.Redo)
                        {
                            eventArgs.CanExecute = true;
                        }
                    }));
            CommandManager.AddPreviewExecutedHandler(
                tbNote,
                new ExecutedRoutedEventHandler((sender, eventArgs) => {
                    if (note != null)
                    {
                        if (eventArgs.Command == ApplicationCommands.Undo ||
                            eventArgs.Command == ApplicationCommands.Redo ||
                            eventArgs.Command == ApplicationCommands.Find)
                            eventArgs.Handled = true;
                        if (eventArgs.Command == ApplicationCommands.Undo)
                        {
                            if (buffer[note.Id].CanUndo)
                            {
                                doundo = true;

                                int currentCaretIndex = tbNote.CaretIndex;

                                int? caretIndex = buffer[note.Id].Undo();
                                tbNote.Text = buffer[note.Id].CurrentText;

                                tbNote.CaretIndex = caretIndex ?? currentCaretIndex;

                                doundo = false;
                            }

                            eventArgs.Handled = true;
                        }
                        else if (eventArgs.Command == ApplicationCommands.Redo)
                        {
                            if (buffer[note.Id].CanRedo)
                            {
                                doundo = true;

                                int currentCaretIndex = tbNote.CaretIndex;

                                int? caretIndex = buffer[note.Id].Redo();
                                tbNote.Text = buffer[note.Id].CurrentText;

                                tbNote.CaretIndex = caretIndex ?? currentCaretIndex;

                                doundo = false;
                            }
                        }
                        else if (eventArgs.Command == ApplicationCommands.Find)
                        {
                            tbFind.Focus();
                        }
                    }
                }));
        }

        internal void LoadText(NoteViewModel note)
        {
            if (this.note != null)
            {
                if (buffer[this.note.Id].IsSaved)
                    buffer.Remove(this.note.Id);
                else
                    buffer[this.note.Id].Position = tbNote.CaretIndex;
            }

            if (note != null)
            {
                tbNote.Focus();

                if (buffer.ContainsKey(note.Id))
                {
                    this.note = buffer[note.Id].Note;
                    tbNote.Text = buffer[note.Id].CurrentText;

                    tbNote.CaretIndex = buffer[this.note.Id].Position;
                }
                else
                {
                    buffer.Add(note.Id, new NoteCell(note));

                    this.note = note;
                    tbNote.Text = this.note.Text;

                    tbNote.CaretIndex = 0;
                }

                tbNote.IsEnabled = true;

                tbFind.IsEnabled = true;
                btnFind.IsEnabled = true;

                btnSave.IsEnabled = !buffer[note.Id].IsSaved;
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
            (frNumbers.Content as LineNumberingPage).GenerateNumbering(tbNote.Text.Split('\n').Length);

            if (!doundo)
                buffer[note.Id].CurrentText = tbNote.Text;

            btnSave.IsEnabled = !buffer[note.Id].IsSaved;
            note.Name = buffer[note.Id].IsSaved ? note.Source.Name : note.Source.Name + "*";
        }

        private void btnFind_Click(object sender, RoutedEventArgs e)
        {
            string find = tbFind.Text;
            string text = tbNote.Text;

            int index = tbNote.CaretIndex < text.Length - 1 ? text.IndexOf(find, tbNote.CaretIndex + 1) : -1;

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
