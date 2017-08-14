#region Copyright (c) 2015-2017 Visyn
// The MIT License(MIT)
// 
// Copyright(c) 2015-2017 Visyn
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Visyn.Public.JetBrains;

namespace Visyn.Collection
{
    public class ConcurrentList<T> : IList<T>,IDisposable
    {
        #region Fields
        [NotNull]
        private readonly List<T> _list;
        [NotNull]
        private readonly ReaderWriterLockSlim _lock;
        #endregion

        #region Constructors
        public ConcurrentList()
        {
            _lock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
            _list = new List<T>();
        }

        public ConcurrentList(int capacity)
        {
            _lock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
            _list = new List<T>(capacity);
        }

        public ConcurrentList(IEnumerable<T> items)
        {
            _lock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
            _list = new List<T>(items);
        }
        #endregion

        #region Methods
        public void Add(T item)
        {
            try
            {
                _lock.EnterWriteLock();
                _list.Add(item);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public void AddRange(IEnumerable<T> collection)
        {
            try
            {
                _lock.EnterWriteLock();
                foreach (var item in collection)
                {
                    _list.Add(item);
                }
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public void Insert(int index, T item)
        {
            try
            {
                _lock.EnterWriteLock();
                _list.Insert(index, item);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public bool Remove(T item)
        {
            try
            {
                _lock.EnterWriteLock();
                return _list.Remove(item);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public int RemoveAll(Predicate<T> match)
        {
            try
            {
                _lock.EnterWriteLock();
                return _list.RemoveAll(match);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }


        public void RemoveAt(int index)
        {
            try
            {
                _lock.EnterWriteLock();
                _list.RemoveAt(index);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public int IndexOf(T item)
        {
            try
            {
                _lock.EnterReadLock();
                return _list.IndexOf(item);
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        public void Clear()
        {
            try
            {
                _lock.EnterWriteLock();
                _list.Clear();
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public bool Contains(T item)
        {
            try
            {
                _lock.EnterReadLock();
                return _list.Contains(item);
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            try
            {
                _lock.EnterReadLock();
                _list.CopyTo(array, arrayIndex);
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        public IEnumerator<T> GetEnumerator() => new ConcurrentEnumerator<T>(_list, _lock);

        IEnumerator IEnumerable.GetEnumerator() => new ConcurrentEnumerator<T>(_list, _lock);

        ~ConcurrentList()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
                GC.SuppressFinalize(this);

            _lock.Dispose();
        }
        #endregion

        #region Properties
        public T this[int index]
        {
            get
            {
                try
                {
                    _lock.EnterReadLock();
                    return _list[index];
                }
                finally
                {
                    _lock.ExitReadLock();
                }
            }
            set
            {
                try
                {
                    _lock.EnterWriteLock();
                    _list[index] = value;
                }
                finally
                {
                    _lock.ExitWriteLock();
                }
            }
        }

        public int Count
        {
            get
            {
                try
                {
                    _lock.EnterReadLock();
                    return _list.Count;
                }
                finally
                {
                    _lock.ExitReadLock();
                }
            }
        }

        public bool IsReadOnly => false;

        #endregion
    }
}