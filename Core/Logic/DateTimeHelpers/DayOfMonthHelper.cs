﻿using Core.Models.Storage;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Logic.DateTimeHelpers
{
    class DayOfMonthHelper : IDTHelper
    {
        public void CheckIsValueCorrect(string text)
        {
            int b;

            if (!int.TryParse(text, out b))
                throw new Exception($"{GroundhogContext.Language.ErrorsMessages.IncorrectValue}.");

            if (b < 1 || b > 31)
                throw new Exception($"{GroundhogContext.Language.ErrorsMessages.IncorrectNumberOfDay}.");
        }

        public List<TaskInstance> FillRepeatedTasks(Task task)
        {
            List<TaskInstance> models = new List<TaskInstance>();

            List<TaskInstance> taskInstances = GroundhogContext.TaskInstanceLogic.Read(task.Id);
            DateTime lastDate = taskInstances.Max(req => req.Date);
            DateTime currentDate = lastDate;

            while ((currentDate - DateTime.Now).TotalDays <= task.PlanningRange)
            {
                int day = int.Parse(task.RepeatValue);
                currentDate = currentDate.AddMonths(1);

                if (day > currentDate.Day && DateTime.DaysInMonth(currentDate.Year, currentDate.Month) > currentDate.Day)
                    currentDate = new DateTime(currentDate.Year, currentDate.Month, DateTime.DaysInMonth(currentDate.Year, currentDate.Month));

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
            DateTime date = selectedDate;
            int value = int.Parse(task.RepeatValue);

            int days = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            if (days < value)
                date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, days);
            else
                date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, value);

            if (date < DateTime.Now.Date)
                date = date.AddMonths(1);

            days = DateTime.DaysInMonth(DateTime.Now.Year, date.Month);
            if (days < value)
                date = new DateTime(DateTime.Now.Year, date.Month, days);
            else
                date = new DateTime(DateTime.Now.Year, date.Month, value);

            return date;
        }

        public int TaskRare(Task task)
        {
            return 31;
        }
    }
}
