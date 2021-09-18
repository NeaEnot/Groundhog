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

        public Task Task
        { 
            get
            {
                return new Task
                {
                    Id = Id,
                    AccauntId = AccauntId,
                    Text = Text,
                    RepeatMode = RepeatMode,
                    RepeatValue = RepeatValue,
                };
            }
        }

        public TaskViewModel(Task task = null)
        {
            if (task != null)
            {
                Id = task.Id;
                AccauntId = task.AccauntId;
                Text = task.Text;
                RepeatMode = task.RepeatMode;
                RepeatValue = task.RepeatValue;
            }
        }
    }
}
