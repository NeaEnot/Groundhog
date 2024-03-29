﻿using Core.Enums;
using Core.Models.Storage;
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
            return $"{task.Id}{separator}{task.Text}{separator}{task.RepeatMode}{separator}{task.RepeatValue}{separator}{task.ToNextDay}\n";
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
                Text = strs[1],
                RepeatMode = (RepeatMode)Enum.Parse(typeof(RepeatMode), strs[2]),
                RepeatValue = strs[3],
                ToNextDay = bool.Parse(strs[4]),
            };

            return task;
        }
    }
}
