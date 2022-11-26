using Core;
using Core.Models;
using GroundhogMobile.Views.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GroundhogMobile.Views.Notes
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotesPage : ContentPage
    {
        public NotesPage()
        {
            InitializeComponent();

            Resources.Add("MenuItemDelete", MenuItemDelete);

            SettingsPage.DownloadFinisfed += LoadData;

            LoadData();
        }

        private void LoadData()
        {
            List<Note> notes =
                GroundhogContext.NoteLogic
                .Read()
                .OrderBy(req => req.Name)
                .ToList();

            notesList.ItemsSource = null;
            notesList.ItemsSource = notes;
        }

        private async void notesList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            NotePage page = new NotePage((Note)e.Item);
            page.Disappearing += (sender2, e2) => SaveNote(page);
            await Navigation.PushAsync(page);
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            Note note = new Note { Name = "!Новая заметка" };
            GroundhogContext.NoteLogic.Create(note);
            LoadData();

            NotePage page = new NotePage(note);
            page.Disappearing += (sender2, e2) => SaveNote(page);
            await Navigation.PushAsync(page);
        }

        public ICommand MenuItemDelete =>
            new Command<Note>((note) =>
            {
                GroundhogContext.NoteLogic.Delete(note.Id);
                LoadData();
            });

        private void SaveNote(NotePage page)
        {
            if (page.IsSuccess)
            {
                GroundhogContext.NoteLogic.Update(page.Note);
                LoadData();
            }
        }
    }
}