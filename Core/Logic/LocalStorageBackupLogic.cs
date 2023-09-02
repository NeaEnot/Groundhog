using Core.Interfaces.Network;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Core.Logic
{
    public class LocalStorageBackupLogic : IBackupLogic
    {
        public List<string> Backups =>
            Directory
            .GetFiles(GroundhogContext.StoragePath)
            .Where(f => f.EndsWith(".backup"))
            .Select(f => f.Replace("cloudStorage.json", "").Replace(".backup", ""))
            .ToList();

        public void MakeBackup(string key)
        {
            File.Copy(
                $"{GroundhogContext.StoragePath}{GroundhogContext.Split}cloudStorage.json",
                $"{GroundhogContext.StoragePath}{GroundhogContext.Split}cloudStorage.json.{key}.backup",
                true);
        }

        public void RestoreBackup(string key)
        {
            File.Copy(
                $"{GroundhogContext.StoragePath}{GroundhogContext.Split}cloudStorage.json.{key}.backup",
                $"{GroundhogContext.StoragePath}{GroundhogContext.Split}cloudStorage.json",
                true);
        }
    }
}
