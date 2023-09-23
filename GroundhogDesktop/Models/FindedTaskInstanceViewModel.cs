using Core.Enums;
using Core.Models.Storage;
using GroundhogDesktop.Converters;
using System;

namespace GroundhogDesktop.Models
{
    internal class FindedTaskInstanceViewModel
    {
        private static DateTimeConverter converter = new DateTimeConverter();

        private Task task;

        public string Id { get; set; }
        public DateTime Date { get; set; }
        public string TaskId { get; set; }
        public bool Completed { get; set; }

        public string Text => $"{converter.Convert(Date, null, "1", null)}, ({converter.Convert(Date, null, "0", null)})";
        public bool Repeated => task.RepeatMode != RepeatMode.None;

        public string TextColor => Completed ? App.Current.Resources["Additional text"].ToString() : App.Current.Resources["Main text"].ToString();
        public string TextDecorations => Completed ? "Strikethrough" : "None";

        internal FindedTaskInstanceViewModel(TaskInstance instance, Task task)
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
