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
using System.Collections.Generic;

namespace Visyn.Collection
{
    public class SimpleCalculatedCache<TInput, TKey, TValue> : SimpleCacheBase<TKey, TValue>
    {
        private readonly KeyValueCalculculator<TInput, TKey, TValue> _calculator;
        public virtual TValue Get(TInput input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input), $"SimpleCalculatedCache.Get requires a non-null input.");
            if (_calculator == null) throw new ArgumentNullException(nameof(input), $"SimpleCache.Get requires a non-null {typeof(KeyValueCalculculator<TInput, TKey, TValue>).Name} Converter.");
            var key = _calculator.UpdateInput(input);
            TValue value;
            if( TryGetValue(key, out value)) return value;
            return Add(key, _calculator.Value);
        }

        public SimpleCalculatedCache(KeyValueCalculculator<TInput, TKey, TValue> calculator, IDictionary<TKey,TValue> dictionary=null) : base(dictionary)
        {
            if (calculator == null) throw new ArgumentNullException(nameof(calculator), $"SimpleCache.Get requires a non-null {typeof(KeyValueCalculculator<TInput, TKey, TValue>).Name} Converter.");
            _calculator = calculator;
        }

        #region Overrides of SimpleCacheBase<TKey,TValue>

        public override TValue Get(TKey key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key), $"SimpleCalculatedCache.Get requires a non-null input.");
            TValue value;
            if (TryGetValue(key, out value)) return value;
            return Get(_calculator.Calculate(key));
        }

        #endregion
    }
}