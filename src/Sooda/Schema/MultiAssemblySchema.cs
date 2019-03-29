using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Sooda;
using Sooda.ObjectMapper;
using Sooda.Schema;

[assembly: SoodaObjectsAssembly(typeof(MultiAssemblySchema))]

namespace Sooda.Schema
{
    public class MultiAssemblySchema : ISoodaSchema
    {
        private static readonly object _lock = new object();

        private static ISoodaObjectFactory[] _factories = new ISoodaObjectFactory[0];
        private static IInterfaceProxyFactory[] _proxies = new IInterfaceProxyFactory[0];
        private static readonly SchemaInfo _schema = new SchemaInfo();

        public static void RegisterAssembly(Assembly assembly)
        {
            if (!assembly.IsDefined(typeof(SoodaObjectsAssemblyAttribute), false))
            {
                SoodaStubAssemblyAttribute sa = (SoodaStubAssemblyAttribute)Attribute.GetCustomAttribute(assembly, typeof(SoodaStubAssemblyAttribute), false);
                if (sa != null)
                    assembly = sa.Assembly;
            }

            SoodaObjectsAssemblyAttribute soa = (SoodaObjectsAssemblyAttribute)Attribute.GetCustomAttribute(assembly, typeof(SoodaObjectsAssemblyAttribute), false);
            if (soa == null)
            {
                throw new ArgumentException("Invalid objects assembly: " + assembly.FullName + ". Must be the stubs assembly and define assembly:SoodaObjectsAssemblyAttribute");
            }

            RegisterSchema(soa.DatabaseSchemaType);
        }

        public static void RegisterSchema(Type databaseSchemaType)
        {
            ISoodaSchema schema = Activator.CreateInstance(databaseSchemaType) as ISoodaSchema;
            if (schema == null)
                throw new ArgumentException("Invalid schema type: " + databaseSchemaType.FullName + ". Class must implementing ISoodaSchema interface.");

           MergeSchema(schema);
        }

        public static void MergeSchema(ISoodaSchema newSchema)
        {
            lock (_lock)
            {
                _schema.UnionExternalSchema(newSchema.Schema);
                _factories = _factories.Union(newSchema.GetFactories()).ToArray();
                _proxies = _proxies.Union(newSchema.GetProxies()).ToArray();
            }
        }

        public ISoodaObjectFactory[] GetFactories() { return _factories; }
        public IInterfaceProxyFactory[] GetProxies() { return _proxies; }
        public SchemaInfo Schema { get { return _schema; } }
    }
}
