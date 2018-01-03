#region Copyright (c) 2015-2018 Visyn
// The MIT License(MIT)
// 
// Copyright (c) 2015-2018 Visyn
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

namespace Visyn.Collection
{
    public class IndexedCollection<T> : IndexedCollection<T,T>
    {
        public IndexedCollection(IReadOnlyList<T> collection) : base(collection) { }

        public IndexedCollection(IEnumerable<T> collection, int count) : base(collection, count) { }
    }

    public class IndexedCollection<TIn1,TIn2,TOut> : IndexedCollection<TIn1, TOut> 
        where TIn1 : TOut
        where TIn2 : TOut
    {
        protected IReadOnlyList<TIn2> Collection2 { get; private set; }

        public IndexedCollection(IReadOnlyList<TIn1> collection1, IReadOnlyList<TIn2> collection2) 
            : base(collection1)
        {
            Collection2 = collection2;            
        }
    }

    public class IndexedCollection<TIn,TOut> : IReadOnlyList<TOut>, IEnumerator<TOut>  where TIn : TOut
    {
        protected IEnumerable<TIn> Collection { get; private set; }

        private IReadOnlyList<TIn> _list;

        protected IReadOnlyList<TIn> List
        {
            get { return _list ?? (_list = new List<TIn>(Collection)); }
            private set { _list = value; }
        }

        protected int Index { get; private set; } = -1;
        public IndexedCollection(IReadOnlyList<TIn> list)
        {
            _list = list;
            Collection = list;
            Count = list.Count;
        }

        public IndexedCollection(IEnumerable<TIn> collection, int count)
        {
            Collection = collection;
            _list = collection as IReadOnlyList<TIn>;
            Count = count;
        }

        public void Reset() { Index = -1; }

        #region Implementation of IEnumerator

        public bool MoveNext() => ++Index < Count;

        object IEnumerator.Current => this[Index];

        public virtual TOut Current => this[Index];

        #endregion

        #region Implementation of IEnumerable

        IEnumerator IEnumerable.GetEnumerator() => this;

        public IEnumerator<TOut> GetEnumerator() => this;

        #endregion

        #region Implementation of IReadOnlyList<out T>
        public int Count { get; }
        public virtual TOut this[int index]
        {
            get
            {
                if(index < 0 || index >= Count) throw new IndexOutOfRangeException($"Index [{index}] is not in range [{0},{Count}]");
                return List[index];
            }
        }

        #endregion
        #region Implementation of IDisposable

        public void Dispose()
        {
            Collection = null;
            List = null;
        }

        #endregion
    }
    
}
