using Sooda.Sql;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Sooda.SqlServer
{
    public sealed class SqlServerDbConnection : ISoodaDbConnectionFactory
    {
        public IDbConnection Create()
        {
            var connection = new SqlConnection();
            return connection;
        }

        public IDbConnection Create(string connectionString)
        {
            var connection = new SqlConnection(connectionString);
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
