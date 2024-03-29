﻿using Core;
using Core.Models.Storage;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GroundhogMobile.Views.Purposes
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PurposeGroupPage : ContentPage
    {
        public bool IsSuccess { get; private set; } = false;
        internal PurposeGroup Group { get; private set; }

        internal PurposeGroupPage(PurposeGroup group)
        {
            if (group == null)
                throw new ArgumentNullException("When creating must pass new object with empty fields.");

            InitializeComponent();

            Group = group;
            BindingContext = group;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textEntryGroup.Text))
                    throw new Exception($"{GroundhogContext.Language.ErrorsMessages.FieldMustBeFilled}.");

                Group.Name = textEntryGroup.Text;
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