using Sooda.Sql;
using System;
using System.Data;
using System.Data.OracleClient;

namespace Sooda.Oracle
{
    public sealed class OracleDbConnectionFactory : ISoodaDbConnectionFactory
    {
        public IDbConnection Create()
        {
            var connection = new OracleConnection();
            return connection;
        }

        public IDbConnection Create(string connectionString)
        {
            var connection = new OracleConnection(connectionString);
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
            if (SoodaConfig.GetString("sooda.oracleClientAutoCommitBugWorkaround", "false") == "true")
            {
                // http://social.msdn.microsoft.com/forums/en-US/adodotnetdataproviders/thread/d4834ce2-482f-40ec-ad90-c3f9c9c4d4b1/
                // http://connect.microsoft.com/VisualStudio/feedback/ViewFeedback.aspx?FeedbackID=351746
                dbConnection.GetType().GetProperty("TransactionState", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(dbConnection, 1, null);
            }
        }
    }
}
