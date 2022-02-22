﻿using Core.Enums;
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
            foreach (Task task in tasks.Where(t => t.RepeatMode != RepeatMode.Нет))
            {
                List<TaskInstance> taskInstances = GroundhogContext.TaskInstanceLogic.Read(task.Id);
                DateTime lastDate = taskInstances.Max(req => req.Date);
                DateTime currentDate = lastDate;

                while ((currentDate - DateTime.Now).TotalDays <= 100)
                {
                    switch (task.RepeatMode)
                    {
                        case RepeatMode.Дни:
                            int days = int.Parse(task.RepeatValue);
                            currentDate = currentDate.AddDays(days);
                            break;
                        case RepeatMode.ЧислоМесяца:
                            int day = int.Parse(task.RepeatValue);
                            currentDate = currentDate.AddMonths(1);

                            if (day > currentDate.Day && DateTime.DaysInMonth(currentDate.Year, currentDate.Month) > currentDate.Day)
                                currentDate = new DateTime(currentDate.Year, currentDate.Month, DateTime.DaysInMonth(currentDate.Year, currentDate.Month));

                            break;
                        case RepeatMode.ДеньГода:
                            currentDate = currentDate.AddYears(1);

                            // Обработка задачи на 29 февраля
                            if (task.RepeatValue == "02.29" && currentDate.Month == 3 && DateTime.DaysInMonth(currentDate.Year, 2) == 29)
                                currentDate = new DateTime(currentDate.Year, 2, 29);

                            break;
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
            switch (task.RepeatMode)
            {
                case RepeatMode.ЧислоМесяца:
                    return GetDateForMounth(task, selectedDate);
                case RepeatMode.ДеньГода:
                    int mounth = int.Parse(task.RepeatValue.Split('.')[0]);
                    int day = int.Parse(task.RepeatValue.Split('.')[1]);

                    if (task.RepeatValue == "02.29" && DateTime.DaysInMonth(selectedDate.Year, 2) == 28)
                        return new DateTime(selectedDate.Year, 3, 1);
                    else
                        return new DateTime(selectedDate.Year, mounth, day);
                default:
                    return selectedDate;
            }
        }

        private static DateTime GetDateForMounth(Task task, DateTime selectedDate)
        {
            DateTime date = selectedDate;
            int value = int.Parse(task.RepeatValue);

            int days = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            if (days < value)
                date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, days);
            else
                date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, value);

            if (date < DateTime.Now)
                date = date.AddMonths(1);

            days = DateTime.DaysInMonth(DateTime.Now.Year, date.Month);
            if (days < value)
                date = new DateTime(DateTime.Now.Year, date.Month, days);
            else
                date = new DateTime(DateTime.Now.Year, date.Month, value);

            return date;
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

        public static int TaskRare(TaskInstance instance, List<Task> tasks)
        {
            Task task = tasks.First(req => req.Id == instance.TaskId);

            int answer = 0;

            switch (task.RepeatMode)
            {
                case RepeatMode.Нет:
                    answer = int.MaxValue;
                    break;
                case RepeatMode.Дни:
                    answer = int.Parse(task.RepeatValue);
                    break;
                case RepeatMode.ЧислоМесяца:
                    answer = 31;
                    break;
                case RepeatMode.ДеньГода:
                    answer = 366;
                    break;
            }

            return answer;
        }
    }
}
