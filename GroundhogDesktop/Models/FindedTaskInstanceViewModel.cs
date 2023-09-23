using Core.Models.Storage;
using GroundhogDesktop.Converters;

namespace GroundhogDesktop.Models
{
    internal class FindedTaskInstanceViewModel : TaskInstanceViewModel
    {
        private static DateTimeConverter converter = new DateTimeConverter();

        public override string Text => $"{converter.Convert(Date, null, "1", null)}, ({converter.Convert(Date, null, "0", null)})";

        internal FindedTaskInstanceViewModel(TaskInstance instance, Task task) : base(instance, task)
        { }
    }
}
