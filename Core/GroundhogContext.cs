using Core.Enums;
using Core.Interfaces;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public static class GroundhogContext
    {
        public static IAccauntLogic AccauntLogic { get; set; }
        public static ITaskInstanceLogic TaskInstanceLogic  { get; set; }
        public static ITaskLogic TaskLogic { get; set; }

        public static Accaunt Accaunt { get; set; }

        public static void FillRepeatedTasks()
        {
            List<Task> tasks = TaskLogic.Read(Accaunt);
            foreach (Task task in tasks)
            {
                if (task.RepeatMode != RepeatMode.Нет)
                {
                    List<TaskInstance> taskInstances = TaskInstanceLogic.Read(task.Id);
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

                        TaskInstanceLogic.Create(model);
                    }
                }
            }
        }
    }
}
