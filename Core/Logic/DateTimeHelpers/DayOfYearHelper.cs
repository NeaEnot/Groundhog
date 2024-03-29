﻿using Core.Models.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Core.Logic.DateTimeHelpers
{
    internal class DayOfYearHelper : IDTHelper
    {
        private static Regex dayOfYearReg = new Regex(@"^(?<mounth>\d\d).(?<day>\d\d)$");

        public void CheckIsValueCorrect(string text)
        {
            if (!dayOfYearReg.IsMatch(text))
                throw new Exception($"{GroundhogContext.Language.ErrorsMessages.IncorrectFormatOfDayOfMonth}: 'MM.dd'.");

            GroupCollection groups = dayOfYearReg.Match(text).Groups;
            int mounth;
            int day;

            if (!int.TryParse(groups["mounth"].Value, out mounth))
                throw new Exception($"{GroundhogContext.Language.ErrorsMessages.IncorrectNumberOfMonth}.");

            if (!int.TryParse(groups["day"].Value, out day))
                throw new Exception($"{GroundhogContext.Language.ErrorsMessages.IncorrectFormatOfDayOfMonth}.");

            if (day > DateTime.DaysInMonth(2020, mounth))
                throw new Exception($"{GroundhogContext.Language.ErrorsMessages.ThereAreFewerDaysInSpecifiedMonth}.");
        }

        public List<TaskInstance> FillRepeatedTasks(Task task)
        {
            List<TaskInstance> models = new List<TaskInstance>();

            List<TaskInstance> taskInstances = GroundhogContext.TaskInstanceLogic.Read(task.Id);
            DateTime lastDate = taskInstances.Max(req => req.Date);
            DateTime currentDate = lastDate;

            while ((currentDate - DateTime.Now).TotalDays <= task.PlanningRange)
            {
                currentDate = currentDate.AddYears(1);

                // Processing the task for February 29
                if (task.RepeatValue == "02.29" && currentDate.Month == 2 && DateTime.DaysInMonth(currentDate.Year, 2) == 29)
                    currentDate = new DateTime(currentDate.Year, 2, 29);
                else
                    currentDate = new DateTime(currentDate.Year, int.Parse(task.RepeatValue.Split('.')[0]), int.Parse(task.RepeatValue.Split('.')[1]));

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
            int mounth = int.Parse(task.RepeatValue.Split('.')[0]);
            int day = int.Parse(task.RepeatValue.Split('.')[1]);

            if (task.RepeatValue == "02.29" && DateTime.DaysInMonth(selectedDate.Year, 2) == 28)
                return new DateTime(selectedDate.Year, 3, 1);
            else
                return new DateTime(selectedDate.Year, mounth, day);
        }

        public int TaskRare(Task task)
        {
            return 366;
        }
    }
}
