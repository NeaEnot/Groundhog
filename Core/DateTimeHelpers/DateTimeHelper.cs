using Core.Enums;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.DateTimeHelpers
{
    public static class DateTimeHelper
    {
        private static Dictionary<RepeatMode, IDTHelper> helpers = new Dictionary<RepeatMode, IDTHelper>()
        {
            { RepeatMode.Нет, new NotHelper() },
            { RepeatMode.Дни, new DaysHelper() },
            { RepeatMode.ЧислоМесяца, new DayOfMounthHelper() },
            { RepeatMode.ДеньГода, new DayOfYearHelper() },
            { RepeatMode.ДниНедели, new DaysOfWeekHelper() }
        };

        public static void FillRepeatedTasks()
        {
            List<TaskInstance> models = new List<TaskInstance>();

            List<Task> tasks = GroundhogContext.TaskLogic.Read();
            foreach (Task task in tasks.Where(t => t.RepeatMode != RepeatMode.Нет))
                models.AddRange(helpers[task.RepeatMode].FillRepeatedTasks(task));

            GroundhogContext.TaskInstanceLogic.Create(models);
        }

        public static void DeleteOldTasks()
        {
            List<TaskInstance> models = new List<TaskInstance>();
            List<Task> tasksToDelete = new List<Task>();

            List<Task> tasks = GroundhogContext.TaskLogic.Read();
            foreach (Task task in tasks)
            {
                List<TaskInstance> instances = GroundhogContext.TaskInstanceLogic.Read(task.Id).Where(req => (DateTime.Now - req.Date).Days >= 100).ToList();
                models.AddRange(instances);

                if (task.RepeatMode == RepeatMode.Нет && instances.Count == 1)
                    tasksToDelete.Add(task);
            }

            GroundhogContext.TaskInstanceLogic.Delete(models.Select(req => req.Id).ToList());
            foreach (Task task in tasksToDelete)
                GroundhogContext.TaskLogic.Delete(task.Id);
        }

        public static DateTime GetDateForTask(Task task, DateTime selectedDate)
        {
            return helpers[task.RepeatMode].GetDateForTask(task, selectedDate);
        }

        public static void ToDay(DateTime day)
        {
            List<Task> tasks = GroundhogContext.TaskLogic.Read();

            foreach (Task task in tasks)
            {
                List<TaskInstance> taskInstances = 
                    GroundhogContext.TaskInstanceLogic.Read(task.Id).Where(req => req.Date.Date < day.Date && !req.Completed).ToList();

                foreach (TaskInstance instance in taskInstances)
                {
                    if (task.ToNextDay)
                        instance.Date = day.Date;
                    else
                        instance.Completed = true;

                    GroundhogContext.TaskInstanceLogic.Update(instance);
                }
            }
        }

        public static int TaskRare(Task task)
        {
            return helpers[task.RepeatMode].TaskRare(task);
        }


        public static void CheckIsValueCorrect(string text, RepeatMode mode)
        {
            helpers[mode].CheckIsValueCorrect(text);
        }
    }
}
