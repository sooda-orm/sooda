namespace Sooda.ConsoleApplicationExample
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Data;
    using Sooda;
    using SoodaConsoleApplicationExampleStubs = Sooda.ConsoleApplicationExample.Stubs;
    
    
    public class Role : SoodaConsoleApplicationExampleStubs.Role_Stub
    {
        
        public Role(SoodaConstructor c) : 
                base(c)
        {
            // Do not modify this constructor.
        }
        
        public Role(SoodaTransaction transaction) : 
                base(transaction)
        {
            // 
            // TODO: Add construction logic here.
            // 
        }
        
        public Role() : 
                this(SoodaTransaction.ActiveTransaction)
        {
            // Do not modify this constructor.
        }
    }
}
