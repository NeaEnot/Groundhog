using Core.Enums;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.DateTimeHelpers
{
    internal class DaysHelper : IDTHelper
    {
        public void CheckIsValueCorrect(string text)
        {
            int a;

            if (!int.TryParse(text, out a))
                throw new Exception("Неверное значение.");

            if (a < 1)
                throw new Exception("Неверное число дней.");
        }

        public List<TaskInstance> FillRepeatedTasks(Task task)
        {
            List<TaskInstance> models = new List<TaskInstance>();

            List<TaskInstance> taskInstances = GroundhogContext.TaskInstanceLogic.Read(task.Id);
            DateTime lastDate = taskInstances.Max(req => req.Date);
            DateTime currentDate = lastDate;

            while ((currentDate - DateTime.Now).TotalDays <= GroundhogContext.GetPlanningRange(RepeatMode.Дни))
            {
                int days = int.Parse(task.RepeatValue);
                currentDate = currentDate.AddDays(days);

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
            throw new NotImplementedException();
        }

        public int TaskRare(Task task)
        {
            return int.Parse(task.RepeatValue);
        }
    }
}
