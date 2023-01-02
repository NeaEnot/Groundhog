using System.Collections.Generic;

namespace Core.Models.Settings.Lang
{
    public class PlanningAndOptimizationLanguage
    {
        public string NonePlanning { get; set; }
        public string DaysPlanning { get; set; }
        public string DaysOfWeekPlanning { get; set; }
        public string WatchesPlanning { get; set; }
        public string DaysOfMonthPlanning { get; set; }
        public string DaysOfYearPlanning { get; set; }
        public string Optimization { get; set; }

        internal static PlanningAndOptimizationLanguage Parse(Dictionary<string, string> dict)
        {
            PlanningAndOptimizationLanguage language = new PlanningAndOptimizationLanguage
            {
                NonePlanning = dict["NonePlanning"],
                DaysPlanning = dict["DaysPlanning"],
                DaysOfWeekPlanning = dict["DaysOfWeekPlanning"],
                WatchesPlanning = dict["WatchesPlanning"],
                DaysOfMonthPlanning = dict["DaysOfMonthPlanning"],
                DaysOfYearPlanning = dict["DaysOfYearPlanning"],
                Optimization = dict["Optimization"]
            };

            return language;
        }

        internal string Serialize()
        {
            string content = "";

            content += $"# Planning modes and optimization" + '\n';
            content += $"NonePlanning={NonePlanning}" + '\n';
            content += $"DaysPlanning={DaysPlanning}" + '\n';
            content += $"DaysOfWeekPlanning={DaysOfWeekPlanning}" + '\n';
            content += $"WatchesPlanning={WatchesPlanning}" + '\n';
            content += $"DaysOfMonthPlanning={DaysOfMonthPlanning}" + '\n';
            content += $"DaysOfYearPlanning={DaysOfYearPlanning}" + '\n';
            content += $"Optimization={Optimization}" + '\n';
            content += '\n';

            return content;
        }
    }
}
