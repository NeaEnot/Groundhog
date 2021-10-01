using Core;
using Core.Interfaces;
using Core.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using TeleSharp.TL;
using TeleSharp.TL.Channels;
using TeleSharp.TL.Contacts;
using TeleSharp.TL.Messages;
using TeleSharp.TL.Upload;
using TLSharp.Core;
using TLSharp.Core.Utils;
using Task = System.Threading.Tasks.Task;

namespace TelegramImplement.Implements
{
    public class NetworkLogic : INetworkLogic
    {
        private Context context = Context.Instanse;
        private TelegramClient client;

        private Accaunt currentAccaunt = null;
        private TLChannel channel;
        private TLChannel Channel
        {
            get
            {
                if (channel == null)
                {
                    GroupCollection groups = GroundhogContext.AccauntLogic.ConnectionStringExpr.Match(GroundhogContext.Accaunt.ConnectionString).Groups;
                    string channelName = groups["channel"].Value;

                    Task<TLResolvedPeer> task =
                        client.SendRequestAsync<TLResolvedPeer>(
                            new TLRequestResolveUsername
                            {
                                Username = channelName
                            });
                    task.Wait();

                    Thread.Sleep(5100);

                    if (task.Result.Chats.Count > 0)
                    {
                        channel = task.Result.Chats[0] as TLChannel;
                    }
                    else
                    {
                        Task<TLUpdates> createTask = client.SendRequestAsync<TLUpdates>(new TLRequestCreateChannel
                        {
                            Title = channelName,
                            About = "Частный канал для облачного хранения данных приложения Groundhog.",
                            Broadcast = false,
                            Megagroup = false
                        });

                        createTask.Wait();

                        channel = createTask.Result.Chats[0] as TLChannel;
                    }
                }

                return channel;
            }
        }

        public void Connect(Func<string> getCode)
        {
            try
            {
                GroupCollection groups = GroundhogContext.AccauntLogic.ConnectionStringExpr.Match(GroundhogContext.Accaunt.ConnectionString).Groups;

                string phone = groups["phone"].Value;
                int api_id = int.Parse(groups["api_id"].Value);
                string api_hash = groups["api_hash"].Value;

                TelegramClient client = new TelegramClient(api_id, api_hash);
                Task connection = client.ConnectAsync();
                connection.Wait();

                if (!client.IsUserAuthorized())
                {
                    Task<string> hashTask = client.SendCodeRequestAsync(phone);
                    hashTask.Wait();
                    string hash = hashTask.Result;

                    string code = getCode();

                    Task<TLUser> userTask = client.MakeAuthAsync(phone, hash, code);
                    userTask.Wait();

                    Thread.Sleep(5100);
                }

                this.client = client;
                currentAccaunt = GroundhogContext.Accaunt;
            }
            catch (Exception ex)
            {
                throw new Exception("Не получилось подключиться: " + ex.Message);
            }
        }

        public bool IsConnected()
        {
            return currentAccaunt != null && currentAccaunt.ConnectionString == GroundhogContext.Accaunt.ConnectionString;
        }

        public void Load()
        {
            try
            {
                if (client == null)
                    throw new Exception("Не было выполнено подключение.");

                Task<TLAbsMessages> msgsTask = client.GetHistoryAsync(new TLInputPeerChannel { ChannelId = Channel.Id, AccessHash = Channel.AccessHash.Value }, limit: 100);
                msgsTask.Wait();
                TLMessage msg =
                    (msgsTask.Result as TLChannelMessages)
                    .Messages
                    .FirstOrDefault(req =>
                        req is TLMessage && (req as TLMessage).Media is TLMessageMediaDocument)
                    as TLMessage;

                Thread.Sleep(5100);

                if (msg != null)
                {
                    TLDocument doc = (msg.Media as TLMessageMediaDocument).Document as TLDocument;
                    var response = client.GetFile(new TLInputDocumentFileLocation
                    {
                        Id = doc.Id,
                        AccessHash = doc.AccessHash
                    }, 1024 * 256);

                    response.Wait();

                    TLFile file = response.Result;

                    string json = System.Text.Encoding.UTF8.GetString(file.Bytes);

                    (List<Accaunt>, List<Core.Models.Task>, List<TaskInstance>) restored = JsonConvert.DeserializeObject<(List<Accaunt>, List<Core.Models.Task>, List<TaskInstance>)>(json);
                    context.Accaunts = restored.Item1;
                    context.Tasks = restored.Item2;
                    context.TaskInstances = restored.Item3;

                    context.Save();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Не удалось загрузить данные: " + ex.Message);
            }
        }

        public void Upload()
        {
            try
            {
                if (client == null)
                    throw new Exception("Не было выполнено подключение.");

                Task<TLAbsInputFile> fileTask = client.UploadFile("storage.json", new StreamReader($"{GroundhogContext.StoragePath}\\storage.json"));
                fileTask.Wait();
                TLAbsInputFile fileResult = fileTask.Result;

                Thread.Sleep(5100);

                TLDocumentAttributeFilename attr = new TLDocumentAttributeFilename { FileName = "storage.json" };

                Task<TLAbsUpdates> responseDoc = client.SendUploadedDocument(
                    new TLInputPeerChannel { ChannelId = Channel.Id, AccessHash = Channel.AccessHash.Value },
                    fileResult,
                    "",
                    "text/json",
                    new TLVector<TLAbsDocumentAttribute> { attr });
                responseDoc.Wait();

                Thread.Sleep(5100);
            }
            catch (Exception ex)
            {
                throw new Exception("Не удалось отправить данные: " + ex.Message);
            }
        }
    }
}
