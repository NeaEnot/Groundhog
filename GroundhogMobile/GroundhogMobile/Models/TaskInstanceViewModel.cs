using Core.Enums;
using Core.Models;
using System;
using Xamarin.Forms;

namespace GroundhogMobile.Models
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

        public string TextColor => Completed ? "Gray" : "White";
        public TextDecorations TextDecorations => Completed ? TextDecorations.Strikethrough : TextDecorations.None;

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
