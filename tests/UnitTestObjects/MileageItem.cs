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
    
    
    public class MileageItem : SoodaUnitTestsObjectsStubs.MileageItem_Stub, IMileageItem
    {
        
        public MileageItem(SoodaConstructor c) : 
                base(c)
        {
            // Do not modify this constructor.
        }
        
        public MileageItem(SoodaTransaction transaction) : 
                base(transaction)
        {
            // 
            // TODO: Add construction logic here.
            // 
        }
        
        public MileageItem() : 
                this(SoodaTransaction.ActiveTransaction)
        {
            // Do not modify this constructor.
        }
    }
}
