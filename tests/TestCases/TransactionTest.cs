//
// Copyright (c) 2014 Piotr Fusik <piotr@fusik.info>
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

using System;
using NUnit.Framework;
using Sooda.UnitTests.BaseObjects;
using System.Data;
using System.Threading;
using Sooda.Caching;

namespace Sooda.UnitTests.TestCases
{
    [TestFixture]
    public class TransactionTest
    {
        [Test]
        public void LazyDbConnection()
        {
            using (new SoodaTransaction())
            {
            }
        }

        [Test]
        public void PassDbConnection()
        {
            using (SoodaDataSource sds = _DatabaseSchema.GetSchema().GetDataSourceInfo("default").CreateDataSource())
            {
                sds.Open();
                // sds.ExecuteNonQuery(sql, params);
                using (IDataReader r = sds.ExecuteRawQuery("select count(*) from contact"))
                {
                    bool b = r.Read();
                    Assert.IsTrue(b);
                    int c = r.GetInt32(0);
                    Assert.AreEqual(7, c);
                }

                using (SoodaTransaction tran = new SoodaTransaction())
                {
                    tran.RegisterDataSource(sds);

                    int c = Contact.GetList(true).Count;
                    Assert.AreEqual(7, c);
                }
            }
        }

        [Test]
        public void ClearInTransactionCache()
        {
            const int cid = 53;
            using (SoodaDataSource sds = _DatabaseSchema.GetSchema().GetDataSourceInfo("default").CreateDataSource())
            {
                sds.Open();
                using (IDataReader r = sds.ExecuteRawQuery("select last_salary from Contact where id = {0}", cid))
                {
                    bool readOk = r.Read();
                    Assert.That(readOk, Is.EqualTo(true), "read data");

                    decimal retrivedSalary = r.GetDecimal(0);

                    Assert.That(retrivedSalary, Is.EqualTo(-1.0m), "retrived salary");
                }
            }

            try
            {
                
                const decimal newSalary = 777.0m;

                using (var t = new SoodaTransaction())
                {
                    Contact c = Contact.GetRef(cid);
                    Assert.That(c.LastSalary.Value, Is.EqualTo(-1.0m), "previous salary");

                    // simulates transaction from outer process
                    Thread nested = new Thread(() =>
                    {
                        using (var nestedTransaction = new SoodaTransaction())
                        {
                            Contact.GetRef(cid).LastSalary = newSalary;
                            Thread.Sleep(100);
                            nestedTransaction.Commit();
                            Thread.Sleep(100);
                        }
                    });

                    Console.WriteLine("External request...");
                    nested.Start();
                    nested.Join();
                    Console.WriteLine("External request... done.");

                    Assert.That(c.LastSalary.Value, Is.Not.EqualTo(newSalary), "last salary is from transaction cache");

                    Console.WriteLine("Invalidating cache...");

                    SoodaTransaction.ActiveTransaction.Rollback();

                    Console.WriteLine("Invalidating cache... done.");

                    var updatedSalary = Contact.GetRef(53).LastSalary.Value;

                    Assert.That(updatedSalary, Is.EqualTo(newSalary), "updated salary");

                }
            }
            finally
            {
                Console.WriteLine("Finally - fix value in database...");
                using (SoodaDataSource sds = _DatabaseSchema.GetSchema().GetDataSourceInfo("default").CreateDataSource())
                {
                    sds.Open();
                    sds.ExecuteNonQuery("update Contact set last_salary = {0} where id = {1}", -1.0m, 53);
                    sds.Commit();
                }
                Console.WriteLine("Finally - fix value in database... done.");
            }
        }
    }
}
