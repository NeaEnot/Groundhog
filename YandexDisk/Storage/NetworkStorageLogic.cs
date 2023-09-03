using Core;
using Core.Interfaces.Network;
using Core.Models.Storage;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using YandexDisk.Client.Clients;
using YandexDisk.Client.Protocol;

namespace YandexDisk.Storage
{
    public class NetworkStorageLogic : YandexDiskNetworkLogic, IBackupLogic
    {
        private protected override ConnectionString ConnectionString => new ConnectionString(() => GroundhogContext.Settings.ConnectionStringStorage);

        public List<string> Backups
        {
            get
            {
                if (!IsConnected())
                    Connect(() => "");

                System.Threading.Tasks.Task<FilesResourceList> threadTask = diskApi.MetaInfo.GetFilesInfoAsync(new FilesResourceRequest { Limit = int.MaxValue });
                threadTask.Wait();

                string path = ConnectionString.Path;
                int index = ConnectionString.Path.LastIndexOf('/');
                string name = path.Substring(index);

                return
                    threadTask.Result.Items
                    .Where(f => f.Path.StartsWith($@"disk:{path}") && f.Path != $@"disk:{path}" && f.Name.EndsWith(".backup"))
                    .Select(f => f.Name.Replace(name, "").Replace(".backup", ""))
                    .ToList();
            }
        }

        private string cloudStorageFile = $@"{GroundhogContext.StoragePath}\cloudStorage.json";

        public override void Load()
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
                GroundhogContext.NoteLogic.Delete(null);
                GroundhogContext.NoteLogic.Create(model.Notes);

                if (model.AppSettings.ConnectionStringStorage != GroundhogContext.Settings.ConnectionStringStorage)
                    model.AppSettings.ConnectionStringStorage = GroundhogContext.Settings.ConnectionStringStorage;
                if (model.AppSettings.ConnectionStringLanguage != GroundhogContext.Settings.ConnectionStringLanguage)
                    model.AppSettings.ConnectionStringLanguage = GroundhogContext.Settings.ConnectionStringLanguage;

                GroundhogContext.Settings = model.AppSettings;
            }
            catch (Exception ex)
            {
                throw new Exception($"{GroundhogContext.Language.ErrorsMessages.FailedToDownloadData}: " + ex.Message);
            }
        }

        public override void Upload()
        {
            List<Task> tasks = GroundhogContext.TaskLogic.Read();
            List<TaskInstance> taskInstances = new List<TaskInstance>();
            foreach (Task task in tasks)
                taskInstances.AddRange(GroundhogContext.TaskInstanceLogic.Read(task.Id));
            List<PurposeGroup> groups = GroundhogContext.PurposeGroupLogic.Read();
            List<Purpose> purposes = new List<Purpose>();
            foreach (PurposeGroup group in groups)
                purposes.AddRange(GroundhogContext.PurposeLogic.Read(group.Id));
            List<Note> notes = GroundhogContext.NoteLogic.Read();

            StorageModel model = new StorageModel
            {
                Tasks = tasks,
                TaskInstances = taskInstances,
                PurposeGroups = groups,
                Purposes = purposes,
                Notes = notes,
                AppSettings = GroundhogContext.Settings
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
                throw new Exception($"{GroundhogContext.Language.ErrorsMessages.FailedToUploadData}: " + ex.Message);
            }
        }

        public void MakeBackup(string key)
        {
            if (!IsConnected())
                Connect(() => "");

            diskApi.Commands
                .CopyAndWaitAsync(
                    new CopyFileRequest
                    {
                        From = ConnectionString.Path,
                        Path = $@"{ConnectionString.Path}.{key}.backup",
                        Overwrite = true
                })
                .Wait();
        }

        public void RestoreBackup(string key)
        {
            if (!IsConnected())
                Connect(() => "");

            diskApi.Commands
                .CopyAndWaitAsync(
                    new CopyFileRequest
                    {
                        From = $@"{ConnectionString.Path}.{key}.backup",
                        Path = ConnectionString.Path,
                        Overwrite = true
                    })
                .Wait();
        }

        public void DeleteBackup(string key)
        {
            if (!IsConnected())
                Connect(() => "");

            diskApi.Commands.DeleteAndWaitAsync(new DeleteFileRequest { Path = $@"{ConnectionString.Path}.{key}.backup" });
        }
    }
}
