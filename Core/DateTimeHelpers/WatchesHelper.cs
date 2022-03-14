using Core.Enums;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.DateTimeHelpers
{
    internal class WatchesHelper : IDTHelper
    {
        public void CheckIsValueCorrect(string text)
        {
            string[] strs = text.Split('-');

            if (strs.Length % 2 == 1)
                throw new Exception("Неверный формат: 'xx-xx'; 'xx-xx-xx-xx' ...\nИли неверное количество аргументов: должно быть чётное количество аргументов.");

            foreach (string str in strs)
            {
                int a;

                if (!int.TryParse(str, out a))
                    throw new Exception($"Неверное значение: {str}.");

                if (a < 1)
                    throw new Exception($"Неверное значение количества дней: {str}.");
            }
        }

        public List<TaskInstance> FillRepeatedTasks(Task task)
        {
            List<TaskInstance> models = new List<TaskInstance>();

            List<TaskInstance> taskInstances = GroundhogContext.TaskInstanceLogic.Read(task.Id);
            DateTime lastDate = taskInstances.Max(req => req.Date);
            DateTime currentDate = lastDate;

            string[] strs = task.RepeatValue.Split('-');
            int sum = 0;

            int skip = int.Parse(strs[strs.Length - 1]);
            bool first = false;
            if (taskInstances.Count == 1)
                first = true;
            else
                currentDate = currentDate.AddDays(skip);

            foreach (string s in strs)
                sum += int.Parse(s);

            while ((currentDate - DateTime.Now).TotalDays + sum <= GroundhogContext.GetPlanningRange(RepeatMode.Вахты))
            {
                for (int i = 0; i < strs.Length; i += 2)
                {
                    int work = int.Parse(strs[i]);
                    int relax = int.Parse(strs[i + 1]);

                    for (int j = 0; j < work; j++)
                    {
                        currentDate = currentDate.AddDays(1);

                        if (first)
                        {
                            first = false;
                            continue;
                        }

                        TaskInstance model = new TaskInstance
                        {
                            TaskId = task.Id,
                            Date = currentDate,
                            Completed = false
                        };

                        models.Add(model);
                    }

                    currentDate = currentDate.AddDays(relax);
                }
            }

            return models;
        }

        public DateTime GetDateForTask(Task task, DateTime selectedDate)
        {
            return selectedDate;
        }

        public int TaskRare(Task task)
        {
            string[] strs = task.RepeatValue.Split('-');
            int work = 0;
            int relax = 0;

            for (int i = 0; i < strs.Length; i += 2)
            {
                work += int.Parse(strs[i]);
                relax += int.Parse(strs[i + 1]);
            }

            int sum = work + relax;

            return sum / work;
        }
    }
}
