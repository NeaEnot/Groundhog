using Core.Models.Storage;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GroundhogWindows.Models
{
    internal class NoteViewModel : INotifyPropertyChanged
    {
        private string name;

        public event PropertyChangedEventHandler PropertyChanged;

        public Note Source { get; private set; }

        public string Id => Source.Id;
        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }
        public string Text
        {
            get => Source.Text;
            set
            {
                Source.Text = value;
                OnPropertyChanged("Text");
            }
        }

        internal NoteViewModel(Note note)
        {
            Source = note;
            name = note.Name;
        }

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
