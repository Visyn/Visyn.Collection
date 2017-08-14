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

using System.Collections.Generic;
using System.Linq;
using Visyn.Public.Comparison;

namespace Visyn.Collection
{
    public static class KeyValuePairExtensions
    {
        public static IEnumerable<TKey> Keys<TKey, TValue>(this IList<KeyValuePair<TKey, TValue>> kvps) 
            => kvps.Select(kvp => kvp.Key);

        public static IEnumerable<TValue> Values<TKey, TValue>(this IList<KeyValuePair<TKey,TValue>> kvps) 
            => kvps.Select(kvp => kvp.Value);

        public static IComparer<KeyValuePair<TKey, TValue>> KeyComparer<TKey, TValue>()
        {
            return new CustomComparer<KeyValuePair<TKey, TValue>>((kvp1, kvp2) 
                => Comparer<TKey>.Default.Compare(kvp1.Key, kvp2.Key));
        }

        public static IComparer<KeyValuePair<TKey,TValue>> KeyComparer<TKey, TValue>(this IList<KeyValuePair<TKey, TValue>> kvps) 
            => KeyComparer<TKey, TValue>();

        public static IComparer<KeyValuePair<TKey, TValue>> DefaultKeyComparer<TKey, TValue>(this IList<KeyValuePair<TKey, TValue>> kvps) 
            => KeyComparer<TKey, TValue>();
    }
}
