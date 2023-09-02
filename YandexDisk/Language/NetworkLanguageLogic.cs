using Core;
using System;
using System.IO;
using System.Threading;
using YandexDisk.Client.Clients;

namespace YandexDisk.Language
{
    public class NetworkLanguageLogic : YandexDiskNetworkLogic
    {
        private protected override ConnectionString ConnectionString => new ConnectionString(() => GroundhogContext.Settings.ConnectionStringLanguage);
        private string cloudLanguagesFile = $@"{GroundhogContext.StoragePath}\cloudLanguages.json";

        public override void Load()
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

        public override void Upload()
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
