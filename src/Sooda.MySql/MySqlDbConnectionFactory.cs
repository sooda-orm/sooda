using MySql.Data.MySqlClient;
using Sooda.Sql;
using System;
using System.Data;

namespace Sooda.MySql
{
    public sealed class MySqlDbConnectionFactory : ISoodaDbConnectionFactory
    {
        public IDbConnection Create()
        {
            var connection = new MySqlConnection();
            return connection;
        }

        public IDbConnection Create(string connectionString)
        {
            var connection = new MySqlConnection(connectionString);
            return connection;
        }

        public void Open(IDbConnection dbConnection)
        {
            dbConnection.Open();
        }

        public void Open(IDbConnection dbConnection, Action BeginTransactionAction)
        {
            Open(dbConnection);
            BeginTransactionAction();
        }
    }
}
