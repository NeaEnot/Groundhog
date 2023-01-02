using System.Collections.Generic;

namespace Core.Models.Settings.Lang
{
    public class TasksLanguage
    {
        public string List { get; set; }
        public string Calendar { get; set; }
        public string Tasks { get; set; }
        public string Task { get; set; }
        public string RepeatMode { get; set; }
        public string TransferTaskToNextDay { get; set; }
        public string OffsetNextTasks { get; set; }
        public string PlanningRange { get; set; }
        public string OptimizationRange { get; set; }

        internal static TasksLanguage Parse(Dictionary<string, string> dict)
        {
            TasksLanguage language = new TasksLanguage
            {
                List = dict["List"],
                Calendar = dict["Calendar"],
                Tasks = dict["Tasks"],
                Task = dict["Task"],
                RepeatMode = dict["RepeatMode"],
                TransferTaskToNextDay = dict["TransferTaskToNextDay"],
                OffsetNextTasks = dict["OffsetNextTasks"],
                PlanningRange = dict["PlanningRange"],
                OptimizationRange = dict["OptimizationRange"],
            };

            return language;
        }

        internal string Serialize()
        {
            string content = "";

            content += $"# Tasks" + '\n';
            content += $"List={List}" + '\n';
            content += $"Calendar={Calendar}" + '\n';
            content += $"Tasks={Tasks}" + '\n';
            content += $"Task={Task}" + '\n';
            content += $"RepeatMode={RepeatMode}" + '\n';
            content += $"TransferTaskToNextDay={TransferTaskToNextDay}" + '\n';
            content += $"OffsetNextTasks={OffsetNextTasks}" + '\n';
            content += $"PlanningRange={PlanningRange}" + '\n';
            content += $"OptimizationRange={OptimizationRange}" + '\n';
            content += '\n';

            return content;
        }
    }
}
