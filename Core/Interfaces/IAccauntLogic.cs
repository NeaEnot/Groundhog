using Core.Models;
using System.Collections.Generic;

namespace Core.Interfaces
{
    public interface IAccauntLogic
    {
        void Create(Accaunt model);
        List<Accaunt> Read();
        void Update(Accaunt model);
        void Delete(string id);
    }
}
