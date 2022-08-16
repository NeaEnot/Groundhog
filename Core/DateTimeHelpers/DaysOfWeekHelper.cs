using Core.Enums;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.DateTimeHelpers
{
    class DaysOfWeekHelper : IDTHelper
    {
        private static List<string> daysOfWeek = new List<string>(new string[] { "Пн", "Вт", "Ср", "Чт", "Пт", "Сб", "Вс" });

        public void CheckIsValueCorrect(string text)
        {
            string[] days = text.Split(',');

            foreach (string d in days)
                if (!daysOfWeek.Contains(d))
                    throw new Exception($"Неверный день недели: {d}.\nИли неверный формат; верный формат: Пн,Вт,Сб...");
        }

        public List<TaskInstance> FillRepeatedTasks(Task task)
        {
            List<TaskInstance> models = new List<TaskInstance>();

            List<TaskInstance> taskInstances = GroundhogContext.TaskInstanceLogic.Read(task.Id);
            DateTime lastDate = taskInstances.Max(req => req.Date);
            DateTime currentDate = lastDate;

            while ((currentDate - DateTime.Now).TotalDays <= GroundhogContext.Settings.PlanningRanges[RepeatMode.ДниНедели])
            {
                do
                    currentDate = currentDate.AddDays(1);
                while (!task.RepeatValue.Contains(currentDate.ToString("ddd")));

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

            while (!task.RepeatValue.Contains(date.ToString("ddd")))
                date = date.AddDays(1);

            return date;
        }

        public int TaskRare(Task task)
        {
            return task.RepeatValue.Split(',').Length;
        }
    }
}
