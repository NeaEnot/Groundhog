namespace StorageFile.Migrations
{
    internal static class MigrationController
    {
        private static IMigration[] migrations =
            new IMigration[]
            {
                new MigrationV6to7()
            };

        public static void DoMigrationIfNeed()
        {
            foreach (IMigration migration in migrations)
                if (migration.CheckNeedMigration())
                    migration.DoMigration();
        }
    }
}
