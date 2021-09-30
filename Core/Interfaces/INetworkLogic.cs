using Core.Models;
using System;

namespace Core.Interfaces
{
    public interface INetworkLogic
    {
        void Sinchronize(Accaunt accaunt);
        void Connect(Func<string> getCode);
    }
}
