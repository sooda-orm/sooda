using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Sooda.Sql
{
    public abstract class DbConnection
    {

        #region Create

        public abstract void Create();
        public abstract void Create(string connectionString);

        #endregion

        #region Open

        public abstract void Open();
        public abstract void Open(Action BeginTransactionAction);

        #endregion

        public abstract IDbConnection Get();
    }
}
