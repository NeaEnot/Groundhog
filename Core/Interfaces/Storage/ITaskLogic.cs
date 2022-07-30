using Core.Models;
using System.Collections.Generic;

namespace Core.Interfaces.Storage
{
    public interface ITaskLogic
    {
        void Create(Task model);
        void Create(List<Task> models);
        List<Task> Read();
        Task Read(string id);
        void Update(Task model);
        void Delete(string id);
    }
}
