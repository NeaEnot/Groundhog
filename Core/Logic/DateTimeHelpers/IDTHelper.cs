﻿using Core.Models.Storage;
using System;
using System.Collections.Generic;

namespace Core.Logic.DateTimeHelpers
{
    internal interface IDTHelper
    {
        List<TaskInstance> FillRepeatedTasks(Task task);
        DateTime GetDateForTask(Task task, DateTime selectedDate);
        void CheckIsValueCorrect(string text);
        int TaskRare(Task task);
    }
}
