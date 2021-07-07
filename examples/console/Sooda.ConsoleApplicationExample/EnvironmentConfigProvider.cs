using Sooda.Config;
using System;

namespace Sooda.ConsoleApplicationExample
{
    public class EnvironmentConfigProvider : ISoodaConfigProvider
    {
        public string GetString(string key)
        {
            var prefix = "SOODA_";

            var currentKey = string.Format("{0}{1}", prefix, key.Replace(".", "_"));
            var ret = Environment.GetEnvironmentVariable(currentKey);
            return ret;
        }
    }
}
