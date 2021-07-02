using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Sooda.Sql
{
    public interface ISoodaDbConnectionFactory
    {
        #region Create

        IDbConnection Create();
        IDbConnection Create(string connectionString);

        #endregion

        #region Open

        void Open(IDbConnection dbConnection);
        void Open(IDbConnection dbConnection, Action BeginTransactionAction);

        #endregion
    }
}
