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
using System.Diagnostics;

namespace Visyn.Collection
{
    public static class Enumeration
    {
        public static IEnumerable<int> CountTo(int count, int by = 1)
        {
            if (count > 0) for (var i = 0; i < count; i += by) yield return i;
            else if (count < 0) for (var i = 0; i > count; i -= by) yield return i;
        }

        public static IEnumerable<double> CountTo(double end, double by=1.0)
        {
            if (end > 0) for (var i = 0.0; i < end; i += by) yield return i;
            else if (end < 0) for (var i = 0.0; i > end; i -= by) yield return i;
        }

        public static IEnumerable<T> CountTo<T>(int count, Func<int, T> func) => CountTo<T>(count, 1, func);

        public static IEnumerable<T> CountTo<T>(int count, int by, Func<int,T> func )
        {
            if (count > 0) for (var i = 0; i < count; i += by) yield return func(i);
            else if (count < 0) for (var i = 0; i > count; i -= by) yield return func(i);
        }

        public static IEnumerable<T> CountTo<T>(int count, Func<int, IEnumerable<T>> func) => CountTo<T>(count, 1, func);
        public static IEnumerable<T> CountTo<T>(int count, int by, Func<int, IEnumerable<T>> func)
        {
            if (count > 0) for (var i = 0; i < count; i += by) foreach (var x in func(i)) yield return x;
                
            else if (count < 0) for (var i = 0; i > count; i -= by) foreach (var x in func(i)) yield return x;
        }

        public static IEnumerable<double> For(double start, double end, double by)
        {
            if (by < 0) Debug.Assert(start >= end);
            if (by > 0) Debug.Assert(start <= end);
            Debug.Assert(by >= 0 || start >= end);
            Debug.Assert(by <= 0 || start <= end);
            if (by > 0) for (var i = start; i < end; i += by) yield return i;
            else if (by < 0) for (var i = start; i > end; i += by) yield return i;
        }

        public static IEnumerable<int> For(int start, int end, int by)
        {
            if (by < 0) Debug.Assert(start >= end);
            if (by > 0) Debug.Assert(start <= end);
            Debug.Assert(by >= 0 || start >= end);
            Debug.Assert(by <= 0 || start <= end);
            if (by > 0) for (var i = start; i < end; i += by) yield return i;
            else if (by < 0) for (var i = start; i > end; i += by) yield return i;
        }
    }
}
