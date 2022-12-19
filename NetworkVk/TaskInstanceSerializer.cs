using Core.Models.Storage;
using System;
using System.Collections.Generic;

namespace NetworkVk
{
    internal static class TaskInstanceSerializer
    {
        private static string separator = "~!~";

        public static string SerializeList(List<TaskInstance> instances)
        {
            string answer = "";

            foreach (TaskInstance instance in instances)
                answer += Serialize(instance);

            return answer;
        }

        public static string Serialize(TaskInstance instance)
        {
            return $"{instance.Id}{separator}{instance.Date}{separator}{instance.TaskId}{separator}{instance.Completed}\n";
        }

        public static List<TaskInstance> DeserializeList(string str)
        {
            string[] strs = str.Split('\n');

            List<TaskInstance> instances = new List<TaskInstance>();

            foreach (string s in strs)
                instances.Add(Deserialize(s));

            return instances;
        }

        public static TaskInstance Deserialize(string str)
        {
            string[] strs = str.Replace(separator, "\t").Split('\t');

            TaskInstance instance = new TaskInstance
            {
                Id = strs[0],
                Date = DateTime.Parse(strs[1]),
                TaskId = strs[2],
                Completed = bool.Parse(strs[3])
            };

            return instance;
        }
    }
}
