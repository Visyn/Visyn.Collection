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
using System.Linq;
using Visyn.Public.JetBrains;

namespace Visyn.Collection
{
    public static class DictionaryExtensions
    {
        /// <exclude />
        public const string KEY_VALUE_FORMAT_STRING = "{0} : {1}";

        #region Add item(s) to dictionary
        /// <summary>
        /// Add if item is missing, or replace item if it already exists in dictionary
        /// </summary>
        /// <typeparam name="TKey">Dictionary key type</typeparam>
        /// <typeparam name="TValue">Dictionary value type</typeparam>
        /// <param name="source">Dictionary to perform action on</param>
        /// <param name="key">Key of item to add or replace</param>
        /// <param name="value">Value to add</param>
        /// <returns>True if item already existed and was replced, otherwise false.</returns>
        public static bool AddOrReplace<TKey,TValue>(this IDictionary<TKey, TValue> source, TKey key, TValue value)
        {
            if (source.ContainsKey(key))
            {
                source[key] = value;
                return true;
            }
            source.Add(key,value);
            return false;
        }

        /// <summary>
        /// Add if item is missing, or replace item if it already exists in dictionary
        /// </summary>
        /// <typeparam name="TKey">The type of the t key.</typeparam>
        /// <typeparam name="TValue">The type of the t value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="item">The item.</param>
        public static void AddOrReplace<TKey, TValue>(this IDictionary<TKey, TValue> source, KeyValuePair<TKey, TValue> item)
        {
            if (source.ContainsKey(item.Key))
            {
                source[item.Key] = item.Value;
                return;
            }
            source.Add(item);
        }

        /// <summary>
        /// Add each item is missing, or replace item if it already exists in dictionary
        /// </summary>
        /// <typeparam name="TKey">The type of the t key.</typeparam>
        /// <typeparam name="TValue">The type of the t value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="values">The values.</param>
        public static void AddOrReplace<TKey, TValue>(this IDictionary<TKey, TValue> source, IEnumerable<KeyValuePair<TKey, TValue>> values )
        {
            foreach (var item in values)
            {
                AddOrReplace(source, item);
            }
        }

        /// <summary>
        /// Add if item is missing from dictionary. Does nothing if dictionary is null, or item is not present.
        /// </summary>
        /// <typeparam name="TKey">The type of the t key.</typeparam>
        /// <typeparam name="TValue">The type of the t value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public static void AddIfNotPresent<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, TValue value)
        {
            if (source == null || source.ContainsKey(key)) return;
            source.Add(key,value);
        }

        /// <summary>
        /// Add if item is missing from dictionary. Does nothing if dictionary is null, or item is not present.
        /// </summary>
        /// <typeparam name="TKey">The type of the t key.</typeparam>
        /// <typeparam name="TValue">The type of the t value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="keyValuePairs">KVP to add to dictionary</param>
        public static void AddIfNotPresent<TKey, TValue>(this IDictionary<TKey, TValue> source, IEnumerable<KeyValuePair<TKey,TValue>> keyValuePairs)
        {
            if (source == null) return;
            foreach (var kvp in keyValuePairs)
            {
                if (source.ContainsKey(kvp.Key)) continue;
                source.Add(kvp.Key, kvp.Value);
            }
        }

        /// <summary>
        /// Add if item produced by the valueGetter function if it is missing from dictionary. 
        /// Does nothing if dictionary is null, or item is not present.
        /// </summary>
        /// <typeparam name="TKey">The type of the t key.</typeparam>
        /// <typeparam name="TValue">The type of the t value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="key">The key.</param>
        /// <param name="valueGetter">The value getter.</param>
        public static void AddIfNotPresent<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, [NotNull] Func<TKey, TValue> valueGetter)
        {
            if (source == null || source.ContainsKey(key)) return;
            source.Add(key, valueGetter(key));
        }

        /// <summary>
        /// Adds the range of items to the dictionary.  Throws exception if item is already present
        /// </summary>
        /// <typeparam name="TKey">The type of the t key.</typeparam>
        /// <typeparam name="TValue">The type of the t value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="items">The items.</param>
        /// <exception cref="System.ArgumentException"></exception>
        public static void AddRange<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, IEnumerable<KeyValuePair<TKey, TValue>> items)
        {
            if (items == null || dictionary == null) return;
            foreach (var item in items)
            {
                if(dictionary.ContainsKey(item.Key))
                {
                    throw new ArgumentException($"An argument with the same key already exists. Key={item.Key}");
                }
                dictionary.Add(item);
            }
        }

        /// <summary>
        /// Adds the range of items to the dictionary.  Throws exception if item is already present
        /// </summary>
        /// <typeparam name="TKey">The type of the t key.</typeparam>
        /// <typeparam name="TValue">The type of the t value.</typeparam>
        /// <typeparam name="TValue2">The type of the t value2.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="items">The items.</param>
        public static void AddRange<TKey, TValue,TValue2>(this IDictionary<TKey, TValue> dictionary, IDictionary<TKey, TValue2> items)
             where TValue2 : TValue
        {
            if (items == null || dictionary == null) return;
            foreach (var key in items.Keys)
            {
                dictionary.Add(key, items[key]);
            }
        }


        /// <summary>
        /// Gets the specified value from the dictionary if present, otherwise adds the key value pair to the dictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of the t key.</typeparam>
        /// <typeparam name="TValue">The type of the t value.</typeparam>
        /// <param name="dictionary">The dictionary to operate on.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>Value</returns>
        public static TValue GetOrAdd<TKey,TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, value);
                return value;
            }
            // Note, this check can be removed if different values are acceptable...
            Debug.Assert(dictionary[key].Equals(value));
            return dictionary[key];
        }

        #endregion Add item(s) to dictionary

        #region Retrieve item(s) from dictionary        
        /// <summary>
        /// Gets the specified value from the dictionary if present, or defaultValue if dictionary null, or key not present.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The specified key for retrieval.</param>
        /// <param name="defaultValue">The default value, returned if dictionary is null, or item is not present.</param>
        /// <returns>Dictionary value if present, otherwise default value.</returns>
        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue)
        {
            if(dictionary == null) return defaultValue;
            TValue value = defaultValue;
            if(dictionary.TryGetValue(key,out value))
            {
                return value;
            }
            if (!value.Equals(defaultValue))
            {
                throw new Exception($"Verify DefaultValue {defaultValue} == Value {value}. Documentation indicated value will not be initialized... ");
            }
            return defaultValue;
        }

        public static string JoinValues<TKey, TValue>(this IDictionary<TKey, TValue> dict, IEnumerable<TKey> keys, string delimiter=",")
        {
            var values = keys.Select(key => dict[key]);
            return String.Join(delimiter, values);
        }
        #endregion Retrieve item(s) from dictionary

        #region Removed items from dictionary        
        /// <summary>
        /// Removes the items with the specified keys.
        /// </summary>
        /// <typeparam name="TKey">The type of the t key.</typeparam>
        /// <typeparam name="TValue">The type of the t value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="keys">The keys.</param>
        public static int Remove<TKey, TValue>(this IDictionary<TKey, TValue> source, IEnumerable<TKey> keys )
        {
            return keys.Count(key => source.Remove(key));
        }

        /// <summary>
        /// Removes the items with the specified keys.
        /// </summary>
        /// <typeparam name="TKey">The type of the t key.</typeparam>
        /// <typeparam name="TValue">The type of the t value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="keys">The keys.</param>
        public static int Remove<TKey, TValue>(this IDictionary<TKey, TValue> source, IList<TKey> keys)
        {
            return keys.Count(key => source.Remove(key));
        }

        #endregion Removed items from dictionary

        /// <summary>
        /// Formatteds the dictionary using the supplied format string
        /// </summary>
        /// <typeparam name="TKey">The type of the t key.</typeparam>
        /// <typeparam name="TValue">The type of the t value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="format">The format string used in string.Format(format,key,dictionary[key], key.GetType(),dictionary[key].GetType()).</param>
        /// <returns>List&lt;System.String&gt;.</returns>
        public static IEnumerable<string> FormattedText(this IDictionary dictionary,string format=KEY_VALUE_FORMAT_STRING)
        {
            return (from object key in dictionary?.Keys select string.Format(format, key, dictionary[key], key.GetType(), dictionary[key].GetType()));
        }

        public static void Add<TKey,TValue>(this Dictionary<TKey, TValue> dict, KeyValuePair<TKey, TValue> kvp )
        {
            dict?.Add(kvp.Key,kvp.Value);
        }

        public static Dictionary<TKey, TValue> ToDictionary<TKey,TValue>(this IDictionary<TKey, object> dict )
        {
            var result = new Dictionary<TKey, TValue>(dict.Count);
            foreach(var value in dict)
            {
                result.Add(value.Key,(TValue)value.Value);
            }
            return result;
        }

        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dict)
        {
            var result = new Dictionary<TKey, TValue>(dict.Count);
            foreach (var value in dict)
            {
                result.Add(value);
            }
            return result;
        }

        [Obsolete("Use Linq instead:   keys.Select(key => dict[key])",true)]
        public static IList<TResult> ToList<TKey,TResult>(this IDictionary<TKey, TResult> dict, IEnumerable<TKey> keys)
        {
            return keys.Select(key => dict[key]).ToList();
        }

        [Obsolete("Use Linq instead:   keys.Select(key => dict[key]).Select(dummy => (TResult) dummy)", true)]
        public static IList<TResult> ToList<TKey, TValue, TResult>(this IDictionary<TKey, TValue> dict, IEnumerable<TKey> keys) where TValue : TResult
        {
            return keys.Select(key => dict[key]).Select(dummy => (TResult) dummy).ToList();
        }

        public static bool TryGet<TKey, TValue, TOut>(this IDictionary<TKey, TValue> dict, TKey key, out TOut value) where TOut : TValue
        {
            if (dict?.ContainsKey(key) == true)
            {
                var obj = dict[key];
                if (obj is TOut)
                {
                    value = (TOut)obj;
                    return true;
                }
            }
            value = default(TOut);
            return false;
        }
        public static bool TryGet<TKey, TValue, TOut>(this IReadOnlyDictionary<TKey, TValue> dict, TKey key, out TOut value) where TOut : TValue
        {
            if (dict?.ContainsKey(key) == true)
            {
                var obj = dict[key];
                if (obj is TOut)
                {
                    value = (TOut)obj;
                    return true;
                }
            }
            value = default(TOut);
            return false;
        }

        public static TOut TryGet<TKey, TValue, TOut>(this IDictionary<TKey, TValue> dict, TKey key, TOut defaultValue) where TOut : TValue
        {
            if (dict?.ContainsKey(key) == true)
            {
                var obj = dict[key];
                if (obj is TOut) return (TOut) obj;
            }
            return defaultValue;
        }

        public static TOut TryGet<TKey, TValue, TOut>(this IReadOnlyDictionary<TKey, TValue> dict, TKey key, TOut defaultValue) where TOut : TValue
        {
            if (dict?.ContainsKey(key) == true)
            {
                var obj = dict[key];
                if (obj is TOut) return (TOut) obj;
            }
            return defaultValue;
        }

        public static string UniqueKey<TValue>(this IReadOnlyDictionary<string, TValue> dict, string key)
        {
            if (dict == null) return key;
            var requested = key;
            var count = 1;
            while(!dict.ContainsKey(requested))
            {
                requested = $"{key} ({count++})";
            }
            return requested;
        }
    }
}
