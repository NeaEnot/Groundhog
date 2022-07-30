namespace Core.Models
{
    public class Note
    {
        public string Id { get; set; }
        public string Text { get; set; }

        public override int GetHashCode()
        {
            return (Id + Text).GetHashCode();
        }
    }
}
