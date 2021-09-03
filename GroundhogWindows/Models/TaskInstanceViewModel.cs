using System;

namespace GroundhogWindows.Models
{
    internal class TaskInstanceViewModel
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public string TaskId { get; set; }
        public bool Completed { get; set; }
        public string Text { get { return App.TaskLogic.Read(TaskId).Text; } }
        public string TextColor { get { return Completed ? "Gray" : "Black"; } }
        public string TextDecorations { get { return Completed ? "Strikethrough" : "None"; } }
    }
}
