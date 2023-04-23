using Core;
using Core.Interfaces.Network;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using YandexDisk.Client.Clients;

namespace YandexDisk.Language
{
    public class NetworkLanguageLogic : INetworkLogic
    {
        public Regex ConnectionStringExpr => ConnectionString.connectionStringExpr;
        public string ConnectionStringFormat => "token=xxxxx;path=path/to/file.ext";

        private ConnectionString connectionString = new ConnectionString(GroundhogContext.Settings.ConnectionStringLanguage);
        private string cloudLanguagesFile = $@"{GroundhogContext.StoragePath}\cloudLanguages.json";

        public void Connect(Func<string> getCode)
        {
            DiskApiHandler.Connect(getCode, connectionString.Token);
        }

        public bool IsConnected() => DiskApiHandler.DiskApi != null;

        public void Load()
        {
            try
            {
                FileInfo file = new FileInfo(cloudLanguagesFile);
                if (file.Exists)
                    file.Delete();

                System.Threading.Tasks.Task threadTask = DiskApiHandler.DiskApi.Files.DownloadFileAsync(connectionString.Path, cloudLanguagesFile);
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
                DiskApiHandler.DiskApi.Files.UploadFileAsync(connectionString.Path, true, cloudLanguagesFile, CancellationToken.None).Wait();
            }
            catch (Exception ex)
            {
                throw new Exception($"{GroundhogContext.Language.ErrorsMessages.FailedToUploadData}: " + ex.Message);
            }
        }
    }
}
