using Core.Models;
using System.Collections.Generic;

namespace Core.Interfaces.Storage
{
    public interface INoteLogic
    {
        void Create(Note model);
        void Create(List<Note> models);
        List<Note> Read();
        void Update(Note model);
        void Delete(string id);
    }
}
