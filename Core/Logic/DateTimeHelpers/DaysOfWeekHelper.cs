using Core.Models.Storage;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Logic.DateTimeHelpers
{
    class DaysOfWeekHelper : IDTHelper
    {
        private static List<string> daysOfWeek =
            new List<string>
            {
                GroundhogContext.Language.DaysOfWeek.MondayAbbreviated,
                GroundhogContext.Language.DaysOfWeek.TuesdayAbbreviated,
                GroundhogContext.Language.DaysOfWeek.Wednes­dayAbbreviated,
                GroundhogContext.Language.DaysOfWeek.ThursdayAbbreviated,
                GroundhogContext.Language.DaysOfWeek.FridayAbbreviated,
                GroundhogContext.Language.DaysOfWeek.SaturdayAbbreviated,
                GroundhogContext.Language.DaysOfWeek.SundayAbbreviated
            };

        public void CheckIsValueCorrect(string text)
        {
            string[] days = text.Split(',');

            foreach (string d in days)
                if (!daysOfWeek.Contains(d))
                    throw new Exception($"{GroundhogContext.Language.ErrorsMessages.IncorrectDayOfTheWeek}: {d}.\n" +
                        $"{GroundhogContext.Language.ErrorsMessages.Or} {GroundhogContext.Language.ErrorsMessages.IncorrectFormat}; " +
                        $"{GroundhogContext.Language.ErrorsMessages.CorrectFormat}: {daysOfWeek[0]},{daysOfWeek[1]},{daysOfWeek[5]}...");
        }

        public List<TaskInstance> FillRepeatedTasks(Task task)
        {
            List<TaskInstance> models = new List<TaskInstance>();

            List<TaskInstance> taskInstances = GroundhogContext.TaskInstanceLogic.Read(task.Id);
            DateTime lastDate = taskInstances.Max(req => req.Date);
            DateTime currentDate = lastDate;

            while ((currentDate - DateTime.Now).TotalDays <= task.PlanningRange)
            {
                do
                    currentDate = currentDate.AddDays(1);
                while (!task.RepeatValue.ToLower().Contains(currentDate.ToString("ddd").ToLower()));

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
