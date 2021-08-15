using Core.Models;
using System.Collections.Generic;

namespace Core.Interfaces
{
    public interface TaskInterface
    {
        public void Create(Task model);
        public List<Task> Read();
        public Task Read(string id);
        public void Update(Task model);
        public void Delete(string id);
    }
}
