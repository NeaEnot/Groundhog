using Core.Models;
using System;
using System.Collections.Generic;

namespace Core.Interfaces
{
    public interface ITaskInstanceLogic
    {
        public void Create(TaskInstance model);
        public List<TaskInstance> Read(DateTime date);
        public void Update(TaskInstance model);
        public void Delete(string id);
    }
}
