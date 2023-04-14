using Core;
using Core.Interfaces.Network;
using Core.Models.Storage;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using TLSchema;
using TLSchema.Channels;
using TLSchema.Messages;
using TLSchema.Upload;
using TLSharp;
using TLSharp.Utils;
using Task = System.Threading.Tasks.Task;

namespace NetworkTelegram
{
    public class NetworkStorageLogic : INetworkLogic
    {
        private static Regex connectionStringExpr = new Regex(@"^phone=(?<phone>\d{11});api_id=(?<api_id>\d+);api_hash=(?<api_hash>\w+);channel=(?<channel>.+)$");

        public Regex ConnectionStringExpr => connectionStringExpr;
        public string ConnectionStringFormat => "phone=00000000000;api_id=00000;api_hash=xx00x;channel=xxxxx";

        private int sleepTime = 5000;

        private TelegramClient client;

        private string currentConnectionString = null;
        private TLChannel channel;
        private TLChannel Channel
        {
            get
            {
                if (channel == null)
                {
                    GroupCollection groups = ConnectionStringExpr.Match(GroundhogContext.Settings.ConnectionString).Groups;
                    string channelName = groups["channel"].Value;

                    TLDialogs dialogs = null;
                    Task.Run(async () => dialogs = await client.GetUserDialogsAsync() as TLDialogs).Wait();

                    Thread.Sleep(sleepTime);

                    channel = 
                        dialogs.Chats
                        .FirstOrDefault(req => 
                            req is TLChannel && 
                            (req as TLChannel).Title != null && 
                            (req as TLChannel).Title == channelName)
                        as TLChannel;

                    if (channel == null)
                    {
                        TLRequestCreateChannel request = new TLRequestCreateChannel
                        {
                            Title = channelName,
                            About = "Private channel for cloud storage of application data Groundhog.",
                            Broadcast = false,
                            Megagroup = false
                        };

                        Task.Run(async () => channel = (await client.SendRequestAsync<TLUpdates>(request)).Chats[0] as TLChannel).Wait();
                    }
                }

                return channel;
            }
        }

        public void Connect(Func<string> getCode)
        {
            try
            {
                GroupCollection groups = ConnectionStringExpr.Match(GroundhogContext.Settings.ConnectionString).Groups;

                string phone = groups["phone"].Value;
                int api_id = int.Parse(groups["api_id"].Value);
                string api_hash = groups["api_hash"].Value;

                TelegramClient client = new TelegramClient(api_id, api_hash, sessionUserId: GroundhogContext.StoragePath + "\\session");
                Task.Run(() => client.ConnectAsync()).Wait();

                if (!client.IsUserAuthorized())
                {
                    string hash = "";
                    Task.Run(async () => hash = await client.SendCodeRequestAsync(phone)).Wait();

                    string code = getCode();

                    Task.Run(() => client.MakeAuthAsync(phone, hash, code)).Wait();

                    Thread.Sleep(sleepTime);
                }

                this.client = client;
                currentConnectionString = GroundhogContext.Settings.ConnectionString;
            }
            catch (Exception ex)
            {
                throw new Exception($"{GroundhogContext.Language.ErrorsMessages.FailedToConnect}: " + ex.Message);
            }
        }

        public bool IsConnected()
        {
            return currentConnectionString != null && currentConnectionString == GroundhogContext.Settings.ConnectionString;
        }

        public void Load()
        {
            try
            {
                if (client == null)
                    throw new Exception($"{GroundhogContext.Language.ErrorsMessages.ConnectionFailed}.");

                TLMessage msg = null;

                Task.Run(async () =>
                {
                    TLInputPeerChannel input = new TLInputPeerChannel
                    {
                        ChannelId = Channel.Id,
                        AccessHash = Channel.AccessHash.Value
                    };

                    TLChannelMessages msgs = await client.GetHistoryAsync(input, limit: 100) as TLChannelMessages;
                    msg = 
                        msgs.Messages
                        .FirstOrDefault(req => 
                            req is TLMessage && (req as TLMessage).Media is TLMessageMediaDocument) 
                        as TLMessage;
                }).Wait();

                Thread.Sleep(sleepTime);

                if (msg != null)
                {
                    TLDocument doc = (msg.Media as TLMessageMediaDocument).Document as TLDocument;
                    TLFile file = null;

                    Task.Run(async () =>
                    {
                        TLInputDocumentFileLocation input = new TLInputDocumentFileLocation
                        {
                            Id = doc.Id,
                            AccessHash = doc.AccessHash,
                            Version = doc.Version
                        };

                        file = await client.GetFile(input, 1024 * 256);
                    }).Wait();

                    string json = System.Text.Encoding.UTF8.GetString(file.Bytes);

                    (List<Core.Models.Storage.Task>, List<TaskInstance>) restored = 
                        JsonConvert.DeserializeObject<(List<Core.Models.Storage.Task>, List<TaskInstance>)>(json);

                    //GroundhogContext.AccauntLogic.Delete();
                    //context.Accaunts = restored.Item1;

                    GroundhogContext.TaskLogic.Delete(null);
                    GroundhogContext.TaskLogic.Create(restored.Item1);
                    GroundhogContext.TaskInstanceLogic.Delete();
                    GroundhogContext.TaskInstanceLogic.Create(restored.Item2);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{GroundhogContext.Language.ErrorsMessages.FailedToDownloadData}: " + ex.Message);
            }
        }

        public void Upload()
        {
            try
            {
                if (client == null)
                    throw new Exception($"{GroundhogContext.Language.ErrorsMessages.ConnectionFailed}.");

                TLAbsInputFile file = null;

                Task.Run(async () =>
                {
                    file = await client.UploadFile("storage.json", new StreamReader($"{GroundhogContext.StoragePath}\\storage.json"));
                }).Wait();

                Thread.Sleep(sleepTime);

                TLDocumentAttributeFilename attr = new TLDocumentAttributeFilename { FileName = "storage.json" };

                Task.Run(() =>
                    client.SendUploadedDocument(
                        new TLInputPeerChannel 
                        { 
                            ChannelId = Channel.Id, 
                            AccessHash = Channel.AccessHash.Value
                        },
                        file,
                        "",
                        "text/json",
                        new TLVector<TLAbsDocumentAttribute> { attr })
                ).Wait();

                Thread.Sleep(sleepTime);
            }
            catch (Exception ex)
            {
                throw new Exception($"{GroundhogContext.Language.ErrorsMessages.FailedToUploadData}: " + ex.Message);
            }
        }
    }
}
