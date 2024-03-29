﻿using Core;
using Core.Models.Storage;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GroundhogMobile.Views.Notes
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotePage : ContentPage
    {
        public bool IsSuccess { get; private set; } = false;
        internal Note Note { get; private set; }

        public NotePage(Note note)
        {
            if (note == null)
                throw new ArgumentNullException("When creating must pass new object with empty fields.");

            InitializeComponent();

            Note = note;
            BindingContext = note;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nameEntry.Text))
                {
                    throw new Exception($"{GroundhogContext.Language.ErrorsMessages.FieldMustBeFilled}.");
                }

                Note.Name = nameEntry.Text;
                Note.Text = textEditor.Text;
                IsSuccess = true;

                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert(GroundhogContext.Language.ErrorsMessages.Error, ex.Message, "Ок");
            }
        }
    }
}