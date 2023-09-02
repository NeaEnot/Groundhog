using System.Collections.Generic;

namespace Core.Interfaces.Network
{
    public interface IBackupLogic
    {
        List<string> Backups { get; }

        void MakeBackup(string key);
        void RestoreBackup(string key);
    }
}
