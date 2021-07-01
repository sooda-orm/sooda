using System;
using System.Collections.Generic;
using System.Data;

namespace Sooda.Sql
{
    public static class SqlBuilderMenager
    {
        private static ISqlBuilder _defaultSqlBuilder;
        private static readonly Dictionary<string, ISqlBuilder> _builders;

        static SqlBuilderMenager()
        {
            _builders = new Dictionary<string, ISqlBuilder>();
        }

        public static void SetDefaultBuilder(ISqlBuilder sqlBuilder)
        {
            _defaultSqlBuilder = sqlBuilder;
        }

        public static void SetBuilder(ISqlBuilder sqlBuilder, string name)
        {
            _builders.Add(name, sqlBuilder);
        }

        public static ISqlBuilder GetDefaultBuilder()
        {
            return _defaultSqlBuilder;
        }

        public static ISqlBuilder GetBuilder(string name)
        {
            return _builders.TryGetValue(name, out ISqlBuilder builder) ? builder : throw new System.Exception($"Builder {name} not found.");
        }
    }
}
