using Core.Interfaces;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StorageFile.Implements
{
    public class TaskInstanceLogic : ITaskInstanceLogic
    {
        private Context context = Context.Instanse;

        public void Create(TaskInstance model)
        {
            model.Id = Guid.NewGuid().ToString();
            context.TaskInstances
                .Add(new TaskInstance
                {
                    Id = model.Id,
                    Date = model.Date,
                    TaskId = model.TaskId,
                    Completed = model.Completed
                });

            context.Save();
        }

        public void Create(List<TaskInstance> models)
        {
            foreach (TaskInstance model in models)
            {
                model.Id = Guid.NewGuid().ToString();
                context.TaskInstances
                    .Add(new TaskInstance
                    {
                        Id = model.Id,
                        Date = model.Date,
                        TaskId = model.TaskId,
                        Completed = model.Completed
                    });
            }

            context.Save();
        }

        public List<TaskInstance> Read(DateTime date)
        {
            return context.TaskInstances
                .Where(req => req.Date.ToString("dd.MM.yyyy") == date.ToString("dd.MM.yyyy"))
                .Select(req => new TaskInstance
                {
                    Id = req.Id,
                    Date = req.Date,
                    TaskId = req.TaskId,
                    Completed = req.Completed
                })
                .ToList();
        }

        public List<TaskInstance> Read(string taskId)
        {
            return context.TaskInstances
                .Where(req => req.TaskId == taskId)
                .Select(req => new TaskInstance
                {
                    Id = req.Id,
                    Date = req.Date,
                    TaskId = req.TaskId,
                    Completed = req.Completed
                })
                .ToList();
        }

        public void Update(TaskInstance model)
        {
            TaskInstance instance = context.TaskInstances.FirstOrDefault(req => req.Id == model.Id);

            if (instance == null)
            {
                throw new Exception("Экземпляра задачи с данным Id не существует.");
            }

            instance.Date = model.Date;
            instance.TaskId = model.TaskId;
            instance.Completed = model.Completed;

            context.Save();
        }

        public void Delete(string id)
        {
            if (id == null)
            {
                context.TaskInstances.Clear();
            }
            else
            {
                TaskInstance instance = context.TaskInstances.FirstOrDefault(req => req.Id == id);

                if (instance == null)
                {
                    throw new Exception("Экземпляра задачи с данным Id не существует.");
                }

                context.TaskInstances.Remove(instance);
            }

            context.Save();
        }
    }
}
