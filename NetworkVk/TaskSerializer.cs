using Core.Enums;
using Core.Models;
using System;
using System.Collections.Generic;

namespace NetworkVk
{
    internal static class TaskSerializer
    {
        private static string separator = "~!~"; 

        public static string SerializeList(List<Task> tasks)
        {
            string answer = "";

            foreach (Task task in tasks)
                answer += Serialize(task);

            return answer;
        }

        public static string Serialize(Task task)
        {
            return $"{task.Id}{separator}{task.AccauntId}{separator}{task.Text}{separator}{task.RepeatMode}{separator}{task.RepeatValue}\n";
        }

        public static List<Task> DeserializeList(string str)
        {
            string[] strs = str.Split('\n');

            List<Task> tasks = new List<Task>();

            foreach (string s in strs)
                tasks.Add(Deserialize(s));

            return tasks;
        }

        public static Task Deserialize(string str)
        {
            string[] strs = str.Replace(separator, "\t").Split('\t');

            Task task = new Task
            {
                Id = strs[0],
                AccauntId = strs[1],
                Text = strs[2],
                RepeatMode = (RepeatMode)Enum.Parse(typeof(RepeatMode), strs[3]),
                RepeatValue = int.Parse(strs[4])
            };

            return task;
        }
    }
}
