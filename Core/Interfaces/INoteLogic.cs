using Core.Models;
using System.Collections.Generic;

namespace Core.Interfaces
{
    public interface INoteLogic
    {
        void Create(Note model);
        void Create(List<Note> models);
        List<Note> Read();
        Note Read(string id);
        void Update(Note model);
        void Delete(string id);
    }
}
