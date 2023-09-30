namespace Core.Models.Storage
{
    public abstract class CommentedElemet
    {
        private string comment;
        public string Comment
        {
            get => string.IsNullOrEmpty(comment) ? null : comment;
            set => comment = value;
        }
    }
}
