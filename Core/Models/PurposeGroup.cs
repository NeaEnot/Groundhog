namespace Core.Models
{
    public class PurposeGroup
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public override int GetHashCode()
        {
            return (Id + Name).GetHashCode();
        }
    }
}
