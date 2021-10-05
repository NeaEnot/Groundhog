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
using TLSchema;
using TLSchema.Channels;
using TLSchema.Messages;
using TLSchema.Upload;
using TLSharp;
using TLSharp.Utils;
using Task = System.Threading.Tasks.Task;

namespace TelegramImplement.Implements
{
    public class NetworkLogic : INetworkLogic
    {
        private int sleepTime = 1000;

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
                            About = "Частный канал для облачного хранения данных приложения Groundhog.",
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
                GroupCollection groups = GroundhogContext.AccauntLogic.ConnectionStringExpr.Match(GroundhogContext.Accaunt.ConnectionString).Groups;

                string phone = groups["phone"].Value;
                int api_id = int.Parse(groups["api_id"].Value);
                string api_hash = groups["api_hash"].Value;

                TelegramClient client = new TelegramClient(api_id, api_hash);
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
                            AccessHash = doc.AccessHash
                        };

                        file = await client.GetFile(input, 1024 * 256);
                    }).Wait();

                    string json = System.Text.Encoding.UTF8.GetString(file.Bytes);

                    (List<Accaunt>, List<Core.Models.Task>, List<TaskInstance>) restored = 
                        JsonConvert.DeserializeObject<(List<Accaunt>, List<Core.Models.Task>, List<TaskInstance>)>(json);

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
                throw new Exception("Не удалось отправить данные: " + ex.Message);
            }
        }
    }
}
