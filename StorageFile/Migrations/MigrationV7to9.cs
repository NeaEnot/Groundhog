using Core;
using Core.Enums;
using Core.Models.Storage;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace StorageFile.Migrations
{
    class MigrationV7to9 : IMigration
    {
        public bool CheckNeedMigration()
        {
            try
            {
                using (StreamReader reader = new StreamReader($@"{GroundhogContext.StoragePath}\storageVersion.dat"))
                {
                    string version = reader.ReadToEnd();
                    string mainVersion = version.Split('.')[0];

                    if (mainVersion == "7")
                        return true;
                }
            }
            catch
            {
                return true;
            }

            return false;
        }

        public void DoMigration()
        {
            List<Task> tasks = new List<Task>();

            Dictionary<OldRepeatMode, RepeatMode> dict = new Dictionary<OldRepeatMode, RepeatMode>
            {
                { OldRepeatMode.Нет, RepeatMode.None },
                { OldRepeatMode.Дни, RepeatMode.Days },
                { OldRepeatMode.ЧислоМесяца, RepeatMode.DayOfMonth },
                { OldRepeatMode.ДеньГода, RepeatMode.DayOfYear },
                { OldRepeatMode.ДниНедели, RepeatMode.DaysOfWeek },
                { OldRepeatMode.Вахты, RepeatMode.Wathes },
            };

            try
            {
                using (StreamReader reader = new StreamReader($@"{GroundhogContext.StoragePath}\Tasks.json"))
                {
                    string json = reader.ReadToEnd();
                    List<OldTask> restored = JsonConvert.DeserializeObject<List<OldTask>>(json);

                    tasks =
                        restored
                        .Select(req => new Task
                        {
                            Id = req.Id,
                            Text = req.Text,
                            RepeatMode = dict[req.RepeatMode],
                            RepeatValue = req.RepeatValue,
                            ToNextDay = req.ToNextDay,
                            OffsetAll = req.OffsetAll,
                            PlanningRange = req.PlanningRange,
                            OptimizationRange = req.OptimizationRange
                        })
                        .ToList();
                }
            }
            catch (Exception ex)
            { }

            Context.Instanse.Tasks.Clear();
            Context.Instanse.Tasks.AddRange(tasks);

            using (StreamWriter writer = new StreamWriter($@"{GroundhogContext.StoragePath}\storageVersion.dat"))
                writer.Write("9.0");
        }

        private enum OldRepeatMode
        {
            Нет,
            Дни,
            ЧислоМесяца,
            ДеньГода,
            ДниНедели,
            Вахты
        }

        private class OldTask
        {
            public string Id { get; set; }
            public string Text { get; set; }
            public OldRepeatMode RepeatMode { get; set; }
            public string RepeatValue { get; set; }
            public bool ToNextDay { get; set; }
            public bool OffsetAll { get; set; }
            public int PlanningRange { get; set; }
            public int OptimizationRange { get; set; }
        }
    }
}
