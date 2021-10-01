using System;

namespace Core.Interfaces
{
    public interface INetworkLogic
    {
        void Connect(Func<string> getCode);
        bool IsConnected();
        void Load();
        void Upload();
    }
}
