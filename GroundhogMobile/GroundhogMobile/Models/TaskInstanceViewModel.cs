using Core.Enums;
using Core.Models;
using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace GroundhogMobile.Models
{
    internal class TaskInstanceViewModel : INotifyPropertyChanged
    {
        private Task task;
        private bool completed;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Id { get; set; }
        public DateTime Date { get; set; }
        public string TaskId { get; set; }
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

        public string Text => task.Text;
        public bool Repeated => task.RepeatMode != RepeatMode.Нет;

        public string TextColor => ((Color)(Completed ? App.Current.Resources["Additional text"] : App.Current.Resources["Main text"])).ToHex();
        public TextDecorations TextDecorations => Completed ? TextDecorations.Strikethrough : TextDecorations.None;

        internal TaskInstanceViewModel(TaskInstance instance, Task task)
        {
            if (instance != null)
            {
                Id = instance.Id;
                Date = instance.Date;
                TaskId = instance.TaskId;
                completed = instance.Completed;
            }

            this.task = task;
        }

        internal TaskInstance Convert()
        {
            return new TaskInstance
            {
                Id = Id,
                Date = Date,
                TaskId = TaskId,
                Completed = Completed
            };
        }
    }
}
