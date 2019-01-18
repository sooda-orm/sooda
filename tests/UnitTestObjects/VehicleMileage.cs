using System.Collections.Generic;
using System.Linq;

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
    
    
    public class VehicleMileage : SoodaUnitTestsObjectsStubs.VehicleMileage_Stub, IMileage
    {
        
        public VehicleMileage(SoodaConstructor c) : 
                base(c)
        {
            // Do not modify this constructor.
        }
        
        public VehicleMileage(SoodaTransaction transaction) : 
                base(transaction)
        {
            Total = 0;
        }
        
        public VehicleMileage() : 
                this(SoodaTransaction.ActiveTransaction)
        {
            // Do not modify this constructor.
        }

        public void registerMileage(int mileage)
        {
            Total += mileage;
        }

        IEnumerable<IMileageItem> IMileage.Items
        {
            get { return Items; }
        }

        IQueryable<IMileageItem> IMileage.ItemsQuery
        {
            get { return ItemsQuery; }
        }
    }
}
