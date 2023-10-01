namespace Core.Models.Storage
{
    /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="PurposeGroup"]/PurposeGroup/*'/>
    public class PurposeGroup : CommentedElemet
    {
        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="PurposeGroup"]/Id/*'/>
        public string Id { get; set; }
        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="PurposeGroup"]/Name/*'/>
        public string Name { get; set; }

        public override int GetHashCode()
        {
            return (Id + Name + Comment).GetHashCode();
        }
    }
}
