namespace Core.Models.Storage
{
    /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="Note"]/Note/*'/>
    public class Note : CommentedElemet
    {
        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="Note"]/Id/*'/>
        public string Id { get; set; }
        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="Note"]/Name/*'/>
        public string Name { get; set; }
        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="Note"]/Text/*'/>
        public string Text { get; set; }

        public override int GetHashCode()
        {
            return (Id + Name + Text + Comment).GetHashCode();
        }
    }
}
