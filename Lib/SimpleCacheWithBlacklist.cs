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
using System.Collections.Generic;

namespace Visyn.Collection
{
    public class SimpleCacheWithBlacklist<TKey,TValue> : SimpleCache<TKey, TValue>
    {
        protected List<TKey> BlackListedKeys;
        public override TValue Get(TKey key)
        {
            if (BlackListedKeys?.Contains(key) == true) return default(TValue);
            TValue value;
            if (TryGetValue(key, out value)) return value;
            if (GetMissingCacheItem == null) { return default(TValue); }

            TValue newItem = GetMissingCacheItem(key);
            if (newItem != null) return Add(key, newItem);

            if(BlackListedKeys == null) BlackListedKeys = new List<TKey>();
            BlackListedKeys.Add(key);
            return default(TValue);
        }

        public SimpleCacheWithBlacklist(Func<TKey, TValue> missingItemGetFunction) : base(missingItemGetFunction)
        {
        }
    }
}