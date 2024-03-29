﻿using Core;
using Core.Interfaces.Storage;
using Core.Models.Storage;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StorageFile.Implements
{
    public class TaskLogic : ITaskLogic
    {
        private Context context = Context.Instanse;

        public void Create(Task model)
        {
            model.Id = IdHelper.GetId("t_");
            context.Tasks
                .Add(new Task
                {
                    Id = model.Id,
                    Text = model.Text,
                    RepeatMode = model.RepeatMode,
                    RepeatValue = model.RepeatValue,
                    ToNextDay = model.ToNextDay,
                    OffsetAll = model.OffsetAll,
                    PlanningRange = model.PlanningRange,
                    OptimizationRange = model.OptimizationRange
                });

            context.Save();
        }

        public void Create(List<Task> models)
        {
            foreach (Task model in models)
            {
                if (string.IsNullOrEmpty(model.Id))
                    model.Id = IdHelper.GetId("t_");

                context.Tasks
                    .Add(new Task
                    {
                        Id = model.Id,
                        Text = model.Text,
                        RepeatMode = model.RepeatMode,
                        RepeatValue = model.RepeatValue,
                        ToNextDay = model.ToNextDay,
                        OffsetAll = model.OffsetAll,
                        PlanningRange = model.PlanningRange,
                        OptimizationRange = model.OptimizationRange
                    });
            }

            context.Save();
        }

        public List<Task> Read()
        {
            return context.Tasks
                .Select(req => new Task
                {
                    Id = req.Id,
                    Text = req.Text,
                    RepeatMode = req.RepeatMode,
                    RepeatValue = req.RepeatValue,
                    ToNextDay = req.ToNextDay,
                    OffsetAll = req.OffsetAll,
                    PlanningRange = req.PlanningRange,
                    OptimizationRange = req.OptimizationRange
                })
                .ToList();
        }

        public Task Read(string id)
        {
            Task task = context.Tasks.FirstOrDefault(req => req.Id == id);

            if (task == null)
                throw new Exception($"{GroundhogContext.Language.ErrorsMessages.EntityWithSameIdDontExist}: {id}.");

            return new Task
            {
                Id = task.Id,
                Text = task.Text,
                RepeatMode = task.RepeatMode,
                RepeatValue = task.RepeatValue,
                ToNextDay = task.ToNextDay,
                OffsetAll = task.OffsetAll,
                PlanningRange = task.PlanningRange,
                OptimizationRange = task.OptimizationRange
            };
        }

        public void Update(Task model)
        {
            Task task = context.Tasks.FirstOrDefault(req => req.Id == model.Id);

            if (task == null)
            {
                throw new Exception($"{GroundhogContext.Language.ErrorsMessages.EntityWithSameIdDontExist}: {model.Id}.");
            }

            task.Text = model.Text;
            task.RepeatMode = model.RepeatMode;
            task.RepeatValue = model.RepeatValue;
            task.ToNextDay = model.ToNextDay;
            task.OffsetAll = model.OffsetAll;
            task.PlanningRange = model.PlanningRange;
            task.OptimizationRange = model.OptimizationRange;

            context.Save();
        }

        public void Delete(string id)
        {
            if (id == null)
            {
                context.Tasks.Clear();
            }
            else
            {
                Task task = context.Tasks.FirstOrDefault(req => req.Id == id);

                if (task == null)
                {
                    throw new Exception($"{GroundhogContext.Language.ErrorsMessages.EntityWithSameIdDontExist}: {id}.");
                }

                context.Tasks.Remove(task);
            }

            context.Save();
        }
    }
}
