using Core.Interfaces;

namespace Core.Models
{
    public class Note : IHashable
    {
        public string Id { get; set; }
        public string Text { get; set; }

        public int GetHash()
        {
            return (Id + Text).GetHashCode();
        }
    }
}
