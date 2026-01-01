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

using NUnit.Framework;
using Sooda.UnitTests.BaseObjects;
using Sooda;
using Sooda.Schema;
using Sooda.UnitTests.Objects;

namespace Sooda.UnitTests.TestCases.ObjectMapper
{
    [TestFixture]
    public class VersioningTest
    {
        [Test]
        public void Basic()
        {
            using (new SoodaTransaction())
            {
                ContactList contacts = Contact.GetList(true, SoodaOrderBy.Ascending("ContactId"));
                Assert.AreEqual(7, contacts.Count);
                Assert.AreEqual("Mary Manager", contacts[0].GetLabel(false));
                Assert.AreEqual("Ed Employee", contacts[1].GetLabel(false));
                Assert.AreEqual("Eva Employee", contacts[2].GetLabel(false));
                Assert.AreEqual("Catie Customer", contacts[3].GetLabel(false));
                Assert.AreEqual("Caroline Customer", contacts[4].GetLabel(false));
                Assert.AreEqual("Chris Customer", contacts[5].GetLabel(false));
                Assert.AreEqual("Chuck Customer", contacts[6].GetLabel(false));
            }
        }

        [Test]
        public void BSchema()
        {
            using (new SoodaTransaction())
            {
                var sch = SoodaTransaction.ActiveTransaction.Schema;
                var ci = sch.FindClassByName("VerTestSim");
                var cf = ci.GetVersionField();
                Assert.IsNotNull(cf);
            }
                
        }

        [Test]
        public void CreateTest()
        {
            int id;
            using (var t = new SoodaTransaction())
            {
                var ss = new VerTestSim();
                ss.Name = "A1";
                id = ss.Id;
                t.Commit();
            }
            using (var t2 = new SoodaTransaction())
            {
                var v0 = VerTestSim.GetRef(id);
                v0.Name = "Updated";
                t2.Commit();
            }

        }
        [Test]
        public void CreateTest2()
        {
            int id1, id2;
            using (var t = new SoodaTransaction())
            {
                var ss = new VerTestSim();
                ss.Name = "T2-1";
                id1 = ss.Id;
                ss = new VerTestSim();
                id2 = ss.Id;
                ss.Name = "T2 - 2";
                t.Commit();
            }
            using (var t2 = new SoodaTransaction())
            {
                var c0 = Contact.GetRef(1);
                c0.LastSalary = id1 + id2;

                var v0 = VerTestSim.GetRef(id1);
                v0.Name = "T2 updated 1";

                var v1 = VerTestSim.GetRef(id2);
                v1.Name = "T2 updated 2";


                t2.Commit();
            }
        }

        [Test]
        public void CreateTest3()
        {
            int id;
            using (var t = new SoodaTransaction())
            {
                var ss = new VerTestSim();
                ss.Name = "A3";
                id = ss.Id;
                t.Commit();
            }
            using (var t2 = new SoodaTransaction())
            {
                var v0 = VerTestSim.GetRef(id);
                v0.Name = "a3 Updated";
                t2.Commit();
            }

            using (var t3 = new SoodaTransaction())
            {
                var v0 = VerTestSim.GetRef(id);
                v0.Name = "a3 Updated 2";
                t3.Commit();
                var v1 = VerTestSim.GetRef(id);
                Assert.AreSame(v0, v1);
                v1.Name = "a3 update 3";
                t3.Commit();
            }

        }


        [Test]
        public void ConcurrentUpdateT1()
        {
            int id;
            using (var t = new SoodaTransaction())
            {
                var ss = new VerTestSim();
                ss.Name = "T4cc";
                id = ss.Id;
                t.Commit();
            }
            var t2 = new SoodaTransaction();
            var v0 = VerTestSim.GetRef(t2, id);
            v0.Name = "t4 Updated 1";
            
            var t3 = new SoodaTransaction();
            var v1 = VerTestSim.GetRef(t3, id);
            v1.Name = "t4 Updated 2";
            
            t3.Commit();
            t2.Commit();
            
        }
    }
    
}
