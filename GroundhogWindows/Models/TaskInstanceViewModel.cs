using Core;
using Core.Enums;
using System;

namespace GroundhogWindows.Models
{
    internal class TaskInstanceViewModel
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public string TaskId { get; set; }
        public bool Completed { get; set; }
        public string Text { get { return GroundhogContext.TaskLogic.Read(TaskId).Text; } }
        public bool Repeated { get { return GroundhogContext.TaskLogic.Read(TaskId).RepeatMode != RepeatMode.Нет; } }
        public string TextColor { get { return Completed ? "Gray" : "Black"; } }
        public string TextDecorations { get { return Completed ? "Strikethrough" : "None"; } }
    }
}
