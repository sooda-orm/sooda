//
// Copyright (c) 2012-2014 Piotr Fusik <piotr@fusik.info>
// Copyright (c) 2019 Maciej Nowak
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

#if DOTNET35

using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Sooda.UnitTests.BaseObjects;
using Sooda.UnitTests.BaseObjects.Interfaces;
using Sooda.UnitTests.BaseObjects.Stubs;
using Sooda.UnitTests.Objects;

namespace Sooda.UnitTests.TestCases.DependencyInversion
{
    [TestFixture]
    public class ModelDependencyInjectionTest
    {
        [Test]
        public void Reference()
        {
            using (new SoodaTransaction())
            {
                Vehicle v = Vehicle.Load(2);
                Assert.That(v.Driver, Is.InstanceOf<IContact>());
                Assert.That(v.Driver, Is.TypeOf<Contact>());
                Assert.That(v.Driver, Is.EqualTo(Contact.Mary));
            }
        }

        [Test]
        public void AccessToChildPropertyAfterLoad()
        {
            using (new SoodaTransaction(typeof(Contact).Assembly))
            {
                Vehicle v = Vehicle.Load(2);
                Assert.That(v.Driver.Name, Is.EqualTo("Mary Manager"));
            }
        }

        [Test]
        public void AccessToChildPropertyAfterLinqFirst()
        {
            using (new SoodaTransaction(typeof(Contact).Assembly))
            {
                Vehicle v = Vehicle.Linq().First(it => it.Id == 2);
                Assert.That(v.Driver.Name, Is.EqualTo("Mary Manager"));
            }
        }

        [Test]
        public void FilterObjectsViaInterface()
        {
            using (new SoodaTransaction(typeof(Contact).Assembly))
            {
                Vehicle v = Vehicle.Linq().FirstOrDefault(it => it.Driver.Name == "Mary Manager");
                Assert.That(v, Is.Not.Null);
            }
        }

        [Test]
        public void ListOfInterfacedObjects()
        {
            using (new SoodaTransaction(typeof(Contact).Assembly))
            {
                var list = ContactRepository.Linq().ToList();
                Assert.That(list.Count, Is.EqualTo(7));
                Assert.That(list[0].Name, Is.EqualTo("Mary Manager"));
                Assert.That(list[0], Is.EqualTo(Contact.Mary));
            }
        }

        [Test]
        public void FilteredListOfInterfacedObjects()
        {
            using (new SoodaTransaction(typeof(Contact).Assembly))
            {
                var list = ContactRepository.Linq().Where(it => it.Name.Contains("Employee")).ToList();
                Assert.That(list.Count, Is.EqualTo(2));
                Assert.That(list[0].Name, Is.EqualTo("Ed Employee"));
                Assert.That(list[0], Is.EqualTo(Contact.Ed));
            }
        }

        [Test]
        public void BaseAssemblyVehicleMileageNotExisting()
        {
            using (new SoodaTransaction(typeof(Contact).Assembly))
            {
                var v = Vehicle.Load(3);
                Assert.That(v.Mileage, Is.Null);
            }
        }

        [Test]
        public void BaseObjectsAssemblyVehicleMileageExisting()
        {
            using (new SoodaTransaction(typeof(BaseObjects.Contact).Assembly))
            {
                var v = Vehicle.Load(2);
                Assert.Throws<InvalidOperationException>(() =>
                {
                    IMileage m = v.Mileage;
                });
            }
        }

        [Test]
        public void ObjectsAssemblyVehicleMileageExisting()
        {
            using (new SoodaTransaction(typeof(Objects.MegaSuperBike).Assembly))
            {
                var v = Vehicle.Load(2);
                Assert.Throws<InvalidOperationException>(() =>
                {
                    IMileage m = v.Mileage;
                });
            }
        }

        [Test]
        public void UnitedAssemblyVehicleMileageExisting()
        {
            using (new SoodaTransaction())
            {
                var v = Vehicle.Load(2);
                IMileage m = null;
                Assert.DoesNotThrow(() => { m = v.Mileage; });
                Assert.That(m, Is.EqualTo(VehicleMileage.GetRef(2)));
            }
        }

        [Test]
        public void SelectCountWhere()
        {
            using (new SoodaTransaction())
            {
                int c = Vehicle.Linq().Count(v => v.Mileage.Total > 50);
                Assert.That(c, Is.EqualTo(2));

                c = Vehicle.Linq().Count(v => v.Mileage.Total < 50);
                Assert.That(c, Is.EqualTo(6));

                c = Vehicle.Linq().Count(v => v.Mileage == null);
                Assert.That(c, Is.EqualTo(1));
            }
        }

        [Test]
        public void RepositorAll()
        {
            using (new SoodaTransaction())
            {
                List<IMileage> list = MileageRepository.Linq().ToList();

                Assert.That(list.Count, Is.EqualTo(8));
                Assert.That(list[0], Is.TypeOf<VehicleMileage>());
            }
        }

        [Test]
        public void RepositoryWhere()
        {
            using (new SoodaTransaction())
            {
                List<IMileage> list = MileageRepository.Linq().Where(m => m.Total > 100).ToList();

                Assert.That(list.Count, Is.EqualTo(1));
                Assert.That(list[0].Total, Is.EqualTo(113));
            }
        }

        [Test]
        public void RepositorySum()
        {
            using (new SoodaTransaction())
            {
                int sum = MileageRepository.Linq().Sum(it => it.Total);
                Assert.That(sum, Is.EqualTo(190));
            }
        }

        [Test]
        public void DependentSubqueryAny()
        {
            using (new SoodaTransaction())
            {
                IList<IMileage> list = MileageRepository.Linq().Where(m => m.Items.Any(mi => mi.Miles > 55)).ToList();
                Assert.That(list.Count, Is.EqualTo(1));
                Assert.That(list[0].Total, Is.EqualTo(77));

                Assert.That(list[0].ItemsQuery.Count(), Is.EqualTo(2));
                Assert.That(list[0].ItemsQuery.Max(i => i.Miles), Is.EqualTo(56));
                Assert.That(list[0].ItemsQuery.Any(i => i.Miles < 70), Is.EqualTo(true));

                Assert.That(list[0].Items.Count(), Is.EqualTo(2));
                Assert.That(list[0].Items.Max(i => i.Miles), Is.EqualTo(56));
                Assert.That(list[0].Items.Any(i => i.Miles < 70), Is.EqualTo(true));
            }
        }

        [Test]
        public void WorkflowOnInterface()
        {
            using (new SoodaTransaction())
            {
                Vehicle v = Vehicle.GetRef(4);
                Assert.That(v.Mileage.Total, Is.EqualTo(113));

                v.Mileage.registerMileage(27);

                Assert.That(v.Mileage.Total, Is.EqualTo(140));

                IList<Vehicle> list = Vehicle.Linq().Where(it => it.Mileage.Total > 100).ToList();

                Assert.That(list.Count, Is.EqualTo(1));
                Assert.That(list[0], Is.EqualTo(v));
            }
        }

    }
}

#endif
