using Core.Enums;
using Core.Models.Storage;
using System;

namespace GroundhogDesktop.Models
{
    internal class TaskInstanceViewModel
    {
        private Task task;

        public string Id { get; set; }
        public DateTime Date { get; set; }
        public string TaskId { get; set; }
        public bool Completed { get; set; }
        public string Comment { get; set; }

        public virtual string Text => task.Text;
        public bool Repeated => task.RepeatMode != RepeatMode.None;

        public string TextColor => Completed ? App.Current.Resources["Additional text"].ToString() : App.Current.Resources["Main text"].ToString();
        public string TextDecorations => Completed ? "Strikethrough" : "None";

        internal TaskInstanceViewModel(TaskInstance instance, Task task)
        {
            if (instance != null)
            {
                Id = instance.Id;
                Date = instance.Date;
                TaskId = instance.TaskId;
                Completed = instance.Completed;
                Comment = instance.Comment;
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
                Completed = Completed,
                Comment = Comment
            };
        }
    }
}
