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
using Sooda.Linq;
using Sooda.UnitTests.BaseObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Sooda.UnitTests.TestCases.Linq
{
    [TestFixture]
    public class CustomExpressionTest
    {
        public static string Double(string s)
        {
            return s + s;
        }

        public static Expression<Func<string, string>> DoubleExpression()
        {
            return s => s + s;
        }

        [Test]
        public void Double()
        {
            using (new SoodaTransaction())
            {
                IEnumerable<Contact> ce = Contact.Linq().Where(c => Double(c.Name) == "Mary ManagerMary Manager");
                CollectionAssert.AreEqual(new Contact[] { Contact.Mary }, ce);

                ce = Contact.Linq().Where(c => Double(c.Name) == "Mary Manager");
                CollectionAssert.IsEmpty(ce);
            }
        }

        [Test]
        public void DoubleNested()
        {
            using (new SoodaTransaction())
            {
                IEnumerable<Contact> ce = Contact.Linq().Where(c => Double(Double(c.Name)) == "Mary ManagerMary ManagerMary ManagerMary Manager");
                CollectionAssert.AreEqual(new Contact[] { Contact.Mary }, ce);
            }
        }

        public static object CarOwner(Car car)
        {
            throw new Exception("should use CarOwnerExpression instead of CarOwner function");
        }

        public static Expression<Func<Car, object>> CarOwnerExpression(Car dummyCar)
        {
            return car => new { Car = car.Name.Value, Owner = car.Owner.Name };
        }

        public static string ContactLabel(Contact c)
        {
            throw new Exception("should use ContactLabelExpression instead of ContactLabel function");
        }

        public static Expression<Func<Contact, string>>  ContactLabelExpression(Contact dummyContact)
        {
            return c => string.Format("+{0}+", c.Name);
        }

        public static string ContactLabelInSql(Contact c)
        {
            throw new Exception("should use ContactLabelInSqlExpression instead of ContactLabelInSql function");
        }

        public static Expression<Func<Contact, string>> ContactLabelInSqlExpression(Contact dummyContact)
        {
            return c =>"+" + c.Name + "+";
        }

        [Test]
        public void DirectNewObject()
        {
            using (new SoodaTransaction())
            {
                var c = Car.Linq().Select(car => new { Car = car.Name.Value, Owner = car.Owner.Name });
                CollectionAssert.AreEquivalent(new object[] { new { Car = "some vehicle", Owner = (string)null }, new { Car = "a car", Owner = "Mary Manager" } }, c);
            }
        }

        [Test]
        public void IndirectNewObject()
        {
            using (new SoodaTransaction())
            {
                var c = Car.Linq().Select(car => CarOwner(car));
                CollectionAssert.AreEquivalent(new object[] { new { Car = "some vehicle", Owner = (string)null }, new { Car = "a car", Owner = "Mary Manager" } }, c);
            }
        }

        [Test]
        public void DirectDotNetFunction()
        {
            using (new SoodaTransaction())
            {
                var c = Vehicle.Linq().Where(vehicle => vehicle.Owner != null).Select(vehicle => string.Format("+{0}+", vehicle.Owner.Name));
                CollectionAssert.AreEquivalent(new [] { "+Mary Manager+", "+Ed Employee+" }, c);
            }
        }

        [Test]
        public void IndirectDotNetFunction()
        {
            using (new SoodaTransaction())
            {
                var c = Vehicle.Linq().Where(vehicle => vehicle.Owner != null).Select(vehicle => ContactLabel(vehicle.Owner));
                CollectionAssert.AreEquivalent(new[] { "+Mary Manager+", "+Ed Employee+" }, c);
            }
        }

        [Test]
        public void IndirectInSqlFunction()
        {
            using (new SoodaTransaction())
            {
                var c = Vehicle.Linq().Where(vehicle => vehicle.Owner != null).Select(vehicle => ContactLabelInSql(vehicle.Owner));
                CollectionAssert.AreEquivalent(new[] { "+Mary Manager+", "+Ed Employee+" }, c);
            }
        }

        [Test]
        public void IndirectNestedPropertyAndFunction()
        {
            using (new SoodaTransaction())
            {
                var c = Car.Linq().Where(car => car.Owner != null).Select(vehicle => ContactLabelInSql(vehicle.CurrentOwner));
                CollectionAssert.AreEquivalent(new[] { "+Mary Manager+" }, c);
            }
        }

        public static double CircleArea(double r)
        {
            return Math.PI * r * r;
        }

        public static Expression<Func<double, double>> CircleAreaExpression(double dummy)
        {
            return r => Math.PI * r * r;
        }

        [Test]
        public void CircleArea()
        {
            using (new SoodaTransaction())
            {
                IEnumerable<Contact> ce = Contact.Linq().Where(c => CircleArea(c.ContactId) < 4);
                CollectionAssert.AreEqual(new Contact[] { Contact.Mary }, ce);
            }
        }

        [Test]
        public void SoodaObjectProperty()
        {
            using (new SoodaTransaction())
            {
                IEnumerable<Contact> ce = Contact.Linq().Where(c => new string[] { "Mary Manager (Manager)", "Eva Employee (Employee)" }.Contains(c.NameAndType2));
                CollectionAssert.AreEquivalent(new Contact[] { Contact.Mary, Contact.Eva }, ce);
            }
        }

        public static Expression<Func<Contact, ContactType, bool>> TypeIsExpression(Contact dummyC, ContactType dummyT)
        {
            return (c, t) => c.Type == t;
        }

        public static bool TypeIs(Contact c, ContactType t)
        {
            return c.Type == t;
        }

        [Test]
        public void TypeIs()
        {
            using (new SoodaTransaction())
            {
                IEnumerable<ContactType> te = ContactType.Linq().Where(t => Contact.Linq().Any(c => TypeIs(c, t) && c.LastSalary.Value == 345));
                CollectionAssert.AreEquivalent(new ContactType[] { ContactType.Employee }, te);
            }
        }

        [Test]
        public void BaseProperty()
        {
            using (new SoodaTransaction())
            {
                IEnumerable<Car> ce = Car.Linq().Where(c => c.BaseClassName == "Vehicle");
                CollectionAssert.AreEquivalent(Car.Linq(), ce);
                ce = Car.Linq().Where(c => c.BaseClassName != "Vehicle");
                CollectionAssert.IsEmpty(ce);
            }
        }

        [Test]
        public void BaseMethod()
        {
            using (new SoodaTransaction())
            {
                IEnumerable<Car> ce = Car.Linq().Where(c => c.GetBaseClassName() == "Vehicle");
                CollectionAssert.AreEquivalent(Car.Linq(), ce);
                ce = Car.Linq().Where(c => c.GetBaseClassName() != "Vehicle");
                CollectionAssert.IsEmpty(ce);
            }
        }

        [Test]
        public void InterfaceProperty()
        {
            using (new SoodaTransaction())
            {
                ParameterExpression pe = Expression.Parameter(typeof(Contact));
                IEnumerable<Contact> ce = Contact.Linq().Where(Expression.Lambda<Func<Contact, bool>>(
                    Expression.Equal(
                        Expression.Property(pe, typeof(INameAndType2).GetProperty("NameAndType2")),
                        Expression.Constant("Mary Manager (Manager)")),
                    pe));
                CollectionAssert.AreEqual(new Contact[] { Contact.Mary }, ce);
            }
        }

        [Test]
        public void InterfaceMethod()
        {
            using (new SoodaTransaction())
            {
                ParameterExpression pe = Expression.Parameter(typeof(Car));
                IEnumerable<Car> ce = Car.Linq().Where(Expression.Lambda<Func<Car, bool>>(
                    Expression.Equal(
                        Expression.Call(pe, typeof(IBaseClassName).GetMethod("GetBaseClassName")),
                        Expression.Constant("Vehicle")),
                    pe));
                CollectionAssert.AreEquivalent(Car.Linq(), ce);
            }
        }
    }
}
