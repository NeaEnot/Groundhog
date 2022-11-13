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
        private MainWindow windowContext;
        private Note selectedNote;
        private bool loaded = false;

        public SelectNotePage(MainWindow windowContext)
        {
            InitializeComponent();

            this.windowContext = windowContext;

            LoadNotes();
        }

        public void LoadNotes()
        {
            loaded = true;

            List<Note> notes =
                GroundhogContext.NoteLogic
                .Read()
                .OrderBy(req => req.Name)
                .ToList();

            //listBoxNotes.ItemsSource = null;
            listBoxNotes.ItemsSource = notes;

            loaded = false;

            if (selectedNote != null)
                listBoxNotes.SelectedItem = notes.First(req => req.Id == selectedNote.Id);
        }

        private void NoteSelected(object sender, SelectionChangedEventArgs e)
        {
            if (loaded)
                return;

            Note selected = (Note)e.AddedItems[0];

            if (selected != null)
                selectedNote = selected;

            windowContext.LoadNote(selectedNote);
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            NoteWindow window = new NoteWindow(null);
            if (window.ShowDialog() == true)
            {
                GroundhogContext.NoteLogic.Create(window.Note);
                LoadNotes();
            }
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

            if (note != null)
            {
                NoteWindow window = new NoteWindow(note);
                if (window.ShowDialog() == true)
                {
                    GroundhogContext.NoteLogic.Update(window.Note);

                    if (window.Note.Id == selectedNote.Id)
                    {
                        selectedNote = window.Note;
                        windowContext.LoadNote(selectedNote);
                    }

                    LoadNotes();
                }
            }
        }

        private void ContextMenuDelete_Click(object sender, RoutedEventArgs e)
        {
            Note note = (Note)listBoxNotes.SelectedItem;

            if (note != null)
            {
                GroundhogContext.NoteLogic.Delete(note.Id);

                LoadNotes();
            }
        }
    }
}
