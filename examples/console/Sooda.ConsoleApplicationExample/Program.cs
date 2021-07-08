using Sooda.Schema;
using System;
using System.Linq;

namespace Sooda.ConsoleApplicationExample
{
    class Program
    {
        static void Run()
        {
            var next = true;
            do
            {
                Console.WriteLine();
                Console.WriteLine("1. Product category");
                //Console.WriteLine("2. Customer");
                //Console.WriteLine("3. Employee");
                Console.WriteLine("----------");
                Console.WriteLine("0. Quit");

                Console.Write(Environment.NewLine + "Choose option: ");
                var number = Console.ReadLine();
                if(int.TryParse(number, out int option))
                {
                    switch (option)
                    {
                        case 0:
                            next = false;
                            break;
                        case 1:
                            Category.Menu();
                            break;
                        default:
                            Console.WriteLine("Invalid operation");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid operation");
                }
            } while (next);
        }

        static void Main(string[] args)
        {
            Sooda.Logging.LogManager.SetLoggingImplementation(new Sooda.Logging.NullLoggingImplementation());
            Sooda.TransactionStrategyMenager.SetTransactionStrategy(new TransactionStrategy.SoodaThreadBoundTransactionStrategy());
            Sooda.Sql.SqlBuilderMenager.SetDefaultBuilder(new SqlServerCore.SqlServerBuilder());
            Sooda.Sql.SoodaDbConnectionMenager.SetConnection(new SqlServerCore.SqlServerDbConnection());
            MultiAssemblySchema.RegisterSchema(typeof(Sooda.ConsoleApplicationExample._DatabaseSchema));
            Sooda.SoodaConfig.SetConfigProvider(new EnvironmentConfigProvider());
            Console.WriteLine(SoodaConfig.GetString("default.connectionString"));
            SoodaTransaction.DefaultObjectsAssembly = typeof(MultiAssemblySchema).Assembly;

            Run();
        }
    }
}
