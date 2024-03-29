﻿using Core.Models.Storage;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Logic.DateTimeHelpers
{
    internal class DaysHelper : IDTHelper
    {
        public void CheckIsValueCorrect(string text)
        {
            int a;

            if (!int.TryParse(text, out a))
                throw new Exception($"{GroundhogContext.Language.ErrorsMessages.IncorrectValue}.");

            if (a < 1)
                throw new Exception($"{GroundhogContext.Language.ErrorsMessages.IncorrectNumberOfDays}.");
        }

        public List<TaskInstance> FillRepeatedTasks(Task task)
        {
            List<TaskInstance> models = new List<TaskInstance>();

            List<TaskInstance> taskInstances = GroundhogContext.TaskInstanceLogic.Read(task.Id);
            DateTime lastDate = taskInstances.Max(req => req.Date);
            DateTime currentDate = lastDate;

            while ((currentDate - DateTime.Now).TotalDays <= task.PlanningRange)
            {
                int days = int.Parse(task.RepeatValue);
                currentDate = currentDate.AddDays(days);

                TaskInstance model = new TaskInstance
                {
                    TaskId = task.Id,
                    Date = currentDate,
                    Completed = false
                };

                models.Add(model);
            }

            return models;
        }

        public DateTime GetDateForTask(Task task, DateTime selectedDate)
        {
            return selectedDate;
        }

        public int TaskRare(Task task)
        {
            return int.Parse(task.RepeatValue);
        }
    }
}
