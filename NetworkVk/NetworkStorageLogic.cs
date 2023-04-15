using Core;
using Core.Interfaces.Network;
using Core.Models.Storage;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using VkNet;
using VkNet.Model;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;

namespace NetworkVk
{
    public class NetworkStorageLogic : INetworkLogic
    {
        private static Regex connectionStringExpr = new Regex(@"^access_token=(?<access_token>[0-9a-f]+);group_id=(?<group_id>\d+)$");
        private static object locker = new object();

        private static int SleepTime { get => 550; }
        private static VkApi vk;

        private string currentConnectionString = null;

        public Regex ConnectionStringExpr => connectionStringExpr;
        public string ConnectionStringFormat => "access_token=ffffffff;group_id=xxxxxxxx";

        public void Connect(Func<string> getCode)
        {
            lock (locker)
            {
                try
                {
                    if (vk == null)
                        vk = new VkApi();
                    if (!vk.IsAuthorized)
                    {
                        GroupCollection groups = ConnectionStringExpr.Match(GroundhogContext.Settings.ConnectionStringStorage).Groups;
                        vk.Authorize(new ApiAuthParams
                        {
                            AccessToken = groups["access_token"].Value
                        });
                    }

                    currentConnectionString = GroundhogContext.Settings.ConnectionStringStorage;
                }
                catch (Exception ex)
                {
                    throw new Exception($"{GroundhogContext.Language.ErrorsMessages.FailedToConnect}: " + ex.Message);
                }
                finally
                {
                    Thread.Sleep(SleepTime);
                }
            }
        }

        public bool IsConnected()
        {
            return vk != null && vk.IsAuthorized && currentConnectionString == GroundhogContext.Settings.ConnectionStringStorage;
        }

        public void Load()
        {
            try
            {
                GroupCollection groups = ConnectionStringExpr.Match(currentConnectionString).Groups;

                lock (locker)
                {
                    if (vk == null)
                        throw new Exception($"{GroundhogContext.Language.ErrorsMessages.ConnectionFailed}.");

                    WallGetObject wall = null;
                    ulong offset = 0;

                    string tobeParsed = "";

                    bool downloaded = false;

                    while (!downloaded)
                    {
                        wall = vk.Wall.Get(new WallGetParams { OwnerId = -long.Parse(groups["group_id"].Value), Count = 100, Offset = offset });
                        Thread.Sleep(SleepTime);

                        if (wall.WallPosts.Count == 0)
                            throw new Exception("No data available");

                        foreach (Post post in wall.WallPosts)
                        {
                            tobeParsed += post.Text + '\n';

                            if (tobeParsed.StartsWith("=== START ===") && tobeParsed.EndsWith("=== END ===\n"))
                            {
                                downloaded = true;
                                break;
                            }

                            offset += 100;
                        }
                    }

                    tobeParsed = tobeParsed.Replace("=== START ===\n", "").Replace("\n=== END ===\n", "");

                    string[] strs = tobeParsed.Split('\n');

                    List<Task> tasks = new List<Task>();
                    List<TaskInstance> taskInstances = new List<TaskInstance>();

                    State state = State.none;

                    foreach (string s in strs)
                    {
                        if (s == "=== Tasks ===")
                        {
                            state = State.tasks;
                            continue;
                        }
                        if (s == "=== TaskInstances ===")
                        {
                            state = State.taskInstances;
                            continue;
                        }

                        switch (state)
                        {
                            case State.tasks:
                                tasks.Add(TaskSerializer.Deserialize(s));
                                break;
                            case State.taskInstances:
                                taskInstances.Add(TaskInstanceSerializer.Deserialize(s));
                                break;
                        }
                    }

                    GroundhogContext.TaskLogic.Delete(null);
                    GroundhogContext.TaskLogic.Create(tasks);
                    GroundhogContext.TaskInstanceLogic.Delete();
                    GroundhogContext.TaskInstanceLogic.Create(taskInstances);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{GroundhogContext.Language.ErrorsMessages.FailedToDownloadData}: " + ex.Message);
            }
            finally
            {
                Thread.Sleep(SleepTime);
            }
        }

        public void Upload()
        {
            try
            {
                GroupCollection groups = ConnectionStringExpr.Match(currentConnectionString).Groups;

                lock (locker)
                {
                    if (vk == null)
                        throw new Exception($"{GroundhogContext.Language.ErrorsMessages.ConnectionFailed}.");

                    List<Task> tasks = GroundhogContext.TaskLogic.Read();

                    List<TaskInstance> taskInstances = new List<TaskInstance>();
                    foreach (Task task in tasks)
                        taskInstances.AddRange(GroundhogContext.TaskInstanceLogic.Read(task.Id));

                    string str = 
                        "=== START ===\n" +
                        "=== Tasks ===\n" + 
                        TaskSerializer.SerializeList(tasks) + 
                        "=== TaskInstances ===\n" + 
                        TaskInstanceSerializer.SerializeList(taskInstances) +
                        "=== END ===";

                    List<string> strs = Trim(str, 15000);
                    strs.Reverse();
                    foreach (string s in strs)
                    {
                        vk.Wall.Post(new WallPostParams { OwnerId = -long.Parse(groups["group_id"].Value), Message = s });
                        Thread.Sleep(SleepTime);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{GroundhogContext.Language.ErrorsMessages.FailedToUploadData}: " + ex.Message);
            }
            finally
            {
                Thread.Sleep(SleepTime);
            }
        }

        private List<string> Trim(string str, int length)
        {
            List<string> answer = new List<string>();

            string[] strs = str.Split('\n');
            string current = "";
            
            foreach (string s in strs)
            {
                string next = current + s + '\n';

                if (next.Length < length)
                {
                    current = next;
                }
                else
                {
                    answer.Add(current);
                    current = s + '\n';
                }
            }

            if (current.Length > 0)
                answer.Add(current);

            return answer;
        }

        private enum State
        {
            none,
            tasks,
            taskInstances
        }
    }
}
