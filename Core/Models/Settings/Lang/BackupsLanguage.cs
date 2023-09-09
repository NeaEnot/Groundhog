using System.Collections.Generic;

namespace Core.Models.Settings.Lang
{
    public class BackupsLanguage
    {
        public string Backup { get; set; }
        public string Cloud { get; set; }
        public string Local { get; set; }
        public string Restore { get; set; }
        public string BackupName { get; set; }
        public string AutoCloudBackup { get; set; }
        public string AutoLocalBackup { get; set; }

        internal static BackupsLanguage Parse(Dictionary<string, string> dict)
        {
            BackupsLanguage language = new BackupsLanguage
            {
                Backup = dict["Backup"],
                Cloud = dict["Cloud"],
                Local = dict["Local"],
                Restore = dict["Restore"],
                BackupName = dict["BackupName"],
                AutoCloudBackup = dict["AutoCloudBackup"],
                AutoLocalBackup = dict["AutoLocalBackup"]
            };

            return language;
        }

        internal string Serialize()
        {
            string content = "";

            content += $"# Backup" + '\n';
            content += $"Backup={Backup}" + '\n';
            content += $"Cloud={Cloud}" + '\n';
            content += $"Local={Local}" + '\n';
            content += $"Restore={Restore}" + '\n';
            content += $"Backup name={BackupName}" + '\n';
            content += $"Auto cloud backup={AutoCloudBackup}" + '\n';
            content += $"Auto local backup={AutoLocalBackup}" + '\n';
            content += '\n';

            return content;
        }
    }
}
