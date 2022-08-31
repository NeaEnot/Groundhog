using Core.Enums;
using Core.Models;

namespace GroundhogMobile.Models
{
    internal class TaskViewModel
    {
        public string Id { get; set; }
        public string AccauntId { get; set; }
        public string Text { get; set; }
        public RepeatMode RepeatMode { get; set; }
        public string RepeatValue { get; set; }
        public bool ToNextDay { get; set; }
        public bool OffsetAll { get; set; }
        public int PlanningRange { get; set; }
        public int OptimizationRange { get; set; }
        public bool Repeated { get { return RepeatMode != RepeatMode.Нет; } }
        public bool RepeatedAndMoved { get { return Repeated && ToNextDay; } }

        internal TaskViewModel(Task task = null)
        {
            if (task != null)
            {
                Id = task.Id;
                Text = task.Text;
                RepeatMode = task.RepeatMode;
                RepeatValue = task.RepeatValue;
                ToNextDay = task.ToNextDay;
                OffsetAll = task.OffsetAll;
                PlanningRange = task.PlanningRange;
                OptimizationRange = task.OptimizationRange;
            }
        }

        internal Task Convert()
        {
            return new Task
            {
                Id = Id,
                Text = Text,
                RepeatMode = RepeatMode,
                RepeatValue = RepeatValue,
                ToNextDay = ToNextDay,
                OffsetAll = OffsetAll,
                PlanningRange = PlanningRange,
                OptimizationRange = OptimizationRange
            };
        }
    }
}
