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
using System.Linq;
using NUnit.Framework;

namespace Visyn.Collection.Test
{
    [TestFixture]
    public static class SimpleCacheTests
    {
        public static readonly Func<string, int> TestStrlenFunc = key => key.Length;

        public static readonly Func<string, string> TestLengthStringFn = (input) => input.ToLower() == "null" ? null : input.Length.ToString();

        public static readonly Func<string, string> TestNullFunc = null;

        public static void CacheGet(this ICache<string, string> cache, string item)
        {
            bool contains = cache.ContainsKey(item);
            int countPrior = cache.Count;
            var result = cache.Get(item);
            if (item.ToLower() == "null") Assert.IsNull(result);
            else Assert.AreEqual(item.Length.ToString(), result);
            Assert.AreEqual(cache.Count, countPrior + (contains ? 0 : 1));
        }

        public static void CacheGet(this ICache<string, int> cache, string item)
        {
            bool contains = cache.ContainsKey(item);
            int countPrior = cache.Count;
            var result = cache.Get(item);
            Assert.AreEqual(item.Length, result);
            Assert.AreEqual(cache.Count, countPrior + (contains ? 0 : 1));
        }

        public static void VerifyCacheCount<TKey,TValue>(this ICache<TKey, TValue> cache, int expectedCount)
        {
            Assert.AreEqual(expectedCount, cache.Count);
            Assert.AreEqual(expectedCount, cache.Keys.Count());
            Assert.AreEqual(expectedCount, cache.Values.Count());
        }

        public static void VerifyCacheCount<TKey, TValue>(this ICacheHitStats cache, int expectedCount, int hits, int misses, int notFound)
        {
            ((ICache<TKey,TValue>)cache).VerifyCacheCount(expectedCount );
            Assert.AreEqual(expectedCount, cache.Entries);
            Assert.AreEqual(hits, cache.Hits,$"Hits: Expected:{hits} Actual:{cache.Hits}");
            Assert.AreEqual(misses, cache.Misses, $"Hits: Expected:{misses} Actual:{cache.Misses}");
            Assert.AreEqual(notFound, cache.NotFound, $"Hits: Expected:{notFound} Actual:{cache.NotFound}");
        }


        [Test]
        public static void StringStringTest()
        {
            var cache = new SimpleCache<string, string>(TestLengthStringFn);

            Assert.AreEqual(cache.GetMissingCacheItem, TestLengthStringFn);
            cache.VerifyCacheCount( 0);
            cache.CacheGet( "first");
            cache.VerifyCacheCount( 1);
            cache.CacheGet("second");
            cache.VerifyCacheCount( 2);
            cache.CacheGet("first");
            cache.VerifyCacheCount( 2);
            cache.CacheGet("null");
            cache.VerifyCacheCount( 3);
            Assert.Throws<ArgumentNullException>(() => cache.Get(null));
            cache.VerifyCacheCount( 3);
            cache.CacheGet("fourth");
            cache.VerifyCacheCount(4);
            cache.CacheGet("first");
            cache.VerifyCacheCount(4);
        }



        [Test]
        public static void SimpleCacheTest()
        {
            var cache = new SimpleCache<string, int>(TestStrlenFunc);

            cache.VerifyCacheCount(0);

            Assert.AreEqual(cache.Count, 0);
            cache.CacheGet("first");
            cache.VerifyCacheCount(1);
            Assert.Throws<ArgumentNullException>(() => cache.Get(null));
            cache.CacheGet("second");
            cache.VerifyCacheCount(2);
            cache.CacheGet("second");
            cache.VerifyCacheCount(2);
            cache.CacheGet("third");
            cache.VerifyCacheCount(3);

            Assert.AreEqual(cache["first"], "first".Length);
            Assert.AreEqual(cache["second"], "second".Length);
        }

        [Test]
        public static void NullConstructorTest()
        {   // No delegate, nothing is added to the cache
            var cacheNull = new SimpleCache<string, int>((Func<string, int>) null);
            Assert.AreEqual(cacheNull.Count, 0);
            Assert.AreEqual(cacheNull.Get("first"), 0);
            Assert.AreEqual(cacheNull.Count, 0);
            Assert.Throws<ArgumentNullException>(() => cacheNull.Get(null));
            Assert.AreEqual(cacheNull.Get("second"), 0);
            Assert.AreEqual(cacheNull.Count, 0);
        }

        [Test]
        public static void NullFunctionTest()
        {
            // Delegate return null, nothing is added to the cache
            var cache = new SimpleCache<string, string>(TestNullFunc);
            Assert.AreEqual(cache.Count, 0);
            Assert.AreEqual(cache.Get("first"), null);
            Assert.AreEqual(cache.Count, 0);
            Assert.AreEqual(cache.Get("second"), null);
            Assert.AreEqual(cache.Count, 0);
            Assert.Throws<ArgumentNullException>(() => cache.Get(null));
            Assert.AreEqual(cache.Get(3.ToString()), null);
            Assert.AreEqual(cache.Count, 0);
        }

#region IEnumerable Tests

        [Test]
        public static void GetEnumeratorTest()
        {
            var cache = new SimpleCache<string, int>(TestStrlenFunc);

            cache.VerifyCacheCount(0);

            Assert.AreEqual(cache.Count, 0);
            cache.CacheGet("first");
            cache.VerifyCacheCount(1);
            Assert.Throws<ArgumentNullException>(() => cache.Get(null));
            cache.CacheGet("second");
            cache.VerifyCacheCount(2);
            
            {
                var enumerator = cache.GetEnumerator();
                int count = 0;
                while (enumerator.MoveNext())
                {
                    var current = enumerator.Current;
                    Assert.IsTrue(cache.ContainsKey(current.Key));
                    count++;
                }
                Assert.AreEqual(2, count);
                cache.VerifyCacheCount(count);
            }
            {
                var enumerator = ((IEnumerable)cache).GetEnumerator();
                int count = 0;
                while (enumerator.MoveNext())
                {
                    var current = enumerator.Current;
                    Assert.IsTrue(current is KeyValuePair<string,int>);
                    Assert.IsTrue(cache.ContainsKey(((KeyValuePair<string, int>)current).Key));
                    count++;
                }
                Assert.AreEqual(2, count);
                cache.VerifyCacheCount(count);
            }
        }
#endregion
        [Test]
        public static void HitCountersTest( )
        {
            var cache = new SimpleCache<string, string>(TestLengthStringFn);

            Assert.AreEqual(cache.GetMissingCacheItem, TestLengthStringFn);
            cache.VerifyCacheCount<string, string>(0,0,0,0);
            cache.CacheGet("first");
            cache.VerifyCacheCount<string, string>(1,0,1,0);
            cache.CacheGet("second");
            cache.VerifyCacheCount<string, string>(2,0,2,0);
            cache.CacheGet("first");
            cache.VerifyCacheCount<string, string>(2,1,2,0);
            cache.CacheGet("null");
            cache.VerifyCacheCount<string, string>(3,1,3,1);
            Assert.Throws<ArgumentNullException>(() => cache.Get(null));
            cache.VerifyCacheCount<string, string>(3,1,3,1);
            cache.CacheGet("fourth");
            cache.VerifyCacheCount<string, string>(4,1,4,1);
            cache.CacheGet("first");
            cache.VerifyCacheCount<string, string>(4,2,4,1);
        }
    }
}
