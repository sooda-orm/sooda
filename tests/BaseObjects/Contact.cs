//
// Copyright (c) 2003-2006 Jaroslaw Kowalski <jaak@jkowalski.net>
// Copyright (c) 2006-2014 Piotr Fusik <piotr@fusik.info>
//
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions
// are met:
//
// * Redistributions of source code must retain the above copyright notice,
//   this list of conditions and the following disclaimer.
//
// * Redistributions in binary form must reproduce the above copyright notice,
//   this list of conditions and the following disclaimer in the documentation
//   and/or other materials provided with the distribution.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE
// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF
// THE POSSIBILITY OF SUCH DAMAGE.
//

namespace Sooda.UnitTests.BaseObjects
{
    using Sooda;
    using System;

    public interface INameAndType2
    {
        string NameAndType2
        {
            get;
        }
    }

    public class Contact : Sooda.UnitTests.BaseObjects.Stubs.Contact_Stub, INameAndType2, Sooda.UnitTests.BaseObjects.Interfaces.IContact
    {
        public bool AfterInsertCalled;

        public string PersistentValue
        {
            get { return (string)GetTransactionPersistentValue("PersistentValue"); }
            set { SetTransactionPersistentValue("PersistentValue", value); }
        }
        public Contact(SoodaConstructor c)
            :
                base(c)
        {
            // Do not modify this constructor.
        }
        public Contact(SoodaTransaction transaction)
            :
                base(transaction)
        {
            // 
            // TODO: Add construction logic here.
            // 
        }
        public Contact()
            :
                this(SoodaTransaction.ActiveTransaction)
        {
            // Do not modify this constructor.
        }

        protected override void AfterObjectInsert()
        {
            base.AfterObjectInsert();
            AfterInsertCalled = true;
        }

        public string NameAndType
        {
            get
            {
                return string.Format("{0} ({1})", Name, Type.Code);
            }
        }

#if DOTNET35
        public string NameAndType2
        {
            get
            {
                return Name + " (" + Type.Code + ")";
            }
        }

        public static System.Linq.Expressions.Expression<Func<Contact, string>> NameAndType2Expression
        {
            get
            {
                return t => t.Name + " (" + t.Type.Code + ")";
            }
        }

        public System.Linq.IQueryable<Contact> SubordinatesInCode
        {
            get
            {
                return SubordinatesQuery;
            }
        }
#endif

        public string FieldUpdateHandlers = null;

        protected override void BeforeFieldUpdate(string name, object oldVal, object newVal)
        {
            FieldUpdateHandlers += "BeforeFieldUpdate_" + name + "\n";
        }

        protected override void AfterFieldUpdate(string name, object oldVal, object newVal)
        {
            FieldUpdateHandlers += "AfterFieldUpdate_" + name + "\n";
        }
    }
}
