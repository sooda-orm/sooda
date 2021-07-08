namespace Sooda.ConsoleApplicationExample
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Data;
    using Sooda;
    using SoodaConsoleApplicationExampleStubs = Sooda.ConsoleApplicationExample.Stubs;
    
    
    public class Employee : SoodaConsoleApplicationExampleStubs.Employee_Stub
    {
        
        public Employee(SoodaConstructor c) : 
                base(c)
        {
            // Do not modify this constructor.
        }
        
        public Employee(SoodaTransaction transaction) : 
                base(transaction)
        {
            // 
            // TODO: Add construction logic here.
            // 
        }
        
        public Employee() : 
                this(SoodaTransaction.ActiveTransaction)
        {
            // Do not modify this constructor.
        }
    }
}
