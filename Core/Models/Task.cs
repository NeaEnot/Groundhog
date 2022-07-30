using Core.Enums;
using Core.Interfaces;

namespace Core.Models
{
    public class Task : IHashable
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public RepeatMode RepeatMode { get; set; }
        public string RepeatValue { get; set; }
        public bool ToNextDay { get; set; }

        public  int GetHash()
        {
            return (Id + Text + RepeatMode + RepeatValue + ToNextDay).GetHashCode();
        }
    }
}
