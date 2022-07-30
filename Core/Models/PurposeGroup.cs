using Core.Interfaces;

namespace Core.Models
{
    public class PurposeGroup : IHashable
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public int GetHash()
        {
            return (Id + Name).GetHashCode();
        }
    }
}
