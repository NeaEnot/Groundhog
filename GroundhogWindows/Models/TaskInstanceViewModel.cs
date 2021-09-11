using Core;
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
        public string TextColor { get { return Completed ? "Gray" : "Black"; } }
        public string TextDecorations { get { return Completed ? "Strikethrough" : "None"; } }
    }
}
