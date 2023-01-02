using Core.Models.Storage;
using System;
using System.Collections.Generic;

namespace Core.Logic.DateTimeHelpers
{
    internal class NotHelper : IDTHelper
    {
        public void CheckIsValueCorrect(string text)
        { }

        public List<TaskInstance> FillRepeatedTasks(Task task)
        {
            return new List<TaskInstance>();
        }

        public DateTime GetDateForTask(Task task, DateTime selectedDate)
        {
            return selectedDate;
        }

        public int TaskRare(Task task)
        {
            return int.MaxValue;
        }
    }
}
