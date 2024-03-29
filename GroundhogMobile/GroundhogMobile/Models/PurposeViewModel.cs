﻿using Core.Models.Storage;
using System.ComponentModel;
using Xamarin.Forms;

namespace GroundhogMobile.Models
{
    internal class PurposeViewModel : INotifyPropertyChanged
    {
        private bool completed;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Id { get; set; }
        public string GroupId { get; set; }
        public string Text { get; set; }
        public string Comment { get; set; }
        public bool Completed
        {
            get => completed;
            set
            {
                completed = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Completed"));
                PropertyChanged(this, new PropertyChangedEventArgs("TextColor"));
                PropertyChanged(this, new PropertyChangedEventArgs("TextDecorations"));
            }
        }

        public string TextColor => ((Color)(Completed ? App.Current.Resources["Additional text"] : App.Current.Resources["Main text"])).ToHex();
        public TextDecorations TextDecorations => Completed ? TextDecorations.Strikethrough : TextDecorations.None;

        internal PurposeViewModel(Purpose purpose = null)
        {
            if (purpose != null)
            {
                Id = purpose.Id;
                GroupId = purpose.GroupId;
                Text = purpose.Text;
                completed = purpose.Completed;
                Comment = purpose.Comment;
            }
        }

        internal Purpose Convert()
        {
            return new Purpose
            {
                Id = Id,
                GroupId = GroupId,
                Text = Text,
                Completed = Completed,
                Comment = Comment
            };
        }
    }
}
