using System;

namespace Core.Models.Storage
{
    /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="TaskInstance"]/TaskInstance/*'/>
    public class TaskInstance : CommentedElemet
    {
        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="TaskInstance"]/Id/*'/>
        public string Id { get; set; }
        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="TaskInstance"]/TaskId/*'/>
        public string TaskId { get; set; }
        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="TaskInstance"]/Date/*'/>
        public DateTime Date { get; set; }
        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="TaskInstance"]/Completed/*'/>
        public bool Completed { get; set; }

        public override int GetHashCode()
        {
            return (Id + TaskId + Date + Completed + Comment).GetHashCode();
        }
    }
}
