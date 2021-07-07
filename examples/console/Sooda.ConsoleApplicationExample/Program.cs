using Sooda.Schema;
using System;

namespace Sooda.ConsoleApplicationExample
{
    class Program
    {
        static void Main(string[] args)
        {
            Sooda.Logging.LogManager.SetLoggingImplementation(new Sooda.Logging.ConsoleLoggingImplementation());
            Sooda.TransactionStrategyMenager.SetTransactionStrategy(new TransactionStrategy.SoodaThreadBoundTransactionStrategy());
            Sooda.Sql.SqlBuilderMenager.SetDefaultBuilder(new SqlServerCore.SqlServerBuilder());
            Sooda.Sql.SoodaDbConnectionMenager.SetConnection(new SqlServerCore.SqlServerDbConnection());
            MultiAssemblySchema.RegisterSchema(typeof(Sooda.ConsoleApplicationExample._DatabaseSchema));
            Sooda.SoodaConfig.SetConfigProvider(new EnvironmentConfigProvider());
            SoodaTransaction.DefaultObjectsAssembly = typeof(MultiAssemblySchema).Assembly;

            using (new SoodaTransaction())
            {
                foreach(var role in Role.AllQuery)
                {
                    Console.WriteLine(role.Name);
                }
            }
        }
    }
}
