using Npgsql;
using Sooda.Sql;
using System;
using System.Data;

namespace Sooda.PostgreSql
{
    public sealed class PostrgreSqlDbConnection : ISoodaDbConnectionFactory
    {
        public IDbConnection Create()
        {
            var connection = new NpgsqlConnection();
            return connection;
        }

        public IDbConnection Create(string connectionString)
        {
            var connection = new NpgsqlConnection(connectionString);
            return connection;
        }

        public void Open(IDbConnection dbConnection)
        {
            dbConnection.Open();
        }

        public void Open(IDbConnection dbConnection,Action BeginTransactionAction)
        {
            Open(dbConnection);
            BeginTransactionAction();
        }
    }
}
