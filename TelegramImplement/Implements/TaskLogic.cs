using Core.Interfaces;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TelegramImplement.Implements
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
                    Text = model.Text,
                    RepeatMode = model.RepeatMode,
                    RepeatValue = model.RepeatValue,
                });

            context.Save();
        }

        public void Delete(string id)
        {
            throw new NotImplementedException();
        }

        public List<Task> Read()
        {
            throw new NotImplementedException();
        }

        public Task Read(string id)
        {
            throw new NotImplementedException();
        }

        public void Update(Task model)
        {
            throw new NotImplementedException();
        }
    }
}
