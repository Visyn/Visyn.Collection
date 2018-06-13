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

using System.Collections.Generic;

namespace Visyn.Collection.MoreLinq
{
    public static partial class MoreEnumerable
    {
        /// <summary>
        /// Concatenates the head item with the specified tail items.
        /// </summary>
        /// <typeparam name="T">Type of the sequence</typeparam>
        /// <param name="head">The head item.</param>
        /// <param name="tail">The tail sequence.</param>
        /// <returns>IEnumerable&lt;T&gt;.</returns>
        public static IEnumerable<T> Concat<T>(this T head, IEnumerable<T> tail)
        {
            yield return head;
            foreach (var item in tail) yield return item;
        }

        /// <summary>
        /// Concatenates the head items with the specified tail item.
        /// </summary>
        /// <typeparam name="T">Type of the sequence</typeparam>
        /// <param name="head">The head items.</param>
        /// <param name="tail">The tail item.</param>
        /// <returns>IEnumerable&lt;T&gt;.</returns>
        public static IEnumerable<T> Concat<T>(this IEnumerable<T> head, T tail)
        {
            foreach (var item in head) yield return item;
            yield return tail;
        }

        /// <summary>
        /// Concatenates the head items with the specified tail items into a single sequence.
        /// </summary>
        /// <typeparam name="T">Type of the sequence</typeparam>
        /// <param name="head">The head sequence.</param>
        /// <param name="tail">The tail sequence.</param>
        /// <returns>IEnumerable&lt;T&gt;.</returns>
        public static IEnumerable<T> Concat<T>(this IEnumerable<T> head, IEnumerable<T> tail)
        {
            foreach (var item in head) yield return item;
            if (tail != null)
            {
                foreach (var item in tail) yield return item;
            }
        }
    }
}
