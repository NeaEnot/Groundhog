namespace Core.Models.Storage
{
    /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="Purpose"]/Purpose/*'/>
    public class Purpose : CommentedElemet
    {
        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="Purpose"]/Id/*'/>
        public string Id { get; set; }
        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="Purpose"]/GroupId/*'/>
        public string GroupId { get; set; }
        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="Purpose"]/Text/*'/>
        public string Text { get; set; }
        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="Purpose"]/Completed/*'/>
        public bool Completed { get; set; }

        public override int GetHashCode()
        {
            return (Id + GroupId + Text + Completed + Comment).GetHashCode();
        }
    }
}
