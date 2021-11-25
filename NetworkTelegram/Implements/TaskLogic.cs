using Core.Interfaces;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetworkTelegram.Implements
{
    public class TaskLogic : ITaskLogic
    {
        private Context context = Context.Instanse;

        public void Create(Task model)
        {
            model.Id = Guid.NewGuid().ToString();
            context.Tasks
                .Add(new Task
                {
                    Id = model.Id,
                    AccauntId = model.AccauntId,
                    Text = model.Text,
                    RepeatMode = model.RepeatMode,
                    RepeatValue = model.RepeatValue,
                });

            context.Save();
        }

        public List<Task> Read(Accaunt accaunt)
        {
            return context.Tasks
                .Where(req => req.AccauntId == accaunt.Id)
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
            Task task = context.Tasks.FirstOrDefault(req => req.Id == id);

            if (task == null)
            {
                throw new Exception("Задачи с данным Id не существует.");
            }

            context.Tasks.Remove(task);

            context.Save();
        }
    }
}
