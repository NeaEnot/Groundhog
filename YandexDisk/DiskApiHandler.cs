using System;
using YandexDisk.Client;
using YandexDisk.Client.Http;

namespace YandexDisk
{
    internal static class DiskApiHandler
    {
        internal static IDiskApi DiskApi { get; private set; }

        internal static void Connect(Func<string> getCode, string token)
        {
            DiskApi = new DiskHttpApi(token);
        }
    }
}
