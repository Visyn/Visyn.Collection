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
using System.Collections.ObjectModel;
using System.Linq;

namespace Visyn.Collection
{
    public static class ListExtensions
    {
        public static void AddRange<T>(this IList<T> list, IEnumerable<T> collection)
        {
            foreach (var item in collection)
            {
                list.Add(item);
            }
        }

        public static void AddRange(this IList list, IEnumerable collection)
        {
            foreach (var item in collection)
            {
                list.Add(item);
            }
        }


        public static IList<T> AddIfNotPresent<T>(this IList<T> list, IEnumerable<T> newItems)
        {
            if (newItems == null) return list;
            foreach (var item in newItems)
            {
                if (list.Contains(item)) continue;
                list.Add(item);
            }
            return list;
        }



        public static IList<T> AddIfNotPresent<T>(this IList<T> list, T newItem)
        {
            if (!list.Contains(newItem))  list.Add(newItem);
            return list;
        }
		
        public static ReadOnlyCollection<T> AsReadOnly<T>(this IList<T> list) => new ReadOnlyCollection<T>(list);

        [Obsolete("Use GetIndexes<T> instead. Name changed for clarity!",true)]
        public static List<T> Get<T>(this IList<T> list, IEnumerable<int> indexes) => new List<T>(indexes.Select(index => list[index]));
        public static IEnumerable<T> GetIndexes<T>(this IList<T> list, IEnumerable<int> indexes) => (indexes.Select(index => list[index]));

        public static List<T> AddIfNotNull<T>(this List<T> list, T newItem)
        {
            if (newItem != null)  list.Add(newItem);
            return list;
        }

        public static List<T> AddIfNotNull<T>(this List<T> list, IEnumerable<T> newItems)
        {
            if (newItems != null)
            {
                list.AddRange(newItems.Where(item => item != null));
            }
            return list;
        }

        public static bool TryAdd<T>(this List<T> list, T newItem)
        {
            if (list.Contains(newItem)) return false;
            list.Add(newItem);
            return true;
        }

        public static T[] ToArray<T>(this IList<T> list )
        {
            var count = list.Count;
            var result = new T[count];
 
            for(var i=0;i< count; i++)
            {
                result[i] = list[i];
            }
            return result;
        }
    }
}
