using Core;
using Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GroundhogWindows
{
    public partial class SelectNotePage : Page
    {
        private MainWindow contextWindow;
        private bool loaded = false;

        internal Note SelectedNote { get; private set; }

        public SelectNotePage()
        {
            InitializeComponent();
        }

        private void LoadNotes()
        {
            loaded = true;

            List<Note> notes =
                GroundhogContext.NoteLogic
                .Read()
                .OrderBy(req => req.Name)
                .ToList();

            listBoxNotes.ItemsSource = null;
            listBoxNotes.ItemsSource = notes;

            loaded = false;
        }

        private void NoteSelected(object sender, SelectionChangedEventArgs e)
        {
            if (loaded)
                return;

            Note selected = (Note)((ListBox)e.Source).SelectedItem;

            if (selected != null)
            {
                SelectedNote = selected;
            }
            else
            {
                bool exist = false;

                foreach (Note note in listBoxNotes.ItemsSource)
                {
                    if (note.Id == SelectedNote.Id)
                    {
                        exist = true;
                        break;
                    }
                }

                if (!exist)
                    SelectedNote = new Note { Id = "" };
            }

            //contextWindow.LoadPurposes();
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            //PurposeGroupWindow window = new PurposeGroupWindow(null);
            //if (window.ShowDialog() == true)
            //{
            //    GroundhogContext.PurposeGroupLogic.Create(window.Group);
            //    LoadNotes();
            //}
        }

        private void listNotes_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            UpdateNote();
        }

        private void ContextMenuUpdate_Click(object sender, RoutedEventArgs e)
        {
            UpdateNote();
        }

        private void UpdateNote()
        {
            Note note = (Note)listBoxNotes.SelectedItem;

            //if (note != null)
            //{
            //    PurposeGroupWindow window = new PurposeGroupWindow(note);
            //    if (window.ShowDialog() == true)
            //    {
            //        GroundhogContext.PurposeGroupLogic.Update(window.Group);
            //        LoadNotes();
            //    }
            //}
        }

        private void ContextMenuDelete_Click(object sender, RoutedEventArgs e)
        {
            Note note = (Note)listBoxNotes.SelectedItem;

            if (note != null)
            {
                if (note.Id == SelectedNote.Id)
                    SelectedNote = new Note { Id = "" };

                GroundhogContext.NoteLogic.Delete(note.Id);

                LoadNotes();
                contextWindow.LoadPurposes();
            }
        }
    }
}
