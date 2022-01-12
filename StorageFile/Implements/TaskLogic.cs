using Core.Interfaces;
using Core.Models;
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
            model.Id = IdHelper.GetNextId(IdHelper.GetMaxId(context.Tasks.Select(req => req.Id).ToList()));
            context.Tasks
                .Add(new Task
                {
                    Id = model.Id,
                    Text = model.Text,
                    RepeatMode = model.RepeatMode,
                    RepeatValue = model.RepeatValue,
                });

            context.Save();
        }

        public void Create(List<Task> models)
        {
            string currentId = IdHelper.GetMaxId(context.Tasks.Select(req => req.Id).ToList());

            foreach (Task model in models)
            {
                if (string.IsNullOrEmpty(model.Id))
                {
                    currentId = IdHelper.GetNextId(currentId);
                    model.Id = currentId;
                }

                context.Tasks
                    .Add(new Task
                    {
                        Id = model.Id,
                        Text = model.Text,
                        RepeatMode = model.RepeatMode,
                        RepeatValue = model.RepeatValue,
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
                    RepeatValue = req.RepeatValue
                })
                .ToList();
        }

        public Task Read(string id)
        {
            Task task = context.Tasks.FirstOrDefault(req => req.Id == id);

            if (task == null)
            {
                throw new Exception("Задачи с данным id не существует.");
            }

            return new Task
            {
                Id = task.Id,
                Text = task.Text,
                RepeatMode = task.RepeatMode,
                RepeatValue = task.RepeatValue,
            };
        }

        public void Update(Task model)
        {
            Task task = context.Tasks.FirstOrDefault(req => req.Id == model.Id);

            if (task == null)
            {
                throw new Exception("Задачи с данным Id не существует.");
            }

            task.Text = model.Text;
            task.RepeatMode = model.RepeatMode;
            task.RepeatValue = model.RepeatValue;

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
                    throw new Exception("Задачи с данным Id не существует.");
                }

                context.Tasks.Remove(task);
            }

            context.Save();
        }
    }
}
