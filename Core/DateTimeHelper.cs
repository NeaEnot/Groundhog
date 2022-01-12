using Core.Enums;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public static class DateTimeHelper
    {
        public static void FillRepeatedTasks()
        {
            List<TaskInstance> models = new List<TaskInstance>();

            List<Task> tasks = GroundhogContext.TaskLogic.Read();
            foreach (Task task in tasks)
            {
                if (task.RepeatMode != RepeatMode.Нет)
                {
                    List<TaskInstance> taskInstances = GroundhogContext.TaskInstanceLogic.Read(task.Id);
                    DateTime lastDate = taskInstances.Max(req => req.Date);
                    DateTime currentDate = lastDate;

                    while ((currentDate - DateTime.Now).TotalDays <= 100)
                    {
                        currentDate = task.RepeatMode == RepeatMode.Дни ? currentDate.AddDays(task.RepeatValue) : currentDate.AddMonths(1);
                        if (task.RepeatMode == RepeatMode.ЧислоМесяца &&
                            task.RepeatValue > currentDate.Day &&
                            DateTime.DaysInMonth(currentDate.Year, currentDate.Month) > currentDate.Day)
                        {
                            currentDate = new DateTime(currentDate.Year, currentDate.Month, DateTime.DaysInMonth(currentDate.Year, currentDate.Month));
                        }

                        TaskInstance model = new TaskInstance
                        {
                            TaskId = task.Id,
                            Date = currentDate,
                            Completed = false
                        };

                        models.Add(model);
                    }
                }
            }

            GroundhogContext.TaskInstanceLogic.Create(models);
        }

        public static DateTime GetDateForTask(Task task, DateTime selectedDate)
        {
            DateTime date = selectedDate;

            if (task.RepeatMode == RepeatMode.ЧислоМесяца)
            {
                int days = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
                if (days < task.RepeatValue)
                    date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, days);
                else
                    date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, task.RepeatValue);

                if (date < DateTime.Now)
                    date = date.AddMonths(1);

                days = DateTime.DaysInMonth(DateTime.Now.Year, date.Month);
                if (days < task.RepeatValue)
                    date = new DateTime(DateTime.Now.Year, date.Month, days);
                else
                    date = new DateTime(DateTime.Now.Year, date.Month, task.RepeatValue);
            }

            return date;
        }
    }
}
