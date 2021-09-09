using Core.Enums;
using Core.Interfaces;
using Core.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Core
{
    public static class GroundhogContext
    {
        public static IAccauntLogic AccauntLogic { get; set; }
        public static ITaskInstanceLogic TaskInstanceLogic  { get; set; }
        public static ITaskLogic TaskLogic { get; set; }

        private static AppSettings settings;

        public static Accaunt Accaunt 
        { 
            get
            {
                if (settings == null)
                {
                    try
                    {
                        using (StreamReader reader = new StreamReader($"AppSettings.json"))
                        {
                            string json = reader.ReadToEnd();
                            settings = JsonConvert.DeserializeObject<AppSettings>(json);
                        }
                    }
                    catch
                    {
                        settings = new AppSettings();
                    }
                }

                try
                {
                    return AccauntLogic.Read().First(req => req.Id == settings.AccauntId);
                }
                catch
                {
                    return null;
                }
            }
            set
            {
                settings.AccauntId = value != null ? value.Id : null;
                using (StreamWriter writer = new StreamWriter($"AppSettings.json"))
                {
                    string json = JsonConvert.SerializeObject(settings);
                    writer.Write(json);
                }
            }
        }

        public static void FillRepeatedTasks()
        {
            if (Accaunt != null)
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
}
