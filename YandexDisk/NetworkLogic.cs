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
                diskApi.Files.DownloadFileAsync(ConnectionString.Path, cloudStorageFile);

                List<Task> tasks = new List<Task>();
                List<TaskInstance> taskInstances = new List<TaskInstance>();

                using (StreamReader reader = new StreamReader(cloudStorageFile))
                {
                    string json = reader.ReadToEnd();
                    (List<Task>, List<TaskInstance>) restored = JsonConvert.DeserializeObject<(List<Task>, List<TaskInstance>)>(json);
                    tasks = restored.Item1;
                    taskInstances = restored.Item2;
                }

                GroundhogContext.TaskLogic.Delete(null);
                GroundhogContext.TaskLogic.Create(tasks);
                GroundhogContext.TaskInstanceLogic.Delete();
                GroundhogContext.TaskInstanceLogic.Create(taskInstances);
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

            using (StreamWriter writer = new StreamWriter(cloudStorageFile))
            {
                string json = JsonConvert.SerializeObject((tasks, taskInstances));
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
