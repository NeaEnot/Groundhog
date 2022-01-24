using Core.Enums;

namespace Core.Models
{
    public class Task
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public RepeatMode RepeatMode { get; set; }
        public int RepeatValue { get; set; }
        public bool ToNextDay { get; set; }
    }
}
