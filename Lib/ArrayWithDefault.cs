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
using System.Diagnostics;
using System.Linq;

namespace Visyn.Collection
{
    /// <summary>
    /// Class ArrayWithDefault non-null value.
    /// </summary>
    /// <typeparam name="T">Array Type</typeparam>
    /// <seealso cref="System.Collections.Generic.IList{T}" />
    /// <seealso cref="System.Collections.Generic.ICollection{T}" />
    /// <seealso cref="System.Collections.IEnumerable" />
    /// <seealso cref="System.Collections.Generic.IEnumerable{T}" />
    [Obsolete("Works, not currently used or tested")]
    public class ArrayWithDefault<T> : IList<T>,ICollection<T>, IEnumerable , IEnumerable<T>
    {
        /// <summary>
        /// The default item to replace default(T)
        /// </summary>
        private readonly T _defaultItem;
        /// <summary>
        /// The internal array storage
        /// </summary>
        private readonly T[] _array;

        /// <summary>
        /// Gets the number of elements contained in the Array.
        /// </summary>
        /// <value>The length.</value>
        public int Length => _array.Length;
        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <value>The count.</value>
        public int Count => _array.Length;
        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.
        /// </summary>
        /// <value><c>true</c> if this instance is read only; otherwise, <c>false</c>.</value>
        public bool IsReadOnly => false;

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <exception cref="System.IndexOutOfRangeException">T</exception>
        public void Add(T item)
        {
            for (var i = 0; i < _array.Length; i++)
            {
                if (!_array[i].Equals(default(T))) continue;
                _array[i] = item;
                return;
            }
            throw new IndexOutOfRangeException($"{nameof(ArrayWithDefault<T>)} Capacity [{Count}] Exceeded!");
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        public void Clear()
        {
            for (var i = 0; i < Count; i++)
            {
                _array[i] = default(T);
            }
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1" /> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <returns>true if <paramref name="item" /> is found in the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, false.</returns>
        public bool Contains(T item)
        {
            return item.Equals(_defaultItem) ? 
                _array.Any(i => i.Equals(default(T))) : 
                _array.Contains(item);
        }


        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1" /> to an <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            items().CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <returns>true if <paramref name="item" /> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, false. This method also returns false if <paramref name="item" /> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1" />.</returns>
        /// <exception cref="ActionNotSupportedException">If attempt to remove default item from Array</exception>
        public bool Remove(T item)
        {
            if(item.Equals(_defaultItem)) throw new NotSupportedException($"Can not Remove default item [{_defaultItem}]");

            for (var i = 0; i < Count; i++)
            {
                if (!_array[i].Equals(item)) continue;
                _array[i] = default(T);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArrayWithDefault{T}"/> class.
        /// </summary>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="capacity">The capacity.</param>
        public ArrayWithDefault(T defaultValue, int capacity)
        {
            _defaultItem = defaultValue;
            _array = new T[capacity];
        }


        /// <summary>
        /// Itemses this instance.
        /// </summary>
        /// <returns>IEnumerable&lt;T&gt;.</returns>
        private IEnumerable<T> items()
        {
            for (var i = 0; i < Count; i++)
            {
                Debug.Assert(!_array[i].Equals(_defaultItem));
                yield return _array[i].Equals(default(T)) ? _defaultItem : _array[i];
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.</returns>
        IEnumerator<T> IEnumerable<T>.GetEnumerator() => items().GetEnumerator();

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        public IEnumerator GetEnumerator() => items().GetEnumerator();


        /// <summary>
        /// Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1" />.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.IList`1" />.</param>
        /// <returns>The index of <paramref name="item" /> if found in the list; otherwise, -1.</returns>
        public int IndexOf(T item) => _array.IndexOf(item.Equals(_defaultItem) ? default(T) : item);

        /// <summary>
        /// Inserts an item to the <see cref="T:System.Collections.Generic.IList`1" /> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item" /> should be inserted.</param>
        /// <param name="item">The object to insert into the <see cref="T:System.Collections.Generic.IList`1" />.</param>
        public void Insert(int index, T item)
        {
            this[index] = item;
        }

        /// <summary>
        /// Removes the <see cref="T:System.Collections.Generic.IList`1" /> item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        public void RemoveAt(int index)
        {
            _array[index] = default(T);
        }

        /// <summary>
        /// Gets or sets the <see cref="T"/> at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>T.</returns>
        public T this[int index]
        {
            get
            {
                var value = _array[index];
                return (value == null || value.Equals(default(T))) ? _defaultItem : value;
            }
            set
            {
                if (_defaultItem.Equals(value)) _array[index] = default(T);
                else _array[index] = value;
            }
        }
    }

#if false
    // Alternate implementation based on SortedList 
    // would be faster if large virtual array was desired
    class ArrayWithDefault<T> : ICollection<T>, IList<T>
    {
        private int last = 0;
        private readonly SortedList<int, T> _internalList;

        private readonly T _defaultItem;

        public ArrayWithDefault(T defaultValue)
        {
            _defaultItem = defaultValue;
            _internalList = new SortedList<int, T>();
        }

        IEnumerable<T> Items()
        {
            for (int i = 0; i < Count; i++)
            {
                yield return this[i];
            }
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => Items().GetEnumerator();

        public IEnumerator GetEnumerator() => Items().GetEnumerator();

        public void Add(T item)
        {
            Insert(Count,item);
        }

        public void Clear()
        {
            _internalList.Clear();
        }

        public bool Contains(T item)
        {
            return _internalList.Values.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1" /> to an <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            for (var i = 0; i < Count; i++)
            {
                array[arrayIndex + i] = this[i];
            }
        }

        public bool Remove(T item)
        {
            return _internalList.Remove(item);
        }

        public int Count => _internalList.Last().Key+1;

        public bool IsReadOnly { get; set; }
        /// <summary>
        /// Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1" />.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.IList`1" />.</param>
        /// <returns>The index of <paramref name="item" /> if found in the list; otherwise, -1.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public int IndexOf(T item)
        {
            if (_internalList.Values.Contains(item))
            {
                foreach (var kvp in _internalList)
                {
                    if (kvp.Value.Equals(item)) return kvp.Key;
                }
            }
            return -1;
        }

        public void Insert(int index, T item)
        {
            _internalList.AddOrReplace(index, item);
        }

        public void RemoveAt(int index)
        {
            _internalList.Remove(index);
        }

        public T this[int index]
        {
            get { _internalList.ContainsKey(index) ? _internalList[index] : _defaultItem; }
            set { _internalList.AddOrReplace(index, value); }
        }
    }
#endif
}
