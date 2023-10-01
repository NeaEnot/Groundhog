namespace Core.Models.Settings
{
    /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="BackupSettings"]/BackupSettings/*'/>
    public class BackupSettings
    {
        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="BackupSettings"]/AutoCloudBackup/*'/>
        public bool AutoCloudBackup { get; set; }
        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="BackupSettings"]/AutoLocalBackup/*'/>
        public bool AutoLocalBackup { get; set; }
    }
}
