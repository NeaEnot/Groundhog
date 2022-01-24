using Core;
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
        public string Text
        {
            get
            {
                if (task == null)
                    task = GroundhogContext.TaskLogic.Read(TaskId);
                return task.Text;
            }
        }
        public bool Repeated
        {
            get
            {
                if (task == null)
                    task = GroundhogContext.TaskLogic.Read(TaskId);
                return task.RepeatMode != RepeatMode.Нет;
            }
        }
        public string TextColor { get { return Completed ? "Gray" : "White"; } }
        public TextDecorations TextDecorations { get { return Completed ? TextDecorations.Strikethrough : TextDecorations.None; } }

        internal TaskInstanceViewModel(TaskInstance instance = null, Task task = null)
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
    }
}
