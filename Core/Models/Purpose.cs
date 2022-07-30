using Core.Interfaces;

namespace Core.Models
{
    public class Purpose : IHashable
    {
        public string Id { get; set; }
        public string GroupId { get; set; }
        public string Text { get; set; }
        public bool Completed { get; set; }

        public int GetHash()
        {
            return (Id + GroupId + Text + Completed).GetHashCode();
        }
    }
}
