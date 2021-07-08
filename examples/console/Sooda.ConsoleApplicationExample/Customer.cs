namespace Sooda.ConsoleApplicationExample
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Data;
    using Sooda;
    using SoodaConsoleApplicationExampleStubs = Sooda.ConsoleApplicationExample.Stubs;
    
    
    public class Customer : SoodaConsoleApplicationExampleStubs.Customer_Stub
    {
        
        public Customer(SoodaConstructor c) : 
                base(c)
        {
            // Do not modify this constructor.
        }
        
        public Customer(SoodaTransaction transaction) : 
                base(transaction)
        {
            // 
            // TODO: Add construction logic here.
            // 
        }
        
        public Customer() : 
                this(SoodaTransaction.ActiveTransaction)
        {
            // Do not modify this constructor.
        }
    }
}
