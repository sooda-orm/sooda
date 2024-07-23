namespace Sooda.UnitTests.Objects
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Data;
    using Sooda;
    using SoodaUnitTestsObjectsStubs = Sooda.UnitTests.Objects.Stubs;
    using Sooda.UnitTests.BaseObjects;
    
    
    public class PersonConcrete : SoodaUnitTestsObjectsStubs.PersonConcrete_Stub
    {
        
        public PersonConcrete(SoodaConstructor c) : 
                base(c)
        {
            // Do not modify this constructor.
        }
        
        public PersonConcrete(SoodaTransaction transaction) : 
                base(transaction)
        {
            // 
            // TODO: Add construction logic here.
            // 
        }
        
        public PersonConcrete() : 
                this(SoodaTransaction.ActiveTransaction)
        {
            // Do not modify this constructor.
        }
    }
}
