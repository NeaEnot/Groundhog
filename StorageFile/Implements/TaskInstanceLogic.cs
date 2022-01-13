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
            model.Id = IdHelper.GetNextId(IdHelper.GetMaxId(context.TaskInstances.Select(req => req.Id).ToList()));
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
            string currentId = IdHelper.GetMaxId(context.TaskInstances.Select(req => req.Id).ToList());

            foreach (TaskInstance model in models)
            {
                currentId = IdHelper.GetNextId(currentId);
                model.Id = currentId;

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

        public void Delete()
        {
            context.TaskInstances.Clear();
            context.Save();
        }

        public void Delete(string id)
        {
            TaskInstance instance = context.TaskInstances.FirstOrDefault(req => req.Id == id);

            if (instance == null)
                throw new Exception("Экземпляра задачи с данным Id не существует.");

            context.TaskInstances.Remove(instance);

            context.Save();
        }

        public void Delete(List<string> ids)
        {
            IEnumerable<TaskInstance> instances = context.TaskInstances.Where(req => ids.Contains(req.Id)).ToList();

            foreach (TaskInstance instance in instances)
                context.TaskInstances.Remove(instance);

            context.Save();
        }
    }
}
