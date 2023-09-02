using Core.Interfaces.Network;
using System;
using System.Text.RegularExpressions;
using YandexDisk.Client;
using YandexDisk.Client.Http;

namespace YandexDisk
{
    public abstract class YandexDiskNetworkLogic : INetworkLogic
    {
        protected static IDiskApi diskApi;

        public string ConnectionStringFormat => "token=xxxxx;path=path/to/file.ext";
        public Regex ConnectionStringExpr => ConnectionString.connectionStringExpr;

        private protected abstract ConnectionString ConnectionString { get; }

        private string lastToken;

        public void Connect(Func<string> getCode)
        {
            lastToken = ConnectionString.Token;
            diskApi = new DiskHttpApi(ConnectionString.Token);
        }

        public bool IsConnected() => diskApi != null && ConnectionString.Token == lastToken;

        public abstract void Load();

        public abstract void Upload();
    }
}
