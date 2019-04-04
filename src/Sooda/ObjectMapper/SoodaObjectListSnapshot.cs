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

using Sooda.Caching;
using Sooda.Logging;
using Sooda.QL;
using Sooda.Schema;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace Sooda.ObjectMapper
{
    public class SoodaObjectListSnapshot : ISoodaObjectList
    {
        private static readonly Logger logger = LogManager.GetLogger("Sooda.ListSnapshot");

        public SoodaObjectListSnapshot()
        {
        }

        public SoodaObjectListSnapshot(IList list)
        {
            foreach (SoodaObject o in list)
            {
                AddObjectToSnapshot(o);
            }
        }

        public SoodaObjectListSnapshot(IList list, SoodaObjectFilter filter)
        {
            foreach (SoodaObject o in list)
            {
                if (filter(o))
                    AddObjectToSnapshot(o);
            }
        }

        public SoodaObjectListSnapshot(IList list, SoodaWhereClause whereClause)
        {
            foreach (SoodaObject o in list)
            {
                if (whereClause.Matches(o, true))
                    AddObjectToSnapshot(o);
            }
        }

        public SoodaObjectListSnapshot(IList list, SoqlBooleanExpression filterExpression) : this(list, new SoodaWhereClause(filterExpression))
        {
        }

        public SoodaObjectListSnapshot(IList list, int first, int length)
        {
            this.classInfo = null;

            int start = first;
            if (start < 0)
            {
                length += start;
                start = 0;
            }
            if (start + length > list.Count)
                length = list.Count - start;

            items.Capacity = length;
            for (int i = 0; i < length; ++i)
            {
                items.Add(list[start + i]);
            }
        }

        public SoodaObjectListSnapshot(IList list, IComparer comp)
        {
            items.Capacity = list.Count;
            for (int i = 0; i < list.Count; ++i)
            {
                items.Add(list[i]);
            }
            items.Sort(comp);
        }

        public SoodaObjectListSnapshot(SoodaTransaction tran, SoodaObjectFilter filter, ClassInfo ci)
        {
            this.classInfo = ci;
            List<WeakSoodaObject> al = tran.GetObjectsByClassName(ci.Name);

            if (al != null)
            {
                // al.Clone() is needed because
                // the filter expression may materialize new objects
                // during checking. This way we avoid "collection modified" exception

                List<SoodaObject> clonedArray = new List<SoodaObject>();
                foreach (WeakSoodaObject wr in al)
                {
                    SoodaObject obj = wr.TargetSoodaObject;
                    if (obj != null)
                        clonedArray.Add(obj);
                }

                foreach (SoodaObject obj in clonedArray)
                {
                    if (filter(obj))
                    {
                        items.Add(obj);
                    }
                }
            }
        }

        protected void AddObjectToSnapshot(SoodaObject o)
        {
            items.Add(o);
        }

        public SoodaObjectListSnapshot(SoodaTransaction t, SoodaWhereClause whereClause, SoodaOrderBy orderBy, int startIdx, int pageCount, SoodaSnapshotOptions options, ClassInfo ci)
        {
            this.classInfo = ci;
            string[] involvedClasses = null;

            bool useCache;
            if ((options & SoodaSnapshotOptions.NoCache) != 0)
                useCache = false;
            else if ((options & SoodaSnapshotOptions.Cache) != 0)
                useCache = true;
            else
                useCache = t.CachingPolicy.ShouldCacheCollection(ci, whereClause, orderBy, startIdx, pageCount);

            if (whereClause != null && whereClause.WhereExpression != null)
            {
                if ((options & SoodaSnapshotOptions.NoWriteObjects) == 0 || useCache)
                {
                    try
                    {
                        GetInvolvedClassesVisitor gic = new GetInvolvedClassesVisitor(classInfo);
                        gic.GetInvolvedClasses(whereClause.WhereExpression);
                        involvedClasses = gic.ClassNames;
                    }
                    catch
                    {
                        // logger.Warn("{0}", ex);
                        // cannot detect involved classes (probably because of RAWQUERY)
                        // - precommit all objects
                        // if we get here, involvedClasses remains set to null
                    }
                }
            }
            else
            {
                // no where clause

                involvedClasses = new string[] { ci.Name };
            }

            if ((options & SoodaSnapshotOptions.NoWriteObjects) == 0)
                t.PrecommitClasses(involvedClasses);

            LoadList(t, whereClause, orderBy, startIdx, pageCount, options, involvedClasses, useCache);
        }

        private void LoadList(SoodaTransaction transaction, SoodaWhereClause whereClause, SoodaOrderBy orderBy, int startIdx, int pageCount, SoodaSnapshotOptions options, string[] involvedClassNames, bool useCache)
        {
            ISoodaObjectFactory factory = transaction.GetFactory(classInfo);
            string cacheKey = null;

            if (useCache)
            {
                // cache makes sense only on clean database
                if (!transaction.HasBeenPrecommitted(classInfo))
                {
                    cacheKey = SoodaCache.GetCollectionKey(classInfo, whereClause);
                }

                IEnumerable keysCollection = transaction.LoadCollectionFromCache(cacheKey, logger);
                if (keysCollection != null)
                {
                    foreach (object o in keysCollection)
                    {
                        SoodaObject obj = factory.GetRef(transaction, o);
                        // this binds to cache
                        obj.EnsureFieldsInited();
                        items.Add(obj);
                    }

                    if (orderBy != null)
                    {
                        items.Sort(orderBy.GetComparer());
                    }

                    if (startIdx > 0)
                    {
                        if (startIdx < items.Count)
                            items.RemoveRange(0, startIdx);
                        else
                            items.Clear();
                    }

                    if (pageCount != -1 && pageCount < items.Count)
                    {
                        items.RemoveRange(pageCount, items.Count - pageCount);
                    }

                    return;
                }
            }

            SoodaDataSource ds = transaction.OpenDataSource(classInfo.GetDataSource());

            if ((options & SoodaSnapshotOptions.KeysOnly) != 0)
            {
                using (IDataReader reader = ds.LoadMatchingPrimaryKeys(transaction.Schema, classInfo, whereClause, orderBy, startIdx, pageCount))
                {
                    while (reader.Read())
                    {
                        SoodaObject obj = SoodaObject.GetRefFromKeyRecordHelper(transaction, factory, reader);
                        items.Add(obj);
                    }
                }
            }
            else
            {
                TableInfo[] loadedTables;

                using (IDataReader reader = ds.LoadObjectList(transaction.Schema, classInfo, whereClause, orderBy, startIdx, pageCount, options, out loadedTables))
                {
                    while (reader.Read())
                    {
                        SoodaObject obj = SoodaObject.GetRefFromRecordHelper(transaction, factory, reader, 0, loadedTables, 0);
                        if ((options & SoodaSnapshotOptions.VerifyAfterLoad) != 0 && whereClause != null && !whereClause.Matches(obj, false))
                            continue; // don't add the object
                        items.Add(obj);
                    }
                }
            }

            if (cacheKey != null && useCache && startIdx == 0 && pageCount == -1 && involvedClassNames != null)
            {
                TimeSpan expirationTimeout;
                bool slidingExpiration;

                if (transaction.CachingPolicy.GetExpirationTimeout(
                            classInfo, whereClause, orderBy, startIdx, pageCount, items.Count,
                            out expirationTimeout, out slidingExpiration))
                {
                    transaction.StoreCollectionInCache(cacheKey, classInfo, items, involvedClassNames, (options & SoodaSnapshotOptions.KeysOnly) == 0, expirationTimeout, slidingExpiration);
                }
            }
        }

        public SoodaObject GetItem(int pos)
        {
            return (SoodaObject)items[pos];
        }

        public int Add(object obj)
        {
            items.Add(obj);
            return items.Count;
        }

        public void Remove(object obj)
        {
            items.Remove(obj);
        }

        public bool Contains(object obj)
        {
            return items.Contains(obj);
        }

        public IEnumerator GetEnumerator()
        {
            return items.GetEnumerator();
        }

        private readonly ArrayList items = new ArrayList();
        private ClassInfo classInfo;

        public bool IsReadOnly
        {
            get { return false; }
        }

        object IList.this[int index]
        {
            get { return items[index]; }
            set { items[index] = value; }
        }

        public void RemoveAt(int index)
        {
            items.RemoveAt(index);
        }

        public void Insert(int index, object value)
        {
            items.Insert(index, value);
        }

        public void Clear()
        {
            items.Clear();
        }

        public int IndexOf(object value)
        {
            return items.IndexOf(value);
        }

        public bool IsFixedSize
        {
            get
            {
                return false;
            }
        }

        public bool IsSynchronized
        {
            get
            {
                return false;
            }
        }

        public int Count
        {
            get
            {
                return items.Count;
            }
        }

        public int PagedCount
        {
            get
            {
                throw new NotSupportedException("Paged count is no longer supported due to performance - it should be calculated it directly in business logic");
            }
        }

        public void CopyTo(Array array, int index)
        {
            items.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return this;
            }
        }

        public ISoodaObjectList GetSnapshot()
        {
            return this;
        }

        public ISoodaObjectList SelectFirst(int n)
        {
            return new SoodaObjectListSnapshot(this, 0, n);
        }

        public ISoodaObjectList SelectLast(int n)
        {
            return new SoodaObjectListSnapshot(this, this.Count - n, n);
        }

        public ISoodaObjectList SelectRange(int from, int to)
        {
            return new SoodaObjectListSnapshot(this, from, to - from);
        }

        public ISoodaObjectList Filter(SoodaObjectFilter filter)
        {
            return new SoodaObjectListSnapshot(this, filter);
        }

        public ISoodaObjectList Filter(SoqlBooleanExpression filterExpression)
        {
            return new SoodaObjectListSnapshot(this, filterExpression);
        }

        public ISoodaObjectList Filter(SoodaWhereClause whereClause)
        {
            return new SoodaObjectListSnapshot(this, whereClause);
        }

        public ISoodaObjectList Sort(IComparer comparer)
        {
            return new SoodaObjectListSnapshot(this, comparer);
        }

        public ISoodaObjectList Sort(string sortOrder)
        {
            return new SoodaObjectListSnapshot(this).Sort(sortOrder);
        }

        public ISoodaObjectList Sort(SoqlExpression expression, SortOrder sortOrder)
        {
            return new SoodaObjectListSnapshot(this).Sort(expression, sortOrder);
        }

        public ISoodaObjectList Sort(SoqlExpression expression)
        {
            return new SoodaObjectListSnapshot(this).Sort(expression, SortOrder.Ascending);
        }
    }
}
