using Core.Models;
using System.Collections.Generic;

namespace Core.Interfaces
{
    public interface ITaskLogic
    {
        public void Create(Task model);
        public List<Task> Read(Accaunt accaunt);
        public Task Read(string id);
        public void Update(Task model);
        public void Delete(string id);
    }
}
