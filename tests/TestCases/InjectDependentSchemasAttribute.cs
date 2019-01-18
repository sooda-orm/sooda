using System;
using Sooda.Schema;

namespace Sooda.UnitTests.TestCases
{
    [AttributeUsage(AttributeTargets.Assembly)]
    internal class InjectDependentSchemasAttribute : Attribute
    {
        static InjectDependentSchemasAttribute()
        {
            try
            {
                MultiAssemblySchema.RegisterSchema(typeof(Sooda.UnitTests.BaseObjects._DatabaseSchema));
                MultiAssemblySchema.RegisterSchema(typeof(Sooda.UnitTests.Objects._DatabaseSchema));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
