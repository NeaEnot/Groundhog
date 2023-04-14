using Core;
using Core.Interfaces.Network;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using YandexDisk.Client;
using YandexDisk.Client.Clients;
using YandexDisk.Client.Http;

namespace YandexDisk.Language
{
    internal class NetworkLanguageLogic : INetworkLogic
    {
        public Regex ConnectionStringExpr => ConnectionString.connectionStringExpr;
        public string ConnectionStringFormat => "token=xxxxx;path=path/to/file.ext";

        private IDiskApi diskApi;
        private string cloudLanguagesFile = $@"{GroundhogContext.StoragePath}\cloudLanguagesFile.json";

        public void Connect(Func<string> getCode)
        {
            diskApi = new DiskHttpApi(ConnectionString.Token);
        }

        public bool IsConnected() => diskApi != null;

        public void Load()
        {
            try
            {
                FileInfo file = new FileInfo(cloudLanguagesFile);
                if (file.Exists)
                    file.Delete();

                System.Threading.Tasks.Task threadTask = diskApi.Files.DownloadFileAsync(ConnectionString.Path, cloudLanguagesFile);
                threadTask.Wait();

                LanguagesSerializer.Deserialize(new DirectoryInfo(GroundhogContext.LanguagesPath), File.ReadAllText(cloudLanguagesFile));
            }
            catch (Exception ex)
            {
                throw new Exception($"{GroundhogContext.Language.ErrorsMessages.FailedToDownloadData}: " + ex.Message);
            }
        }

        public void Upload()
        {
            using (StreamWriter writer = new StreamWriter(cloudLanguagesFile))
            {
                string json = LanguagesSerializer.Serialize(new DirectoryInfo(GroundhogContext.LanguagesPath));
                writer.Write(json);
            }

            try
            {
                diskApi.Files.UploadFileAsync(ConnectionString.Path, true, cloudLanguagesFile, CancellationToken.None).Wait();
            }
            catch (Exception ex)
            {
                throw new Exception($"{GroundhogContext.Language.ErrorsMessages.FailedToUploadData}: " + ex.Message);
            }
        }
    }
}
