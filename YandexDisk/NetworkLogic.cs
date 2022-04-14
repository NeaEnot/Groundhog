using Core;
using Core.Interfaces;
using Core.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using YandexDisk.Client;
using YandexDisk.Client.Clients;
using YandexDisk.Client.Http;

namespace YandexDisk
{
    public class NetworkLogic : INetworkLogic
    {
        public Regex ConnectionStringExpr => ConnectionString.connectionStringExpr;
        public string ConnectionStringFormat => "token=xxxxx;path=path/to/file.ext";

        private IDiskApi diskApi;
        private string cloudStorageFile = $@"{GroundhogContext.StoragePath}\cloudStorage.json";

        public void Connect(Func<string> getCode)
        {
            diskApi = new DiskHttpApi(ConnectionString.Token);
        }

        public bool IsConnected() => diskApi != null;

        public void Load()
        {
            try
            {
                FileInfo file = new FileInfo(cloudStorageFile);
                if (file.Exists)
                    file.Delete();

                System.Threading.Tasks.Task threadTask = diskApi.Files.DownloadFileAsync(ConnectionString.Path, cloudStorageFile);
                threadTask.Wait();

                StorageModel model = null;

                using (StreamReader reader = new StreamReader(cloudStorageFile))
                {
                    string json = reader.ReadToEnd();
                    model = JsonConvert.DeserializeObject<StorageModel>(json);
                }

                GroundhogContext.TaskLogic.Delete(null);
                GroundhogContext.TaskLogic.Create(model.Tasks);
                GroundhogContext.TaskInstanceLogic.Delete();
                GroundhogContext.TaskInstanceLogic.Create(model.TaskInstances);
                GroundhogContext.PurposeGroupLogic.Delete(null);
                GroundhogContext.PurposeGroupLogic.Create(model.PurposeGroups);
                GroundhogContext.PurposeLogic.Delete();
                GroundhogContext.PurposeLogic.Create(model.Purposes);
            }
            catch (Exception ex)
            {
                throw new Exception("Не удалось скачать данные: " + ex.Message);
            }
        }

        public void Upload()
        {
            List<Task> tasks = GroundhogContext.TaskLogic.Read();
            List<TaskInstance> taskInstances = new List<TaskInstance>();
            foreach (Task task in tasks)
                taskInstances.AddRange(GroundhogContext.TaskInstanceLogic.Read(task.Id));
            List<PurposeGroup> groups = GroundhogContext.PurposeGroupLogic.Read();
            List<Purpose> purposes = new List<Purpose>();
            foreach (PurposeGroup group in groups)
                purposes.AddRange(GroundhogContext.PurposeLogic.Read(group.Id));

            StorageModel model = new StorageModel
            {
                Tasks = tasks,
                TaskInstances = taskInstances,
                PurposeGroups = groups,
                Purposes = purposes
            };

            using (StreamWriter writer = new StreamWriter(cloudStorageFile))
            {
                string json = JsonConvert.SerializeObject(model);
                writer.Write(json);
            }

            try
            {
                diskApi.Files.UploadFileAsync(ConnectionString.Path, true, cloudStorageFile, CancellationToken.None).Wait();
            }
            catch (Exception ex)
            {
                throw new Exception("Не удалось загрузить данные: " + ex.Message);
            }
        }
    }
}
