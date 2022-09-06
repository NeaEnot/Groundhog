using Core;
using Core.Enums;
using Core.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace StorageFile.Migrations
{
    internal class MigrationV6to7 : IMigration
    {
        public bool CheckNeedMigration()
        {
            try
            {
                using (StreamReader reader = new StreamReader($@"{GroundhogContext.StoragePath}\storageVersion.dat"))
                {
                    string version = reader.ReadToEnd();
                    string mainVersion = version.Split('.')[0];

                    if (mainVersion == "6")
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
            List<TaskInstance> taskInstances = new List<TaskInstance>();
            List<Purpose> purposes = new List<Purpose>();
            List<PurposeGroup> purposeGroups = new List<PurposeGroup>();

            try
            {
                using (StreamReader reader = new StreamReader($@"{GroundhogContext.StoragePath}\storage.json"))
                {
                    string json = reader.ReadToEnd();
                    OldStorageModel restored = JsonConvert.DeserializeObject<OldStorageModel>(json);

                    taskInstances = restored.TaskInstances;
                    purposes = restored.Purposes;
                    purposeGroups = restored.PurposeGroups;

                    tasks =
                        restored.Tasks
                        .Select(req => new Task
                        {
                            Id = req.Id,
                            Text = req.Text,
                            RepeatMode = req.RepeatMode,
                            RepeatValue = req.RepeatValue,
                            ToNextDay = req.ToNextDay,
                            OffsetAll = false,
                            PlanningRange = GroundhogContext.Settings.PlanningRanges[req.RepeatMode],
                            OptimizationRange = GroundhogContext.Settings.OptimizationRange
                        })
                        .ToList();
                }
            }
            catch (Exception ex)
            { }

            Context.Instanse.Tasks.Clear();
            Context.Instanse.Tasks.AddRange(tasks);
            Context.Instanse.TaskInstances.Clear();
            Context.Instanse.TaskInstances.AddRange(taskInstances);
            Context.Instanse.PurposeGroups.Clear();
            Context.Instanse.PurposeGroups.AddRange(purposeGroups);
            Context.Instanse.Purposes.Clear();
            Context.Instanse.Purposes.AddRange(purposes);

            using (StreamWriter writer = new StreamWriter($@"{GroundhogContext.StoragePath}\storageVersion.dat"))
                writer.Write("7.0");
        }

        private class OldTask
        {
            public string Id { get; set; }
            public string Text { get; set; }
            public RepeatMode RepeatMode { get; set; }
            public string RepeatValue { get; set; }
            public bool ToNextDay { get; set; }
        }

        private class OldStorageModel
        {
            public List<OldTask> Tasks { get; set; }
            public List<TaskInstance> TaskInstances { get; set; }
            public List<Purpose> Purposes { get; set; }
            public List<PurposeGroup> PurposeGroups { get; set; }
        }
    }
}
