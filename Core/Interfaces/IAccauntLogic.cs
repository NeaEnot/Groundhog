using Core.Models;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Core.Interfaces
{
    public interface IAccauntLogic
    {
        void Create(Accaunt model);
        List<Accaunt> Read();
        void Update(Accaunt model);
        void Delete(string id);

        Regex ConnectionStringExpr { get; }
        string ConnectionStringFormat { get; }
    }
}
