using NUnit.Framework;
using Sooda.Schema;
using Sooda.UnitTests.TestCases.ConfigProvider;

namespace Sooda.UnitTests.TestCases
{
    /// <summary>
    /// konfiguracja testów dla bieżącej przestrzeni nazw
    /// </summary>
    [SetUpFixture]
    public class TestFixtureSetup
    {
        [OneTimeSetUp]
        public void SoodaConfig()
        {
            Sooda.Logging.LogManager.SetLoggingImplementation(new Sooda.Logging.ConsoleLoggingImplementation());
            Sooda.TransactionStrategyMenager.SetTransactionStrategy(new TransactionStrategy.SoodaThreadBoundTransactionStrategy());
            Sooda.Sql.SqlBuilderMenager.SetDefaultBuilder(new Sooda.SqlServerCore.SqlServerBuilder());
            Sooda.Sql.SoodaDbConnectionMenager.SetConnection(new Sooda.SqlServerCore.SqlServerDbConnection());
            MultiAssemblySchema.RegisterSchema(typeof(Sooda.UnitTests.BaseObjects._DatabaseSchema));
            MultiAssemblySchema.RegisterSchema(typeof(Sooda.UnitTests.Objects._DatabaseSchema));
            Sooda.SoodaConfig.SetConfigProvider(new EnvironmentConfigProvider());
            SoodaTransaction.DefaultObjectsAssembly = typeof(MultiAssemblySchema).Assembly;
        }
    }
}
