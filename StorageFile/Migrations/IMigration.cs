namespace StorageFile.Migrations
{
    internal interface IMigration
    {
        void DoMigration();
        bool CheckNeedMigration();
    }
}
