using Core.Enums;

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
    }
}
