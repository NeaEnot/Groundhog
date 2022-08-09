using Core;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GroundhogMobile
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

        private void notesList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            //Navigation.PushAsync(new PurposesPage((PurposeGroup)e.Item));
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            Note note = new Note { Name = "!Новая заметка" };
            GroundhogContext.NoteLogic.Create(note);
            LoadData();

            //await Navigation.PushAsync(page);
        }

        public ICommand MenuItemDelete =>
            new Command<Note>((note) =>
            {
                GroundhogContext.NoteLogic.Delete(note.Id);
                LoadData();
            });
    }
}