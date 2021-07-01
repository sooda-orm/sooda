using Sooda.Sql;
using System;
using System.Data;
using System.Data.OracleClient;

namespace Sooda.Oracle
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
            _connection = new OracleConnection();
        }

        public override void Create(string connectionString)
        {
            _connection = new OracleConnection(connectionString);
        }

        public override void Open()
        {
            _connection.Open();
        }

        public override void Open(Action BeginTransactionAction)
        {
            Open();
            BeginTransactionAction();
            if (SoodaConfig.GetString("sooda.oracleClientAutoCommitBugWorkaround", "false") == "true")
            {
                // http://social.msdn.microsoft.com/forums/en-US/adodotnetdataproviders/thread/d4834ce2-482f-40ec-ad90-c3f9c9c4d4b1/
                // http://connect.microsoft.com/VisualStudio/feedback/ViewFeedback.aspx?FeedbackID=351746
                _connection.GetType().GetProperty("TransactionState", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(_connection, 1, null);
            }
        }

        public override IDbConnection Get()
        {
            return _connection;
        }
    }
}
