using Sooda.Config;
using System;

namespace Sooda.UnitTests.TestCases.ConfigProvider
{
    public class EnvironmentConfigProvider : ISoodaConfigProvider
    {
        private readonly XmlConfigProvider _xmlConfigProvider;

        public EnvironmentConfigProvider()
            : this("Sooda.config.xml")
        {            
        }

        public EnvironmentConfigProvider(string fileName)
        {
            _xmlConfigProvider = XmlConfigProvider.FindConfigFile(fileName);
        }

        public string GetString(string key)
        {
            var prefix = "SOODA_";

            var currentKey = string.Format("{0}{1}", prefix, key.Replace(".", "_"));
            var ret = Environment.GetEnvironmentVariable(currentKey);
            if (!string.IsNullOrWhiteSpace(ret))
                return ret;

            return _xmlConfigProvider.GetString(key);
        }
    }
}
