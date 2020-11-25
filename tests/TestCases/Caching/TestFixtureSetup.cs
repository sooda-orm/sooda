using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Sooda.Schema;

namespace Sooda.UnitTests.TestCases.Caching
{
    [SetUpFixture]
    public class TestFixtureSetup
    {
        [OneTimeSetUp]
        public void SoodaConfig()
        {
            MultiAssemblySchema.RegisterSchema(typeof(Sooda.UnitTests.BaseObjects._DatabaseSchema));
            MultiAssemblySchema.RegisterSchema(typeof(Sooda.UnitTests.Objects._DatabaseSchema));
            SoodaTransaction.DefaultObjectsAssembly = typeof(MultiAssemblySchema).Assembly;
        }
    }
}
