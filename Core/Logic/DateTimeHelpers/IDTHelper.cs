using Core.Models.Storage;
using System;
using System.Collections.Generic;

namespace Core.Logic.DateTimeHelpers
{
    internal interface IDTHelper
    {
        List<TaskInstance> FillRepeatedTasks(Task task, DateTime startDate);
        DateTime GetDateForTask(Task task, DateTime selectedDate, DateTime nowDate);
        void CheckIsValueCorrect(string text);
        int TaskRare(Task task);
    }
}
