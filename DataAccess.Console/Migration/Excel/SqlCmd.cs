namespace DataAccess.Console.Migration.Excel
{
    public static class SqlCmd
    {
        public static string Reseed(string tableName, int seed)
        {
            return string.Format("dbcc checkident('{0}', reseed, {1});", tableName, seed);
        }

        public static string EmptyTable(string tableName)
        {
            return string.Format("delete from {0};", tableName);
        }
    }
}
