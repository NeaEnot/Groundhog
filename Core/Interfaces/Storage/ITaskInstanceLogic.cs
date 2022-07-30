using Core.Models;
using System;
using System.Collections.Generic;

namespace Core.Interfaces.Storage
{
    public interface ITaskInstanceLogic
    {
        void Create(TaskInstance model);
        void Create(List<TaskInstance> models);
        List<TaskInstance> Read(DateTime date);
        List<TaskInstance> Read(string taskId);
        void Update(TaskInstance model);
        void Delete();
        void Delete(string id);
        void Delete(List<string> ids);
    }
}
