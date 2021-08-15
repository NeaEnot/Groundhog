using Core.Models;
using System.Collections.Generic;

namespace Core.Interfaces
{
    public interface IAccauntLogic
    {
        public void Create(Accaunt model);
        public List<Accaunt> Read();
        public void Update(Accaunt model);
        public void Delete(string id);
    }
}
