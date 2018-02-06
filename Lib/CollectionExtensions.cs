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
using System.Linq;
using System.Reflection;
using System.Text;
using Visyn.JetBrains;
using NotRecommended = System.ObsoleteAttribute;

namespace Visyn.Collection
{
    public static class CollectionExtensions
    {
        /// <summary>
        /// Adds the items to the collection.
        /// </summary>
        /// <typeparam name="T">Collection type</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="items">The items to add.</param>
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            foreach(var item in items) collection.Add(item);
        }


        /// <summary>
        /// Determines the type of the collection if possible.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <returns>Type. Generic type argument if list is generic, or type of first item if non-generic.</returns>
        public static Type CollectionType(this ICollection collection)
        {
            if (collection == null) return null;
            var type = collection.GetType();
            if(type.GetTypeInfo().IsGenericType && type.GenericTypeArguments.Length == 1)
            {
                return type.GenericTypeArguments[0];
            }
            return collection.FirstItem()?.GetType();
        }

        /// <summary>
        /// Determines the type of the collection if possible.
        /// </summary>
        /// <typeparam name="T">Collection type argument</typeparam>
        /// <param name="collection">The collection.</param>
        /// <returns>Type.</returns>
        public static Type CollectionType<T>(this Array collection) => CollectionType((ICollection)collection);

        /// <summary>
        /// Firsts the item.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <returns>System.Object.</returns>
        [CanBeNull]
        public static object FirstItem(this ICollection collection)
        {
            if (collection == null || collection.Count <= 0 ) return null;
            
            var iList = collection as IList;
            if (iList != null)
            {   // Implements IList, use indexer
                return iList[0];
            }
            // Use the slower way
            return collection.Cast<object>().FirstOrDefault();
        }

        /// <summary>
        ///   Returns a subvector extracted from the current vector.
        /// </summary>
        /// 
        /// <param name="source">The vector to return the subvector from.</param>
        /// <param name="indexes">Array of indices.</param>
        /// 
        public static IEnumerable<T> Get<T>(this IList<T> source, int[] indexes)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (indexes == null) throw new ArgumentNullException(nameof(indexes));

            return indexes.Select(index => source[index]);
        }

        public static List<TItem> Interleave<TItem>(IList<TItem> list, IList<TItem> other,TItem defaultIfMissing )
        {
            if(list == null) list = new List<TItem>();
            if(other == null) other = new List<TItem>();

            int count = Math.Max(list.Count, other.Count);
            var array = new TItem[count*2];
            if (list.Count == other.Count)
            {   // equal size lists, no need to Count for length exceeded
                for (int i = 0, j = 0; i < count; i++)
                {
                    array[j++] = list[i];
                    array[j++] = other[i];
                }
            }
            else
            {   // lists aren't the same length, fill empty spots with default item
                for (int i = 0, j = 0; i < count; i++)
                {
                    array[j++] = i < list.Count ? list[i] : defaultIfMissing;
                    array[j++] = i < other.Count ? other[i] : defaultIfMissing;
                }
            }
            return new List<TItem>(array);
        }

        [CanBeNull]
        [NotRecommended("Case to ICollection and use FirstItem(ICollection ..)")]
        public static object FirstItem<T>(this ICollection<T> collection)
        {
            return FirstItem((ICollection)collection);
        }

        [CanBeNull]
        [NotRecommended("Case to ICollection and use FirstItem(ICollection ..)")]
        public static object FirstItem<T>(this T[] collection)
        {
            if (collection == null || collection.Length <= 0) return null;
            return collection[0];
        }

        public static T[] FirstItems<T>(this T[] array, int count)
        {
            var result = new T[count];
            for (var i = 0; i < count; i++)
                result[i] = array[i];
            return result;
        }

        public static IEnumerable<T> FirstItems<T>(this IEnumerable<T> collection, int count)
        {
            var i = 0;
            foreach(var item in collection)
            {
                if (i++ < count) yield return item;
                else break;
            }
        }

        public static IEnumerable FirstItems(this IEnumerable collection, int count)
        {
            var i = 0;
            foreach (var item in collection)
            {
                if (i++ < count) yield return item;
                else break;
            }
        }

        [Obsolete("Use ToArray<object>() instead",false)]
        public static object[] ToObjectArray(this ICollection collection)
        {
            var count = collection?.Count ?? 0;
            var objects = new object[count];
            if (count == 0 || collection == null) return new object[] {};

            var index = 0;
            
            foreach (var item in collection)
            {
                objects[index++] = item;
            }
            return objects;
        }


        public static T[] ToArray<T>(this ICollection collection, Func<object, T> converter = null)
        {
            if (collection == null) return new T[] { };
            var objects = new T[collection.Count];
            var index = 0;

            foreach (var item in collection)
            {
                T value;
                if (converter != null) value = (item != null) ? converter.Invoke(item) : default(T);
                else value = (item != null) ? (T)item : default(T);
                objects[index++] = value;
            }
            return objects;
        }

        public static Tout[] ToArray<TIn,Tout>(this ICollection<TIn> collection, Func<TIn, Tout> converter = null)
        {
            if (collection == null) return new Tout[] { };
            var objects = new Tout[collection.Count];
            var index = 0;

            foreach (var item in collection)
            {
                Tout value = converter.Invoke(item);
                objects[index++] = value;
            }
            return objects;
        }

        public static IEnumerable<T[]> ToArraysBy<T>(this IEnumerable<T> source, int count, bool throwIfRemainder)
        {
            var  result = new T[count];
            var total = 0;
            var mod = 0;
            foreach (var item in source)
            {
                mod = total++ % count;
                result[mod] = item;
                if (mod != count - 1) continue;
                yield return result;
                result = new T[count];
            }
            if(mod != count-1) throw new IndexOutOfRangeException($"{nameof(source)} does not contain number of items evenly divisable by {count}. Items: {total} Remainder: {total%count}");
        }

        public static IEnumerable<TOut> ToEnumerable<TIn,TOut>(this IEnumerable<TIn> source ) where TIn : TOut => source.Select(item => (TOut) item);
        public static IEnumerable<TOut> ToEnumerable<TIn, TOut>(this IEnumerable<TIn> source, Func<TIn, TOut> converter) => source.Select(converter);

        /// <summary>
        /// Converts a collection to a new List of type <typeparamref name="TOutput" /> using the supplied Converter object-><typeparamref name="TOutput" />.
        /// </summary>
        /// <typeparam name="TOutput">The type of the t output.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="converter">The user supplied type converter.</param>
        /// <returns>List&lt;TOutput&gt;.</returns>
        /// <exception cref="Exception">A delegate callback may throws an exception (unchecked).</exception>
        public static List<TOutput> ToList<TOutput>(this IEnumerable collection, [NotNull] Func<object, TOutput> converter)
        {
            if (converter == null) return ToList<TOutput>(collection);
            return collection == null ? new List<TOutput>() : AsList<TOutput>(from object item in collection select converter.Invoke(item));
        }   

        /// <summary>
        /// Converts a collection to a new List of type <typeparamref name="T" /> 
        /// </summary>
        /// <typeparam name="T">The type of the output list.</typeparam>
        /// <param name="collection">The collection to add to list.</param>
        /// <returns>List&lt;T&gt;.</returns>
        /// <exception cref="ArgumentNullException">
        ///         <paramref name="collection" /> is null.</exception>
        [NotNull]
        public static List<T> ToList<T>([CanBeNull]this IEnumerable collection)
        {
            return collection == null ? new List<T>() : AsList<T>(from object item in collection select (T)item);
        }

        /// <summary>
        /// Converts a collection to a new List of type <typeparamref name="T" /> 
        /// </summary>
        /// <typeparam name="T">The type of the output list.</typeparam>
        /// <param name="collection">The collection to add to list.</param>
        /// <returns>List&lt;T&gt;.</returns>
        /// <exception cref="ArgumentNullException">
        ///         <paramref name="collection" /> is null.</exception>
        [NotNull]
        public static List<T> ToList<T>([CanBeNull]this ICollection collection) 
            => collection == null ? new List<T>() : 
                new List<T>(from object item in collection select (T)item);


        [NotNull]
        public static List<T> AsList<T>([CanBeNull]this IEnumerable<T> enumeration) 
            => enumeration == null ? new List<T>() : 
                new List<T>(enumeration);

        [NotNull]
        public static List<T> AsList<T>([CanBeNull]this IEnumerable enumeration) 
            => enumeration == null ? new List<T>() : 
                (from object item in enumeration select (T)item).ToList();

        [NotNull]
        public static List<T> AsList<T>([CanBeNull]this IEnumerable enumeration, [CanBeNull]Func<object, T> converter)
        {
            if (converter == null) return AsList<T>(enumeration);
            return enumeration == null ? new List<T>() : (from object item in enumeration select converter.Invoke(item)).ToList();
        }


        [NotNull]
        public static string Concatinate(this ICollection collection, string delimiter)
        {
            if (collection == null || collection.Count <= 0) return "";
            var builder = new StringBuilder();
            foreach (var item in collection) builder.Append(item).Append(delimiter);
            if (builder.Length > 0) builder.Length -= delimiter.Length;
            return builder.ToString();
        }

        [NotNull]
        public static string Concatinate(this ICollection collection, char delimiter)
        {
            if (collection == null || collection.Count <= 0) return "";
            var builder = new StringBuilder();
            foreach (var item in collection) builder.Append(item).Append(delimiter);
            builder.Length--;
            return builder.ToString();
        }

        [Obsolete("Use RemoveItems<T>",true)]
        public static IList RemoveElements(this IList list, IEnumerable remove)
        {
            if (remove == null) return list;
            foreach(var item in remove)
            {
                list.Remove(item);
            }
            return list;
        }

        [Obsolete("Use RemoveItems<T>",true)]
        public static IList<T> RemoveElements<T>(this IList<T> list, IEnumerable<T> remove)
        {
            if (remove == null) return list;
            foreach (var item in remove)
            {
                list.Remove(item);
            }
            return list;
        }

        public static IList RemoveItems(this IList list, IEnumerable remove)
        {
            if (remove == null) return list;
            
            foreach(var item in remove)
            {
                if(list.Contains(item))
                    list.Remove(item);
            }
            return list;
        }

        public static IList<T> RemoveItems<T>(this IList<T> list, IEnumerable<T> remove)
        {
            return (remove == null) ? list : list.RemoveItems(remove.ToList());
        }

        public static IList<T> RemoveItems<T>(this IList<T> list, IList<T> remove)
        {
            if (remove?.Count > 0) 
                list.RemoveAll(remove.Contains);
            return list;
        }

        public static int RemoveAll<T>(this IList<T> list, Predicate<T> match)
        {
            var removeIndex = new List<int>();
            // Find all list items for removal
            // Note list is indexed in reverse order so removal by index (below) occurs from largest index->smallest
            for(var i = list.Count-1; i>=0;i--)
            {
                if(match(list[i])) removeIndex.Add(i);
            }
            
            foreach (var i in removeIndex)
            {
                list.RemoveAt(i);
            }
            return removeIndex.Count;
        }

        public static bool CollectionsAreEqual(this IEnumerable a, IEnumerable b)
        {
            if(a == null) return b == null;
            if (b == null) return false;
            if (ReferenceEquals(a, b)) return true;

            if(a is ICollection && b is ICollection)
            {
                if (((ICollection)a).Count != ((ICollection)b).Count) return false;
            }

            var enumA = a.GetEnumerator();
            {
                var enumB = b.GetEnumerator();
                {
                    var aNext = enumA.MoveNext();
                    var bNext = enumB.MoveNext();
                    while (aNext && bNext)
                    {
                        if (!enumA.Current.Equals(enumB.Current)) return false;
                        aNext = enumA.MoveNext();
                        bNext = enumB.MoveNext();
                    }
                    if (aNext != bNext) // Lengths differ...
                        return false;
                }
            }
            return true;
            // Old implementation, not as clean and understandable
#if false
            var enumerator2 = b.GetEnumerator();
  //          enumerator2.Reset();
            foreach (var item1 in a)
            {
                if (!enumerator2.MoveNext()) return false;   // no more items to compare
                var item2 = enumerator2.Current;
                if (!Equals(item1, item2)) return false;
            }
            return enumerator2.MoveNext() == false;  // if enum2Next is true, there are more elements in the collection...
#endif
        }

        public static bool CollectionsAreEqual(this ICollection coll1, IList coll2)
        {
            if (coll1 == null) return coll2 == null;
            if (ReferenceEquals(coll1, coll2)) return true;
            if (coll1.Count != coll2.Count) return false;

            return !coll1.Cast<object>().Where((t, i) => !Equals(t,coll2[i])).Any();
        }


        public static bool Matches<T>(this ICollection<T> coll, T item, IComparer<T> comparer)
        {
            if(comparer == null) throw new NullReferenceException($"{nameof(Matches)} comparer can not be null");

            return coll.Any(c => comparer.Compare(c, item) == 0);
        }

        public static Dictionary<TKey,TValue> ToDictionary<TKey,TValue,T>(this IEnumerable<T> values, Func<T, KeyValuePair<TKey, TValue>> func)
        {
            var dict = new Dictionary<TKey,TValue>();
            foreach (var item in values)
            {
                dict.Add(func(item));
            }
            return dict;
        }

        public static IEnumerable<string> ToStringArray<T>(this IEnumerable<T> collection ) => collection.Select(item => item.ToString());
    }
}
