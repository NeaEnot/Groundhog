using Core;
using Core.Interfaces;
using Core.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace NetworkVk
{
    public class NetworkLogic : INetworkLogic
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
                        GroupCollection groups = ConnectionStringExpr.Match(GroundhogContext.Accaunt.ConnectionString).Groups;
                        vk.Authorize(new ApiAuthParams
                        {
                            AccessToken = groups["access_token"].Value
                        });
                    }

                    currentConnectionString = GroundhogContext.Accaunt.ConnectionString;
                }
                catch (Exception ex)
                {
                    throw new Exception("Не получилось подключиться: " + ex.Message);
                }
                finally
                {
                    Thread.Sleep(SleepTime);
                }
            }
        }

        public bool IsConnected()
        {
            return vk != null && vk.IsAuthorized && currentConnectionString == GroundhogContext.Accaunt.ConnectionString;
        }

        public void Load()
        {
            try
            {
                GroupCollection groups = ConnectionStringExpr.Match(currentConnectionString).Groups;

                lock (locker)
                {
                    if (vk == null)
                        throw new Exception("Не было выполнено подключение.");

                    WallGetObject loaded = vk.Wall.Get(new WallGetParams { OwnerId = -long.Parse(groups["group_id"].Value), Count = 1 });

                    string json = loaded.WallPosts[0].Text;

                    (List<Accaunt>, List<Task>, List<TaskInstance>) restored =
                        JsonConvert.DeserializeObject<(List<Accaunt>, List<Task>, List<TaskInstance>)>(json);

                    //GroundhogContext.AccauntLogic.Delete();
                    //context.Accaunts = restored.Item1;

                    GroundhogContext.TaskLogic.Delete(null);
                    GroundhogContext.TaskLogic.Create(restored.Item2);
                    GroundhogContext.TaskInstanceLogic.Delete(null);
                    GroundhogContext.TaskInstanceLogic.Create(restored.Item3);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Не удалось загрузить данные: " + ex.Message);
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
                        throw new Exception("Не было выполнено подключение.");

                    List<Accaunt> accaunts = GroundhogContext.AccauntLogic.Read();

                    List<Task> tasks = new List<Task>();
                    foreach (Accaunt accaunt in accaunts)
                        tasks.AddRange(GroundhogContext.TaskLogic.Read(accaunt));

                    List<TaskInstance> taskInstances = new List<TaskInstance>();
                    foreach (Task task in tasks)
                        taskInstances.AddRange(GroundhogContext.TaskInstanceLogic.Read(task.Id));

                    string json = JsonConvert.SerializeObject((accaunts, tasks, taskInstances));

                    vk.Wall.Post(new WallPostParams { OwnerId = -long.Parse(groups["group_id"].Value), Message = json });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Не удалось загрузить данные: " + ex.Message);
            }
            finally
            {
                Thread.Sleep(SleepTime);
            }
        }
    }
}
