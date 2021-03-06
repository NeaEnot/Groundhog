using Core.Models;
using System.Collections.Generic;

namespace Core.Interfaces
{
    public interface IPurposeGroupLogic
    {
        void Create(PurposeGroup model);
        void Create(List<PurposeGroup> models);
        List<PurposeGroup> Read();
        void Update(PurposeGroup model);
        void Delete(string id);
    }
}
