namespace Sooda.UnitTests.Objects
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Data;
    using Sooda;
    using SoodaUnitTestsObjectsStubs = Sooda.UnitTests.Objects.Stubs;
    using Sooda.UnitTests.BaseObjects;
    using Sooda.UnitTests.BaseObjects.Interfaces;
    
    
    public class VerTestSim : SoodaUnitTestsObjectsStubs.VerTestSim_Stub
    {
        
        public VerTestSim(SoodaConstructor c) : 
                base(c)
        {
            // Do not modify this constructor.
        }
        
        public VerTestSim(SoodaTransaction transaction) : 
                base(transaction)
        {
            Version = 0;
            LastModified = DateTime.Now;
        }
        
        public VerTestSim() : 
                this(SoodaTransaction.ActiveTransaction)
        {
            // Do not modify this constructor.
        }
    }
}
