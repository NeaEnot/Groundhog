namespace StorageFile.Migrations
{
    internal static class MigrationController
    {
        private static IMigration[] migrations =
            new IMigration[]
            {
                new MigrationV6to7()
            };

        public static bool DoMigrationIfNeed()
        {
            bool answer = false;

            foreach (IMigration migration in migrations)
            {
                if (migration.CheckNeedMigration())
                {
                    migration.DoMigration();
                    answer = true;
                }
            }

            return answer;
        }
    }
}
