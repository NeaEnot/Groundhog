using Core.Enums;
using System.Collections.Generic;

namespace Core.Models.Settings
{
    public class AppSettings
    {
        public string ConnectionString { get; set; }

        public Dictionary<RepeatMode, int> PlanningRanges { get; set; }
        public int OptimizationRange { get; set; }
        public ColorSchema ColorSchema { get; set; }
        public Language Language { get; set; }

        public AppSettings()
        {
            PlanningRanges = new Dictionary<RepeatMode, int>
            {
                { RepeatMode.None, 0 },
                { RepeatMode.Days, 100 },
                { RepeatMode.DayOfMonth, 366 },
                { RepeatMode.DayOfYear, 3660 },
                { RepeatMode.DaysOfWeek, 100 },
                { RepeatMode.Wathes, 100 }
            };

            OptimizationRange = 50;

            ColorSchema = new ColorSchema();
        }


    }
}
