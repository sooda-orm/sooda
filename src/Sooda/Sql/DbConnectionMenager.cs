namespace Sooda.Sql
{
    public static class DbConnectionMenager
    {
        private static DbConnection _dbConnection;

        public static void SetConnection(DbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public static DbConnection GetConnection()
        {
            return _dbConnection;
        }
    }
}
