﻿using Core.Models.Storage;
using System.Collections.Generic;

namespace Core.Interfaces.Storage
{
    public interface IPurposeLogic
    {
        void Create(Purpose model);
        void Create(List<Purpose> models);
        List<Purpose> Read(string groupId);
        void Update(Purpose model);
        void Delete();
        void Delete(string id);
        void Delete(List<string> ids);
    }
}
