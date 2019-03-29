//
// Copyright (c) 2015 Piotr Fusik <piotr@fusik.info>
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
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Text;
using NUnit.Framework;
using Sooda.Schema;
using Sooda.UnitTests.BaseObjects;
using Sooda.UnitTests.BaseObjects.Stubs;
using FieldInfo = Sooda.Schema.FieldInfo;

namespace Sooda.UnitTests.TestCases
{
    [TestFixture]
    public class SchemaTest
    {
        [Test]
        public void ParentClass()
        {
            ClassInfo classInfo = Contact_Factory.TheClassInfo;
            foreach (FieldInfo fi in classInfo.UnifiedFields)
            {
                Assert.AreEqual(classInfo, fi.ParentClass);
                Assert.IsNull(fi.ParentRelation);
            }
        }

        [Test]
        public void ParentRelation()
        {
            RelationInfo relationInfo = _DatabaseSchema.GetSchema().FindRelationByName("ContactToBike");
            foreach (FieldInfo fi in relationInfo.Table.Fields)
            {
                Assert.AreEqual(relationInfo, fi.ParentRelation);
                Assert.IsNull(fi.ParentClass);
            }
        }

        public static IEnumerable<TestCaseData> ResolveClassData()
        {
            foreach (var classInfo in Objects._DatabaseSchema.GetSchema().Classes)
            {
                yield return new TestCaseData(classInfo.Name);
            }
        }

        [Test]
        [TestCaseSource("ResolveClassData")]
        public void ResolveClass(string className)
        {
            var schema = Objects._DatabaseSchema.GetSchema();

            var classInfo = schema.FindClassByName(className);
            Assert.That(classInfo, Is.Not.Null);

            var resolveMethod = classInfo.GetType().GetMethod("Resolve", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.That(resolveMethod, Is.Not.Null);

            resolveMethod.Invoke(classInfo, new object[] { schema }); // 1st call

            var reference = DumpMembers(classInfo, new string[] { });

            resolveMethod.Invoke(classInfo, new object[] { schema }); // 2nd call
            resolveMethod.Invoke(classInfo, new object[] { schema }); // 3rd call

            var validate = DumpMembers(classInfo, new string[] { });
            AssertEqual(reference, validate);
        }

        [Test]
        public void ResolveSchemaInfo()
        {
            var schema = Objects._DatabaseSchema.GetSchema();

            schema.Resolve();

            var referencedValues = DumpMembers(schema, new string[]{});

            foreach (var @class in schema.Classes)
            {
                foreach (var kv in DumpMembers(@class, new string[0]))
                {
                    referencedValues[@class.Name + "." + kv.Key] = kv.Value;
                }
            }

            schema.Resolve(); // 2nd call
            schema.Resolve(); // 3rd call

            var validate = DumpMembers(schema, new string[] { });

            foreach (var @class in schema.Classes)
            {
                foreach (var kv in DumpMembers(@class, new string[0]))
                {
                    validate[@class.Name + "." + kv.Key] = kv.Value;
                }
            }

            AssertEqual(referencedValues, validate);
        }



        [Test]
        [TestCaseSource("ResolveClassData")]
        public void ResolveByDynamicFields(string className)
        {
            var excludeMembers = new [] {"_rwLock"};
            
            using (SoodaTransaction t = new SoodaTransaction())
            {
                var reference = DumpMembers(t.Schema, excludeMembers);
                const string StringField = "test_dynamic_field";
                DynamicFieldManager.Add(new FieldInfo
                {
                    ParentClass = t.Schema.FindClassByName(className),
                    Name = StringField,
                    TypeName = "String",
                    Size = 128,
                    IsNullable = false
                }, t);

                FieldInfo fi = t.Schema.FindClassByName(className).FindFieldByName(StringField);
                DynamicFieldManager.Remove(fi, t);

                var validate = DumpMembers(t.Schema, excludeMembers);
                AssertEqual(reference, validate);
                
            }
        }

        private static void AssertEqual(Dictionary<string, string> expected, Dictionary<string, string> current)
        {
            List<string> diff = new List<string>();
            foreach (var r in expected)
            {
                var cur = current[r.Key];
                var exp = r.Value;
                if (cur != exp)
                {
                    var lcs = LongestCommonSubstring(cur, exp);

                    Console.WriteLine("### {0} expected: {1}\n", r.Key, Partial(exp, lcs, 128));
                    Console.WriteLine("### {0} current: {1}\n", r.Key, Partial(cur, lcs, 128));
                    diff.Add(r.Key);
                }
            }

            Assert.That(diff, Is.Empty, string.Format("Different fields: {0}", string.Join(", ", diff)));
            Assert.That(current.Count, Is.EqualTo(expected.Count));
        }

        private static string Partial(string input, int start, int length)
        {
            if (start > input.Length)
                throw new ArgumentException("must be less then input length", nameof(start));
            
            const int margin = 32;

            var end = start + length;
            if (end > input.Length)
                end = input.Length;

            
            var main = input.Substring(start, end - start);

            var prefixStart = input.LastIndexOf('\n', Math.Max(start - margin, 0)) + 1;
            var prefix = (prefixStart  == 0) ? input.Substring(0, start) :
                string.Format("...({0} more chars)...{1}", prefixStart, input.Substring(prefixStart, start - prefixStart));
            var suffix = (end + margin >= input.Length) ? input.Substring(end, input.Length - end) :
                string.Format("{0}...({1} more chars)...", input.Substring(end, margin), input.Length - (end + margin));

            return string.Concat(prefix, ">>>>", main, suffix);
        }

        private static int LongestCommonSubstring(string a, string b)
        {
            var min = Math.Min(a.Length, b.Length);
            for (var i = 0; i < min; ++i)
            {
                if (a[i] != b[i])
                    return i;
            }
            return min;
        }

        private static Dictionary<string, string> DumpMembers(object @object, IEnumerable<string> exclude)
        {
            var ex = new HashSet<string>(exclude);
            var referencedValues = new Dictionary<string, string>();
            var members = @object.GetType().GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).ToList();

            foreach (var memb in members)
            {
                if (ex.Contains(memb.Name))
                    continue;

                object v;
                if (memb is System.Reflection.FieldInfo fld)
                    v = fld.GetValue(@object);
                else if (memb is System.Reflection.PropertyInfo prop)
                    v = prop.GetValue(@object, null);
                else
                    continue;

                var formatted = FormatValue(v, true);
                referencedValues.Add(memb.Name, formatted);
            }

            return referencedValues;
        }

        private static string FormatValue(object v, bool childDetails)
        {
            if (v == null)
                return null;

            if (v is string s)
                return string.Format("'{0}'", s);

            if (v is StringCollection strc)
            {
                var desc = new StringBuilder();
                desc.Append("[");
                for (var i = 0; i < strc.Count; ++i)
                    desc.AppendFormat(i == 0 ? "'{0}'" : ", '{0}'", strc[i]);
                desc.AppendFormat("] ({0} item(s))", strc.Count);
                return desc.ToString();
            }

            if (v is IDictionary dict)
            {
                var desc = new StringBuilder();
                desc.AppendFormat("{0} item(s)", dict.Count);

                if (!childDetails)
                    return desc.ToString();

                foreach (DictionaryEntry entry in dict)
                {
                    desc.AppendFormat("\n{0} => {1}", FormatValue(entry.Key, false), FormatValue(entry.Value, false));
                }

                return desc.ToString();
            }

            if (v is ICollection col)
            {
                var desc = new StringBuilder();
                desc.AppendFormat("{0} item(s)", col.Count);

                if (!childDetails)
                {
                    desc.Append(" [");
                    bool first = true;
                    foreach (var item in col)
                    {
                        desc.AppendFormat(first ? "{0}" : ", {0}", item);
                        first = false;
                    }
                    desc.Append("]");
                    return desc.ToString();
                }

                foreach (var item in col)
                {
                    if (item == null)
                    {
                        desc.Append("\n--- null ---");
                        continue;
                    }

                    var itemType = item.GetType();
                    desc.AppendFormat("\n--- {0} ---", item);
                    foreach (var memb in itemType.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                    {
                        object memberValue;
                        if (memb is System.Reflection.FieldInfo fld)
                            memberValue = fld.GetValue(item);
                        else if (memb is System.Reflection.PropertyInfo prop && prop.GetIndexParameters().Length == 0)
                            memberValue = prop.GetValue(item, null);
                        else
                            continue;

                        string val = FormatValue(memberValue, false);
                        desc.AppendFormat("\n{0}: {1}", memb.Name, val);
                    }
                }

                return desc.ToString();
            }

            return v.ToString();
        }
    }
}
