using System;
using System.Reflection;

namespace Sooda.Config
{
    public class SoodaConfigProvider
    {
        public static ISoodaConfigProvider CreateNew()
        {
            ISoodaConfigProvider configProvider = null;
            try
            {
                Assembly a = Assembly.GetEntryAssembly();
                if (a != null)
                {
                    configProvider = SetConfigProviderFromAttribute((SoodaConfigAttribute)Attribute.GetCustomAttribute(a, typeof(SoodaConfigAttribute), false));
                }

                if (configProvider == null)
                {
                    try
                    {
                        string typeName = System.Configuration.ConfigurationManager.AppSettings["sooda.config"];
                        // Console.WriteLine("typeName: {0}", typeName);
                        if (typeName == "xmlconfig")
                        {
                            string xmlconfigfile = System.Configuration.ConfigurationManager.AppSettings["sooda.xmlconfigfile"];
                            if (xmlconfigfile == null)
                                xmlconfigfile = "sooda.config.xml";
                            configProvider = XmlConfigProvider.FindConfigFile(xmlconfigfile);
                        }
                        else if (typeName != null)
                        {
                            Type t = Type.GetType(typeName);
                            configProvider = Activator.CreateInstance(t) as ISoodaConfigProvider;
                        }
                    }
                    catch (Exception e)
                    {
                        throw new SoodaConfigException(String.Format("Error while loading configuration provider {0}", e));
                    }
                }
            }
            catch (Exception ex)
            {
                throw new SoodaConfigException(String.Format("Error while loading configuration provider {0}", ex));
            }
            finally
            {

                if (configProvider == null)
                {
                    configProvider = new AppSettingsConfigProvider();
                }
            }
            return configProvider;
        }

        private static ISoodaConfigProvider SetConfigProviderFromAttribute(SoodaConfigAttribute at)
        {
            if (at == null)
                return null;

            if (at.XmlConfigFileName != null)
            {
                return XmlConfigProvider.FindConfigFile(at.XmlConfigFileName);

            }
            if (at.ProviderType != null)
            {
                return Activator.CreateInstance(at.ProviderType) as ISoodaConfigProvider;               
            }

            return null;
        }
    }
}
