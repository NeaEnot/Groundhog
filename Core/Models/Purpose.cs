namespace Core.Models
{
    public class Purpose
    {
        public string Id { get; set; }
        public string GroupId { get; set; }
        public string Text { get; set; }
        public bool Completed { get; set; }

        public override int GetHashCode()
        {
            return (Id + GroupId + Text + Completed).GetHashCode();
        }
    }
}
