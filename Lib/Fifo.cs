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
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace Visyn.Collection
{
    /// <summary>Represents a first-in, first-out collection of objects.</summary>
    /// <typeparam name="T">Specifies the type of elements in the queue.</typeparam>
    [DebuggerDisplay("Count = {Count}")]
    [ComVisible(false)]
    public class Fifo<T> : ICollection, IReadOnlyCollection<T>, ICollection<T>
    {
        private static readonly T[] _emptyArray = new T[0];
        private T[] _array;
        private int _head;
        private int _tail;
        private int _size;
        private int _version;

        private object _syncRoot;
        //private const int _MinimumGrow = 4;
        //private const int _ShrinkThreshold = 32;
        //private const int _GrowFactor = 200;
        //private const int _DefaultCapacity = 4;

        private readonly EqualityComparer<T> equalityComparer = EqualityComparer<T>.Default;

        /// <summary>Gets the number of elements contained in the <see cref="T:System.Collections.Generic.Queue`1" />.</summary>
        /// <returns>The number of elements contained in the <see cref="T:System.Collections.Generic.Queue`1" />.</returns>

        public int Count => _size;

        public int Capacity => _array.Length;

        public int SpaceRemaining => Capacity - _size-1;

        bool ICollection.IsSynchronized => false;


        object ICollection.SyncRoot
        {
                
            get
            {
                if (_syncRoot == null)
                    Interlocked.CompareExchange<object>(ref _syncRoot, new object(), null);
                return _syncRoot;
            }
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.Queue`1" /> class that is empty and has the default initial capacity.</summary>
            
        public Fifo()
        {
            _array = Fifo<T>._emptyArray;
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.Queue`1" /> class that is empty and has the specified initial capacity.</summary>
        /// <param name="capacity">The initial number of elements that the <see cref="T:System.Collections.Generic.Queue`1" /> can contain.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="capacity" /> is less than zero.</exception>
            
        public Fifo(int capacity)
        {
            if (capacity <= 0)
                throw new ArgumentOutOfRangeException(nameof(capacity), capacity, $"{nameof(capacity)} [{capacity}] must be > 0.");
            _array = new T[capacity];
            _head = 0;
            _tail = 0;
            _size = 0;
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.Queue`1" /> 
        /// class that contains elements copied from the specified collection and has sufficient capacity 
        /// to accommodate the number of elements copied.</summary>
        /// <param name="collection">The collection whose elements are copied to the new <see cref="T:System.Collections.Generic.Queue`1" />.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="collection" /> is null.</exception>
            
        public Fifo(IEnumerable<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection),$"Collection must be non-null!");
            _array = new T[4];
            _size = 0;
            _version = 0;
            foreach (T obj in collection)
                Enqueue(obj);
        }

        /// <summary>Removes all objects from the <see cref="T:System.Collections.Generic.Queue`1" />.</summary>
            
        public void Clear()
        {
            if (_head < _tail)
            {
                Array.Clear(_array, _head, _size);
            }
            else
            {
                Array.Clear(_array, _head, _array.Length - _head);
                Array.Clear(_array, 0, _tail);
            }
            _head = 0;
            _tail = 0;
            _size = 0;
            _version = _version + 1;
        }

        /// <summary>Copies the <see cref="T:System.Collections.Generic.Queue`1" /> elements to an existing one-dimensional <see cref="T:System.Array" />, starting at the specified array index.</summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.Queue`1" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="array" /> is null.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="arrayIndex" /> is less than zero.</exception>
        /// <exception cref="T:System.ArgumentException">The number of elements in the source <see cref="T:System.Collections.Generic.Queue`1" /> is greater than the available space from <paramref name="arrayIndex" /> to the end of the destination <paramref name="array" />.</exception>
            
        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array), $"Array must be non-null!");
            if (arrayIndex < 0 || arrayIndex > array.Length)
                throw new ArgumentOutOfRangeException(nameof(arrayIndex), arrayIndex, $"{nameof(arrayIndex)} [{arrayIndex}] must be in range [1,{array.Length}].");

            int length1 = array.Length;
            if (length1 - arrayIndex < _size) throw new ArgumentException($"Array length-offset < size [{length1}-{arrayIndex}<{_size}]!");
            int num = length1 - arrayIndex < _size ? length1 - arrayIndex : _size;
            if (num == 0)
                return;
            int length2 = _array.Length - _head < num ? _array.Length - _head : num;
            Array.Copy(_array, _head, array, arrayIndex, length2);
            int length3 = num - length2;
            if (length3 <= 0)
                return;
            Array.Copy(_array, 0, array, arrayIndex + _array.Length - _head, length3);
        }

            
        void ICollection.CopyTo(Array array, int index)
        {
            if (array == null) throw new ArgumentNullException(nameof(array), $"Array must be non-null!");
            if (array.Rank != 1) throw new ArgumentException($"Multi-dimensional array not supported!");
            if (array.GetLowerBound(0) != 0) throw new ArgumentException($"Array with non-zero lower bound not supported!");
            int length1 = array.Length;
            if (index < 0 || index > length1) throw new ArgumentOutOfRangeException(nameof(index), index, $"{nameof(index)} [{index}] must be in range [1,{array.Length}].");

            if (length1 - index < _size) throw new ArgumentException($"Array length-offset < size [{length1}-{index}<{_size}]!");
            int num = length1 - index < _size ? length1 - index : _size;
            if (num == 0)
                return;
            int length2 = _array.Length - _head < num ? _array.Length - _head : num;
            Array.Copy(_array, _head, array, index, length2);
            int length3 = num - length2;
            if (length3 <= 0)
                return;
            Array.Copy(_array, 0, array, index + _array.Length - _head, length3);
        }

        /// <summary>Adds an object to the end of the <see cref="T:System.Collections.Generic.Queue`1" />.</summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.Queue`1" />. The value can be null for reference types.</param>
            
        public void Enqueue(T item)
        {
            if (_size == _array.Length)
            {
                int capacity = (int)(_array.Length * 200L / 100L);
                if (capacity < _array.Length + 4)
                    capacity = _array.Length + 4;
                SetCapacity(capacity);
            }
            _array[_tail] = item;
            _tail = (_tail + 1) % _array.Length;
            _size = _size + 1;
            _version = _version + 1;
        }
        public void Enqueue(T[] items)
        {
            if (_size + items.Length >= _array.Length)
            {
                int capacity = (int)(_array.Length * 200L / 100L);
                if (capacity < _array.Length + 4)
                    capacity = _array.Length + 4;
                SetCapacity(capacity);
            }
            for (var i = 0; i < items.Length; i++)
            {
                _array[_tail] = items[i];
                _tail = (_tail + 1) % _array.Length;
                _size = _size + 1;
                _version = _version + 1;
            }
        }


        /// <summary>Returns an enumerator that iterates through the <see cref="T:System.Collections.Generic.Queue`1" />.</summary>
        /// <returns>An <see cref="T:System.Collections.Generic.Queue`1.Enumerator" /> for the <see cref="T:System.Collections.Generic.Queue`1" />.</returns>

        public Fifo<T>.Enumerator GetEnumerator() => new Fifo<T>.Enumerator(this);


        IEnumerator<T> IEnumerable<T>.GetEnumerator() => new Fifo<T>.Enumerator(this);


        IEnumerator IEnumerable.GetEnumerator() => new Fifo<T>.Enumerator(this);

        /// <summary>Removes and returns the object at the beginning of the <see cref="T:System.Collections.Generic.Queue`1" />.</summary>
        /// <returns>The object that is removed from the beginning of the <see cref="T:System.Collections.Generic.Queue`1" />.</returns>
        /// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Collections.Generic.Queue`1" /> is empty.</exception>
            
        public T Dequeue()
        {
            if (_size == 0) throw new InvalidOperationException("Can not Dequeue.  Queue is empty!");
            T obj = _array[_head];
            _array[_head] = default(T);
            _head = (_head + 1) % _array.Length;
            _size = _size - 1;
            _version = _version + 1;
            return obj;
        }

        public IEnumerable<T> Dequeue(int items)
        {
            if (items > _size) throw new ArgumentException($"Cannot dequeue more items [{items}] than currently queued {_size}!");
            while(items-- > 0)
            {
                yield return Dequeue();
            }
        }

        public IEnumerable<T> DequeueUntil(T value)
        {
            T item;
            do
            {
                if (Count == 0) break;
                item = Dequeue();
                yield return item;
                
            } while (!equalityComparer.Equals(item,value));
        }

        /// <summary>Returns the object at the beginning of the <see cref="T:System.Collections.Generic.Queue`1" /> without removing it.</summary>
        /// <returns>The object at the beginning of the <see cref="T:System.Collections.Generic.Queue`1" />.</returns>
        /// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Collections.Generic.Queue`1" /> is empty.</exception>

        public T Peek()
        {
            if (_size == 0) throw new InvalidOperationException("Can not peek.  Queue is empty!");
            return _array[_head];
        }

        /// <summary>Determines whether an element is in the <see cref="T:System.Collections.Generic.Queue`1" />.</summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.Queue`1" />. The value can be null for reference types.</param>
        /// <returns>true if <paramref name="item" /> is found in the <see cref="T:System.Collections.Generic.Queue`1" />; otherwise, false.</returns>
            
        public bool Contains(T item)
        {
            int index = _head;
            int size = _size;
            
            while (size-- > 0)
            {
                if (item == null)
                {
                    if (_array[index] == null)
                        return true;
                }
                else if (_array[index] != null && equalityComparer.Equals(_array[index], item))
                    return true;
                index = (index + 1) % _array.Length;
            }
            return false;
        }

        internal T GetElement(int i) => _array[(_head + i) % _array.Length];

        /// <summary>Copies the <see cref="T:System.Collections.Generic.Queue`1" /> elements to a new array.</summary>
        /// <returns>A new array containing elements copied from the <see cref="T:System.Collections.Generic.Queue`1" />.</returns>
            
        public T[] ToArray()
        {
            T[] objArray = new T[_size];
            if (_size == 0)
                return objArray;
            if (_head < _tail)
            {
                Array.Copy(_array, _head, objArray, 0, _size);
            }
            else
            {
                Array.Copy(_array, _head, objArray, 0, _array.Length - _head);
                Array.Copy(_array, 0, objArray, _array.Length - _head, _tail);
            }
            return objArray;
        }

        private void SetCapacity(int capacity)
        {
            T[] objArray = new T[capacity];
            if (_size > 0)
            {
                if (_head < _tail)
                {
                    Array.Copy(_array, _head, objArray, 0, _size);
                }
                else
                {
                    Array.Copy(_array, _head, objArray, 0, _array.Length - _head);
                    Array.Copy(_array, 0, objArray, _array.Length - _head, _tail);
                }
            }
            _array = objArray;
            _head = 0;
            _tail = _size == capacity ? 0 : _size;
            _version = _version + 1;
        }

        /// <summary>Sets the capacity to the actual number of elements in the <see cref="T:System.Collections.Generic.Queue`1" />, if that number is less than 90 percent of current capacity.</summary>
            
        public void TrimExcess()
        {
            if (_size >= (int)(_array.Length * 0.9)) return;
            SetCapacity(_size);
        }

        #region Implementation of ICollection<T>

        public void Add(T item) => Enqueue(item);
        public void AddRange(IEnumerable<T> items)
        {
            foreach (var item in items) Enqueue(item);
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        public bool IsReadOnly => false;

        #endregion

        /// <summary>Enumerates the elements of a <see cref="T:System.Collections.Generic.Queue`1" />.</summary>
            
      
        public struct Enumerator : IEnumerator<T>, IDisposable, IEnumerator
        {
            private Fifo<T> _q;
            private int _index;
            private int _version;
            private T _currentElement;

            /// <summary>Gets the element at the current position of the enumerator.</summary>
            /// <returns>The element in the <see cref="T:System.Collections.Generic.Queue`1" /> at the current position of the enumerator.</returns>
            /// <exception cref="T:System.InvalidOperationException">The enumerator is positioned before the first element of the collection or after the last element. </exception>
                
            public T Current
            {
                get
                {
                    if (_index >= 0) return _currentElement;
                    if (_index == -1) throw new InvalidOperationException("Enum not started!");
                    throw new InvalidOperationException("Enum ended!");
                }
            }

                
            object IEnumerator.Current
            {
                get
                {
                    if (_index >= 0) return _currentElement;
                    if (_index == -1) throw new InvalidOperationException("Enum not started!");
                    throw new InvalidOperationException("Enum ended!");
                }
            }

            internal Enumerator(Fifo<T> q)
            {
                _q = q;
                _version = _q._version;
                _index = -1;
                _currentElement = default(T);
            }

            /// <summary>Releases all resources used by the <see cref="T:System.Collections.Generic.Queue`1.Enumerator" />.</summary>
                
            public void Dispose()
            {
                _index = -2;
                _currentElement = default(T);
            }

            /// <summary>Advances the enumerator to the next element of the <see cref="T:System.Collections.Generic.Queue`1" />.</summary>
            /// <returns>true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.</returns>
            /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
                
            public bool MoveNext()
            {
                if (_index == -2)
                    return false;
                _index = _index + 1;
                if (_index == _q._size)
                {
                    _index = -2;
                    _currentElement = default(T);
                    return false;
                }
                _currentElement = _q.GetElement(_index);
                return true;
            }

                
            void IEnumerator.Reset()
            {
                _index = -1;
                _currentElement = default(T);
            }
        }
    }
}
