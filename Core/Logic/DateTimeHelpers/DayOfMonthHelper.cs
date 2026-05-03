using Core.Models.Storage;
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

        public List<TaskInstance> FillRepeatedTasks(Task task, DateTime startDate)
        {
            List<TaskInstance> models = new List<TaskInstance>();

            List<TaskInstance> taskInstances = GroundhogContext.TaskInstanceLogic.Read(task.Id);
            DateTime lastDate = taskInstances.Max(req => req.Date);
            DateTime currentDate = lastDate;
            int day = int.Parse(task.RepeatValue);

            while ((currentDate - startDate).TotalDays <= task.PlanningRange)
            {
                currentDate = currentDate.AddMonths(1);

                if (day > currentDate.Day && DateTime.DaysInMonth(currentDate.Year, currentDate.Month) >= day)
                    currentDate = new DateTime(currentDate.Year, currentDate.Month, day);

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

        public DateTime GetDateForTask(Task task, DateTime selectedDate, DateTime nowDate)
        {
            DateTime date = selectedDate;
            int value = int.Parse(task.RepeatValue);

            int days = DateTime.DaysInMonth(nowDate.Year, nowDate.Month);
            if (days < value)
                date = new DateTime(nowDate.Year, nowDate.Month, days);
            else
                date = new DateTime(nowDate.Year, nowDate.Month, value);

            if (date < nowDate.Date)
                date = date.AddMonths(1);

            days = DateTime.DaysInMonth(nowDate.Year, date.Month);
            if (days < value)
                date = new DateTime(nowDate.Year, date.Month, days);
            else
                date = new DateTime(nowDate.Year, date.Month, value);

            return date;
        }

        public int TaskRare(Task task)
        {
            return 31;
        }
    }
}
