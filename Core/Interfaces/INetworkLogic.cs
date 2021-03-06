using System;
using System.Text.RegularExpressions;

namespace Core.Interfaces
{
    public interface INetworkLogic
    {
        Regex ConnectionStringExpr { get; }
        string ConnectionStringFormat { get; }

        void Connect(Func<string> getCode);
        bool IsConnected();
        void Load();
        void Upload();
    }
}
