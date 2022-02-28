using Core.Models;
using System;
using System.Collections.Generic;

namespace Core.DateTimeHelpers
{
    internal interface IDTHelper
    {
        List<TaskInstance> FillRepeatedTasks(Task task);
        DateTime GetDateForTask(Task task, DateTime selectedDate);
        void CheckIsValueCorrect(string text);
        int TaskRare(Task task);
    }
}
