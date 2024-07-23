namespace Sooda.UnitTests.BaseObjects
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Data;
    using Sooda;
    using SoodaUnitTestsBaseObjectsStubs = Sooda.UnitTests.BaseObjects.Stubs;
    
    
    public class PersonBase : SoodaUnitTestsBaseObjectsStubs.PersonBase_Stub
    {
        
        public PersonBase(SoodaConstructor c) : 
                base(c)
        {
            // Do not modify this constructor.
        }
        
        public PersonBase(SoodaTransaction transaction) : 
                base(transaction)
        {
            // 
            // TODO: Add construction logic here.
            // 
        }
        
        public PersonBase() : 
                this(SoodaTransaction.ActiveTransaction)
        {
            // Do not modify this constructor.
        }
    }
}
