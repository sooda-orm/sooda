using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sooda.Sql
{
    public static class SqlBuilderMenager
    {
        private static ISqlBuilder _sqlBuilder;

        public static void SetSqlBuilder(ISqlBuilder sqlBuilder)
        {
            _sqlBuilder = sqlBuilder;
        }

        public static ISqlBuilder GetBuilder()
        {
            return _sqlBuilder;
        }
    }
}
