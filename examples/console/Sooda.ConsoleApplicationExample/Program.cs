using Sooda.Config;
using Sooda.Schema;
using System;
using System.Collections.Generic;

namespace Sooda.ConsoleApplicationExample
{
    class Program
    {
        static void Main(string[] args)
        {
            Prepare();
            using (new SoodaTransaction())
            {
                
            }
        }

        static void Prepare()
        {
            Sooda.Logging.LogManager.SetLoggingImplementation(new Sooda.Logging.ConsoleLoggingImplementation());
            Sooda.TransactionStrategyMenager.SetTransactionStrategy(new TransactionStrategy.SoodaThreadBoundTransactionStrategy());
            Console.WriteLine("Transaction strategy has been set: {0}", nameof(TransactionStrategy.SoodaThreadBoundTransactionStrategy));
            Sooda.Sql.SqlBuilderMenager.SetDefaultBuilder(new SqlServerCore.SqlServerBuilder());
            Console.WriteLine("SQL builder has been set: {0}", nameof(SqlServerCore.SqlServerBuilder));
            Sooda.Sql.SoodaDbConnectionMenager.SetConnection(new Sooda.SqlServerCore.SqlServerDbConnection());
            Console.WriteLine("DB Connection has been set: {0}", nameof(SqlServerCore.SqlServerDbConnection));
            //MultiAssemblySchema.RegisterSchema(typeof(Sooda.UnitTests.BaseObjects._DatabaseSchema));
            //Console.WriteLine("Schema has been registred: {0}", nameof(Sooda.UnitTests.BaseObjects._DatabaseSchema));
            //Sooda.SoodaConfig.SetConfigProvider(new XmlConfigProvider());
            SoodaTransaction.DefaultObjectsAssembly = typeof(MultiAssemblySchema).Assembly;
        }
    }
}
