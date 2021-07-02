namespace Sooda.Sql
{
    public static class SoodaDbConnectionMenager
    {
        private static ISoodaDbConnectionFactory _dbConnection;

        public static void SetConnection(ISoodaDbConnectionFactory dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public static ISoodaDbConnectionFactory GetConnection()
        {
            return _dbConnection;
        }
    }
}
