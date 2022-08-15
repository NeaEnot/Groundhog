using Core.Enums;
using System.Collections.Generic;

namespace Core.Models
{
    public class AppSettings
    {
        public string ConnectionString { get; set; }

        public Dictionary<RepeatMode, int> PlanningRanges { get; set; }
        public int OptimizationRange { get; set; }
        public ColorSchema ColorSchema { get; set; }

        public AppSettings()
        {
            PlanningRanges = new Dictionary<RepeatMode, int>
            {
                { RepeatMode.Нет, 0 },
                { RepeatMode.Дни, 100 },
                { RepeatMode.ЧислоМесяца, 366 },
                { RepeatMode.ДеньГода, 3660 },
                { RepeatMode.ДниНедели, 100 },
                { RepeatMode.Вахты, 100 }
            };

            OptimizationRange = 50;

            ColorSchema = new ColorSchema();
        }
    }
}
