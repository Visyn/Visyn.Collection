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

using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Visyn.Collection
{
    public abstract class SimpleCacheBase<TKey,TValue> : ICache<TKey, TValue>, ICacheHitStats
    {
        public int Hits { get; private set; }
        public int Misses { get; private set; }
        public int NotFound { get; private set; }
        public int Entries => Count;

        private readonly IDictionary<TKey, TValue> Cache;

        public abstract TValue Get(TKey key);
        protected SimpleCacheBase(IDictionary<TKey,TValue> dictionary)
        {
            Cache = dictionary ?? new ConcurrentDictionary<TKey, TValue>();
        }

        /// <summary>
        /// Add item to the cache
        /// </summary>
        /// <param name="key">Cache Key</param>
        /// <param name="value">Cache Value</param>
        /// <returns>Value added</returns>
        protected TValue Add(TKey key, TValue value)
        {
            if (value == null) NotFound++;
            Cache.Add(key, value);
            return value;
        }

        #region Implementation of IEnumerable

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return Cache.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Implementation of IReadOnlyCollection<out KeyValuePair<string,T>>

        /// <summary>
        /// Gets the number of elements in the collection.
        /// </summary>
        /// <returns>
        /// The number of elements in the collection. 
        /// </returns>
        public int Count => Cache.Count;

        #endregion

        #region Implementation of IReadOnlyDictionary<string,T>

        /// <summary>
        /// Determines whether the read-only dictionary contains an element that has the specified key.
        /// </summary>
        /// <returns>
        /// true if the read-only dictionary contains an element that has the specified key; otherwise, false.
        /// </returns>
        /// <param name="key">The key to locate.</param><exception cref="T:System.ArgumentNullException"><paramref name="key"/> is null.</exception>
        public bool ContainsKey(TKey key) => Cache.ContainsKey(key);

        /// <summary>
        /// Gets the value that is associated with the specified key.
        /// </summary>
        /// <returns>
        /// true if the object that implements the <see cref="T:System.Collections.Generic.IReadOnlyDictionary`2"/> interface contains an element that has the specified key; otherwise, false.
        /// </returns>
        /// <param name="key">The key to locate.</param><param name="value">When this method returns, the value associated with the specified key, if the key is found; otherwise, the default value for the type of the <paramref name="value"/> parameter. This parameter is passed uninitialized.</param><exception cref="T:System.ArgumentNullException"><paramref name="key"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetValue(TKey key, out TValue value)
        {
            var result = Cache.TryGetValue(key, out value);
            if (result) Hits++;
            else Misses++;
            return result;
        }

        /// <summary>
        /// Gets the element that has the specified key in the read-only dictionary.
        /// </summary>
        /// <returns>
        /// The element that has the specified key in the read-only dictionary.
        /// </returns>
        /// <param name="key">The key to locate.</param><exception cref="T:System.ArgumentNullException"><paramref name="key"/> is null.</exception><exception cref="T:System.Collections.Generic.KeyNotFoundException">The property is retrieved and <paramref name="key"/> is not found. </exception>
        public TValue this[TKey key] => Get(key);

        /// <summary>
        /// Gets an enumerable collection that contains the keys in the read-only dictionary. 
        /// </summary>
        /// <returns>
        /// An enumerable collection that contains the keys in the read-only dictionary.
        /// </returns>
        public IEnumerable<TKey> Keys => Cache.Keys;

        /// <summary>
        /// Gets an enumerable collection that contains the values in the read-only dictionary.
        /// </summary>
        /// <returns>
        /// An enumerable collection that contains the values in the read-only dictionary.
        /// </returns>
        public IEnumerable<TValue> Values => Cache.Values;

        #endregion
    }
}