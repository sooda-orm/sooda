namespace Sooda.UnitTests.Objects
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Data;
    using Sooda;
    using SoodaUnitTestsObjectsStubs = Sooda.UnitTests.Objects.Stubs;
    using Sooda.UnitTests.BaseObjects;
    
    
    public class EightFieldsEx : SoodaUnitTestsObjectsStubs.EightFieldsEx_Stub
    {
        
        public EightFieldsEx(SoodaConstructor c) : 
                base(c)
        {
            // Do not modify this constructor.
        }
        
        public EightFieldsEx(SoodaTransaction transaction) : 
                base(transaction)
        {
            // 
            // TODO: Add construction logic here.
            // 
        }
        
        public EightFieldsEx() : 
                this(SoodaTransaction.ActiveTransaction)
        {
            // Do not modify this constructor.
        }
    }
}
