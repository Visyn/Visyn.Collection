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

namespace Visyn.Collection.MoreLinq
{
    public static partial class MoreEnumerable
    {
        public static IEnumerable<T> TakeLast<T>(this IEnumerable<T> en, int count)
        {
            return en.ToList<T>().TakeLast(count);
        }

        public static IEnumerable<T> TakeLast<T>(this IList<T> list, int count)
        {
            int start = Math.Max(list.Count - count,0);
            for(int i=start;i<list.Count;i++)
            {
                yield return list[i];
            }
        }

        public static IEnumerable<T> TakeLast<T>(this T[] array, int count)
        {
            int start = Math.Max(array.Length - count, 0);
            for (int i = start; i < array.Length; i++)
            {
                yield return array[i];
            }
        }
    }
}
