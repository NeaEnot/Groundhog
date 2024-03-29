﻿using Core;
using Core.Models.Storage;
using Newtonsoft.Json;
using StorageFile.Extensions;
using StorageFile.Migrations;
using System;
using System.Collections.Generic;
using System.IO;

namespace StorageFile
{
    internal class Context
    {
        private static Context instance;

        internal List<Task> Tasks { get; set; }
        internal List<TaskInstance> TaskInstances { get; set; }
        internal List<Purpose> Purposes { get; set; }
        internal List<PurposeGroup> PurposeGroups { get; set; }
        internal List<Note> Notes { get; set; }

        private Dictionary<string, int> hashes;

        private Context()
        {
            Load();
            
            hashes = new Dictionary<string, int>
            {
                { "Tasks", Tasks.GetHash() },
                { "TaskInstances", TaskInstances.GetHash() },
                { "Purposes", Purposes.GetHash() },
                { "PurposeGroups", PurposeGroups.GetHash() },
                { "Notes", Notes.GetHash() }
            };
        }

        internal static Context Instanse
        {
            get
            {
                if (instance == null)
                {
                    instance = new Context();

                    if (MigrationController.DoMigrationIfNeed())
                        instance.Save();
                }

                return instance;
            }
        }

        private void Save<T>(List<T> models)
        {
            string path = $@"{GroundhogContext.StoragePath}\{typeof(T).Name}s.json";
            using (StreamWriter writer = new StreamWriter(path))
            {
                string json = JsonConvert.SerializeObject(models);
                writer.Write(json);
            }
        }

        private List<T> Load<T>()
        {
            try
            {
                string path = $@"{GroundhogContext.StoragePath}\{typeof(T).Name}s.json";
                using (StreamReader reader = new StreamReader(path))
                {
                    string json = reader.ReadToEnd();
                    List<T> restored = JsonConvert.DeserializeObject<List<T>>(json);

                    return restored ?? new List<T>();
                }
            }
            catch (Exception ex)
            {
                return new List<T>();
            }
        }

        internal void Save()
        {
            if (Tasks.GetHash() != hashes["Tasks"])
            {
                hashes["Tasks"] = Tasks.GetHash();
                Save(Tasks);
            }
            if (TaskInstances.GetHash() != hashes["TaskInstances"])
            {
                hashes["TaskInstances"] = TaskInstances.GetHash();
                Save(TaskInstances);
            }
            if (PurposeGroups.GetHash() != hashes["PurposeGroups"])
            {
                hashes["PurposeGroups"] = PurposeGroups.GetHash();
                Save(PurposeGroups);
            }
            if (Purposes.GetHash() != hashes["Purposes"])
            {
                hashes["Purposes"] = Purposes.GetHash();
                Save(Purposes);
            }
            if (Notes.GetHash() != hashes["Notes"])
            {
                hashes["Notes"] = Notes.GetHash();
                Save(Notes);
            }
        }

        internal void Load()
        {
            Tasks = Load<Task>();
            TaskInstances = Load<TaskInstance>();
            Purposes = Load<Purpose>();
            PurposeGroups = Load<PurposeGroup>();
            Notes = Load<Note>();
        }
    }
}
