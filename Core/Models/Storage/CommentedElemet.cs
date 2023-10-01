namespace Core.Models.Storage
{
    /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="CommentedElemet"]/CommentedElemet/*'/>
    public abstract class CommentedElemet
    {
        private string comment;
        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="CommentedElemet"]/Comment/*'/>
        public string Comment
        {
            get => string.IsNullOrEmpty(comment) ? null : comment;
            set => comment = value;
        }
    }
}
