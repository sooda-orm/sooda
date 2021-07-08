namespace Sooda.ConsoleApplicationExample
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Data;
    using Sooda;
    using SoodaConsoleApplicationExampleStubs = Sooda.ConsoleApplicationExample.Stubs;
    
    
    public class OrderItem : SoodaConsoleApplicationExampleStubs.OrderItem_Stub
    {
        
        public OrderItem(SoodaConstructor c) : 
                base(c)
        {
            // Do not modify this constructor.
        }
        
        public OrderItem(SoodaTransaction transaction) : 
                base(transaction)
        {
            // 
            // TODO: Add construction logic here.
            // 
        }
        
        public OrderItem() : 
                this(SoodaTransaction.ActiveTransaction)
        {
            // Do not modify this constructor.
        }
    }
}
