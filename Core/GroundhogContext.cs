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
        public static INetworkLogic NetworkLogic { get; set; }

        private static AppSettings settings;

        public static Accaunt Accaunt 
        { 
            get
            {
                if (settings == null)
                {
                    try
                    {
                        using (StreamReader reader = new StreamReader($"{StoragePath}\\AppSettings.json"))
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
                using (StreamWriter writer = new StreamWriter($"{StoragePath}\\AppSettings.json"))
                {
                    string json = JsonConvert.SerializeObject(settings);
                    writer.Write(json);
                }
            }
        }

        public static string StoragePath
        {
            get
            {
                string system = System.Runtime.InteropServices.RuntimeInformation.OSDescription;
                string storagePath;

                if (system.Contains("Microsoft Windows"))
                    storagePath = "storage";
                else
                    storagePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

                DirectoryInfo storage = new DirectoryInfo(storagePath);
                if (!storage.Exists)
                    storage.Create();

                return storagePath;
            }
        }

        public static void FillRepeatedTasks()
        {
            if (Accaunt != null)
            {
                List<TaskInstance> models = new List<TaskInstance>();

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

                            models.Add(model);
                        }
                    }
                }

                TaskInstanceLogic.Create(models);
            }
        }

        public static DateTime GetDateForTask(Task task, DateTime selectedDate)
        {
            DateTime date = selectedDate;

            if (task.RepeatMode == RepeatMode.ЧислоМесяца)
            {
                int days = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
                if (days < task.RepeatValue)
                    date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, days);
                else
                    date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, task.RepeatValue);

                if (date < DateTime.Now)
                    date = date.AddMonths(1);

                days = DateTime.DaysInMonth(DateTime.Now.Year, date.Month);
                if (days < task.RepeatValue)
                    date = new DateTime(DateTime.Now.Year, date.Month, days);
                else
                    date = new DateTime(DateTime.Now.Year, date.Month, task.RepeatValue);
            }

            return date;
        }
    }
}
