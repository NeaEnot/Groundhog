using System.Collections.Generic;

namespace Core.Interfaces.Network
{
    /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="IBackupLogic"]/IBackupLogic/*'/>
    public interface IBackupLogic
    {
        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="IBackupLogic"]/Backups/*'/>
        List<string> Backups { get; }

        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="IBackupLogic"]/MakeBackup/*'/>
        void MakeBackup(string key);
        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="IBackupLogic"]/RestoreBackup/*'/>
        void RestoreBackup(string key);
        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="IBackupLogic"]/DeleteBackup/*'/>
        void DeleteBackup(string key);
    }
}
