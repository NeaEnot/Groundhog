using Core.Enums;
using System.Collections.Generic;

namespace Core.Models.Settings
{
    /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="AppSettings"]/AppSettings/*'/>
    public class AppSettings
    {
        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="AppSettings"]/ConnectionStringStorage/*'/>
        public string ConnectionStringStorage { get; set; }
        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="AppSettings"]/ConnectionStringLanguage/*'/>
        public string ConnectionStringLanguage { get; set; }

        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="AppSettings"]/PlanningRanges/*'/>
        public Dictionary<RepeatMode, int> PlanningRanges { get; set; }
        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="AppSettings"]/OptimizationRange/*'/>
        public int OptimizationRange { get; set; }

        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="AppSettings"]/ColorSchema/*'/>
        public ColorSchema ColorSchema { get; set; }
        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="AppSettings"]/BackupSettings/*'/>
        public BackupSettings BackupSettings { get; set; }
        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="AppSettings"]/Language/*'/>
        public string Language { get; set; }

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
            BackupSettings = new BackupSettings();
        }
    }
}
