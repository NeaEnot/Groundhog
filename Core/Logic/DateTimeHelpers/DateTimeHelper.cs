﻿using Core.Enums;
using Core.Models.Storage;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Logic.DateTimeHelpers
{
    /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="DateTimeHelper"]/DateTimeHelper/*'/>
    public static class DateTimeHelper
    {
        private static readonly Dictionary<RepeatMode, IDTHelper> helpers = new Dictionary<RepeatMode, IDTHelper>()
        {
            { RepeatMode.None, new NotHelper() },
            { RepeatMode.Days, new DaysHelper() },
            { RepeatMode.DayOfMonth, new DayOfMonthHelper() },
            { RepeatMode.DayOfYear, new DayOfYearHelper() },
            { RepeatMode.DaysOfWeek, new DaysOfWeekHelper() },
            { RepeatMode.Wathes, new WatchesHelper() }
        };

        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="DateTimeHelper"]/FillRepeatedTasks/*'/>
        public static void FillRepeatedTasks()
        {
            List<TaskInstance> models = new List<TaskInstance>();

            List<Task> tasks = GroundhogContext.TaskLogic.Read();
            foreach (Task task in tasks)
                models.AddRange(helpers[task.RepeatMode].FillRepeatedTasks(task));

            if (models.Count > 0)
                GroundhogContext.TaskInstanceLogic.Create(models);
        }

        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="DateTimeHelper"]/DeleteOldTasks/*'/>
        public static void DeleteOldTasks()
        {
            List<TaskInstance> models = new List<TaskInstance>();
            List<Task> tasksToDelete = new List<Task>();

            List<Task> tasks = GroundhogContext.TaskLogic.Read();
            foreach (Task task in tasks)
            {
                List<TaskInstance> instances = 
                    GroundhogContext.TaskInstanceLogic
                    .Read(task.Id)
                    .ToList();
                models.AddRange(instances.Where(req => (DateTime.Now - req.Date).Days >= task.OptimizationRange));

                if (instances.Count == 0)
                    tasksToDelete.Add(task);
            }

            List<string> ids = models.Select(req => req.Id).ToList();
            if (ids.Count > 0)
                GroundhogContext.TaskInstanceLogic.Delete(ids);

            foreach (Task task in tasksToDelete)
                GroundhogContext.TaskLogic.Delete(task.Id);
        }

        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="DateTimeHelper"]/GetDateForTask/*'/>
        public static DateTime GetDateForTask(Task task, DateTime selectedDate)
        {
            return helpers[task.RepeatMode].GetDateForTask(task, selectedDate);
        }

        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="DateTimeHelper"]/ToDay/*'/>
        public static void ToDay(DateTime day)
        {
            List<Task> tasks = GroundhogContext.TaskLogic.Read();
            HashSet<TaskInstance> toUpdate = new HashSet<TaskInstance>();

            foreach (Task task in tasks)
            {
                List<TaskInstance> taskInstances = GroundhogContext.TaskInstanceLogic.Read(task.Id);
                List<TaskInstance> oldTaskInstances = taskInstances.Where(req => req.Date.Date < day.Date && !req.Completed).ToList();

                if (oldTaskInstances.Count == 0)
                    continue;

                DateTime firstDate = oldTaskInstances.Min(req => req.Date);
                int offsetDays = (day.Date - firstDate.Date).Days;

                if (task.OffsetAll)
                {
                    foreach (TaskInstance instance in taskInstances.Where(req => req.Date >= day))
                    {
                        if (!instance.Completed)
                        {
                            instance.Date = instance.Date.AddDays(offsetDays);
                            toUpdate.Add(instance);
                        }
                    }
                }

                foreach (TaskInstance instance in oldTaskInstances)
                {
                    if (task.ToNextDay)
                        instance.Date = day.Date;
                    else
                        instance.Completed = true;

                    toUpdate.Add(instance);
                }
            }

            GroundhogContext.TaskInstanceLogic.Update(toUpdate.ToList());
        }

        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="DateTimeHelper"]/TaskRare/*'/>
        public static int TaskRare(Task task)
        {
            return task != null ? helpers[task.RepeatMode].TaskRare(task) : 0;
        }

        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="DateTimeHelper"]/CheckIsValueCorrect/*'/>
        public static void CheckIsValueCorrect(string text, RepeatMode mode)
        {
            helpers[mode].CheckIsValueCorrect(text);
        }
    }
}
