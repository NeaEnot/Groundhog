using Core;
using Core.Models.Storage;
using GroundhogDesktop.Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GroundhogDesktop.Views.Notes
{
    public partial class SelectNotePage : Page
    {
        private MainWindow windowContext;
        private NoteViewModel selectedNote;
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

            List<NoteViewModel> notes =
                GroundhogContext.NoteLogic
                .Read()
                .OrderBy(req => req.Name)
                .Select(req => new NoteViewModel(req))
                .ToList();

            listBoxNotes.ItemsSource = notes;

            loaded = false;

            if (selectedNote != null)
                listBoxNotes.SelectedItem = notes.First(req => req.Id == selectedNote.Id);
        }

        private void NoteSelected(object sender, SelectionChangedEventArgs e)
        {
            if (loaded)
                return;

            NoteViewModel selected = (NoteViewModel)e.AddedItems[0];

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
            NoteViewModel model = (NoteViewModel)listBoxNotes.SelectedItem;

            if (model != null)
            {
                NoteWindow window = new NoteWindow(model.Source);
                if (window.ShowDialog() == true)
                {
                    GroundhogContext.NoteLogic.Update(window.Note);

                    if (window.Note.Id == selectedNote.Id)
                    {
                        selectedNote = new NoteViewModel(window.Note);
                        windowContext.LoadNote(selectedNote);
                    }

                    LoadNotes();
                }
            }
        }

        private void ContextMenuDelete_Click(object sender, RoutedEventArgs e)
        {
            NoteViewModel model = (NoteViewModel)listBoxNotes.SelectedItem;

            if (model != null)
            {
                GroundhogContext.NoteLogic.Delete(model.Id);

                if (selectedNote.Id == model.Id)
                {
                    selectedNote = null;
                    windowContext.LoadNote(null);
                }

                LoadNotes();
            }
        }

        private void ContextMenuComment_Click(object sender, RoutedEventArgs e)
        {
            NoteViewModel viewModel = (NoteViewModel)listBoxNotes.SelectedItem;

            if (viewModel != null)
            {
                Note model = viewModel.Source;
                CommentWindow window = new CommentWindow(model);

                window.ShowDialog();

                if (window.IsChanged)
                    GroundhogContext.NoteLogic.Update(model);
            }
        }
    }
}
