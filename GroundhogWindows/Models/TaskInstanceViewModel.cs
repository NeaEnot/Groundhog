using Core.Enums;
using Core.Models;
using System;

namespace GroundhogWindows.Models
{
    internal class TaskInstanceViewModel
    {
        private Task task;

        public string Id { get; set; }
        public DateTime Date { get; set; }
        public string TaskId { get; set; }
        public bool Completed { get; set; }

        public string Text => task.Text;
        public bool Repeated => task.RepeatMode != RepeatMode.Нет;

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
