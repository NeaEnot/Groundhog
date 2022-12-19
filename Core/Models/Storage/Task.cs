using Core.Enums;

namespace Core.Models.Storage
{
    public class Task
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public RepeatMode RepeatMode { get; set; }
        public string RepeatValue { get; set; }
        public bool ToNextDay { get; set; }
        public bool OffsetAll { get; set; }
        public int PlanningRange { get; set; }
        public int OptimizationRange { get; set; }

        public override int GetHashCode()
        {
            return (Id + Text + RepeatMode + RepeatValue + ToNextDay).GetHashCode();
        }
    }
}
