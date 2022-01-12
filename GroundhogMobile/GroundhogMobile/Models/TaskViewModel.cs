using Core.Enums;
using Core.Models;

namespace GroundhogMobile.Models
{
    internal class TaskViewModel
    {
        public string Id { get; set; }
        public string AccauntId { get; set; }
        public string Text { get; set; }
        public RepeatMode RepeatMode { get; set; }
        public int RepeatValue { get; set; }
        public bool Repeated { get { return RepeatMode != RepeatMode.Нет; } }

        internal Task Task
        { 
            get
            {
                return new Task
                {
                    Id = Id,
                    Text = Text,
                    RepeatMode = RepeatMode,
                    RepeatValue = RepeatValue,
                };
            }
        }

        internal TaskViewModel(Task task = null)
        {
            if (task != null)
            {
                Id = task.Id;
                Text = task.Text;
                RepeatMode = task.RepeatMode;
                RepeatValue = task.RepeatValue;
            }
        }
    }
}
