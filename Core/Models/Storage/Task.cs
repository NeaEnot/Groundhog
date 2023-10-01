using Core.Enums;

namespace Core.Models.Storage
{
    /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="Task"]/Task/*'/>
    public class Task
    {
        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="Task"]/Id/*'/>
        public string Id { get; set; }
        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="Task"]/Text/*'/>
        public string Text { get; set; }
        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="Task"]/RepeatMode/*'/>
        public RepeatMode RepeatMode { get; set; }
        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="Task"]/RepeatValue/*'/>
        public string RepeatValue { get; set; }
        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="Task"]/ToNextDay/*'/>
        public bool ToNextDay { get; set; }
        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="Task"]/OffsetAll/*'/>
        public bool OffsetAll { get; set; }
        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="Task"]/PlanningRange/*'/>
        public int PlanningRange { get; set; }
        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="Task"]/OptimizationRange/*'/>
        public int OptimizationRange { get; set; }

        public override int GetHashCode()
        {
            return (Id + Text + RepeatMode + RepeatValue + ToNextDay).GetHashCode();
        }
    }
}
