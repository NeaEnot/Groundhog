namespace Core.Models.Storage
{
    public class Note : CommentedElemet
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }

        public override int GetHashCode()
        {
            return (Id + Name + Text + Comment).GetHashCode();
        }
    }
}
