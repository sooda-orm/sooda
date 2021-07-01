using Sooda.Sql;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Sooda.SqlServer
{
    public sealed class DbConnectionWrapper : DbConnection
    {
        private IDbConnection _connection;

        public DbConnectionWrapper(IDbConnection connection)
        {
            _connection = connection;
        }

        public DbConnectionWrapper()
        {

        }

        public override void Create()
        {
            _connection = new SqlConnection();
        }

        public override void Create(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
        }

        public override void Open()
        {
            _connection.Open();
        }

        public override void Open(Action BeginTransactionAction)
        {
            Open();
            BeginTransactionAction();
        }

        public override IDbConnection Get()
        {
            return _connection;
        }
    }
}
