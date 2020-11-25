using NUnit.Framework;
using Sooda.Schema;
using Sooda.UnitTests.TestCases.ConfigProvider;

namespace Sooda.UnitTests.TestCases.Caching
{
    [SetUpFixture]
    public class TestFixtureSetup
    {
        [OneTimeSetUp]
        public void SoodaConfig()
        {
            Sooda.Logging.LogManager.SetLoggingImplementation(new Sooda.Logging.ConsoleLoggingImplementation());
            MultiAssemblySchema.RegisterSchema(typeof(Sooda.UnitTests.BaseObjects._DatabaseSchema));
            MultiAssemblySchema.RegisterSchema(typeof(Sooda.UnitTests.Objects._DatabaseSchema));
            Sooda.SoodaConfig.SetConfigProvider(new EnvironmentConfigProvider());
            SoodaTransaction.DefaultObjectsAssembly = typeof(MultiAssemblySchema).Assembly;
        }
    }
}
