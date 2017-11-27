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
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;
using NUnit.Framework;

#pragma warning disable 618
namespace Visyn.Collection.Test
{
    [TestFixture]
    [SuppressMessage("ReSharper", "InvokeAsExtensionMethod")]
    public class CollectionExtensionsTests
    {
        private static object[] _objectArray => new object[] { "first", 1, 1.2, "another string", null, 7, 8, 9};
        private static object[] _objectArrayWithNulls => new object[] { "first", 1, null, 1.2, 12,"another string", null, 7, 9 };

        private static readonly List<Array> _validArrays = new List<Array>(new Array[] { _objectArray, _objectArrayWithNulls, _byteArray, _doubleArray });

        private static byte[] _byteArray => new byte[] {1, 2, 3, 4};
        private static double[] _doubleArray => new[] { 1.0, 2.0, 3.0, 4.0 };
        private static ICollection _nullICollection => null;
        private static ICollection _intArrayAsICollection => new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        private static ICollection _intListAsICollection => new List<int>(new[] { 1, 2, 3, 4, 5, 6 });

        private static readonly List<ICollection> _validICollectionsOfInts = new List<ICollection>(new[]  {
            _intArrayAsICollection, 
            _intListAsICollection
            });

        private static ICollection<int> _nullIntCollection => null;

        private static ICollection<int> _intListAsCollectionT => new List<int>(new [] { 1, 2, 3, 4, 5});
        private static ICollection<int> _intArrayAsCollectionT => new[] { 1, 2, 3, 4, 5 , 6, 7, 8 };

        private static readonly List<ICollection<int>> _validICollectionsInt = new List<ICollection<int>>(new []  {
            _intListAsCollectionT, 
            _intArrayAsCollectionT
            });


        [Test]
        public void CollectionTypeTest()
        {

            // null collections
            Assert.AreEqual(CollectionExtensions.CollectionType(_nullIntCollection as ICollection), null);
            Assert.AreEqual(CollectionExtensions.CollectionType(_nullICollection), null);

            Assert.AreEqual(_objectArray.CollectionType(), _objectArray[0].GetType());
            Assert.AreEqual(_objectArrayWithNulls.CollectionType(), _objectArrayWithNulls[0].GetType());

            // Valid ICollection of ints
            Assert.AreEqual(_intArrayAsICollection.CollectionType(), typeof(int));
            Assert.AreEqual(_intListAsICollection.CollectionType(), typeof(int));

            // ICollection<int>
            Assert.AreEqual((_nullIntCollection as ICollection).CollectionType(), null);
            
            Assert.AreEqual((_intListAsCollectionT as ICollection).CollectionType(), typeof(int));
            Assert.AreEqual((_intArrayAsCollectionT as ICollection).CollectionType(), typeof(int));
        }

        [Test]
        public void FirstItemTest()
        {
            // null collections
            Assert.AreEqual(CollectionExtensions.FirstItem(_nullIntCollection), null);
            Assert.AreEqual(CollectionExtensions.FirstItem(_nullICollection), null);

            // object arrays
            Assert.AreEqual(_objectArray.FirstItem(), "first");
            Assert.AreEqual(_objectArrayWithNulls.FirstItem(), "first");
            foreach(var array in _validArrays)
            {
                object first = array.Cast<object>().FirstOrDefault();
                Assert.AreEqual(array.FirstItem(), first);
            }

            Assert.AreEqual(_intListAsICollection.FirstItem(), 1);
            Assert.AreEqual(_intArrayAsICollection.FirstItem(), 1);


            foreach (ICollection collection in _validICollectionsOfInts)
            {
                var objects = collection.FirstItem();
                Assert.AreEqual(objects, 1);
            }

            foreach (ICollection<int> collection in _validICollectionsInt)
            {
                var objects = collection.FirstItem();
                Assert.AreEqual(objects, 1);
            }
        }

        [Test]
        public void ToObjectArrayTest()
        {
            // null collections
            Assert.AreEqual(CollectionExtensions.ToObjectArray(_nullIntCollection as ICollection), new object[] { });
            Assert.AreEqual(CollectionExtensions.ToObjectArray(_nullICollection), new object[] { });

            foreach (var array in _validArrays)
            {
                var objects2 = array.ToObjectArray();
                CollectionAssert.AreEquivalent(array, objects2);
            }

            // Valid ICollection of ints
            var objects = _intArrayAsICollection.ToObjectArray();
            CollectionAssert.AreEquivalent(_intArrayAsICollection, objects);

            objects = _intListAsICollection.ToObjectArray();
            CollectionAssert.AreEquivalent(_intListAsICollection, objects);

            foreach (ICollection collection in _validICollectionsOfInts)
            {
                collection.ToArray<object>();
                objects = collection.ToObjectArray();
                CollectionAssert.AllItemsAreInstancesOfType(objects, typeof(object));
                CollectionAssert.AreEquivalent(collection, objects);
            }

            foreach (ICollection<int> collection in _validICollectionsInt)
            {
                objects = (collection as ICollection).ToObjectArray();
                CollectionAssert.AllItemsAreInstancesOfType(objects, typeof(object));
                CollectionAssert.AreEquivalent(collection, objects);
            }
        }


        [Test]
        public void ToArrayTest()
        {
            // null collections
            Assert.AreEqual(CollectionExtensions.ToArray<object>(_nullIntCollection as ICollection), new object[] { });
            Assert.AreEqual(CollectionExtensions.ToArray<object>(_nullICollection), new object[] { });

            {
                // valid arrays
                object[] objects = CollectionExtensions.ToArray<object>(_objectArray);
                CollectionAssert.AreEquivalent(_objectArray, objects);
                objects = CollectionExtensions.ToArray<object>(_objectArrayWithNulls);
                CollectionAssert.AreEquivalent(_objectArrayWithNulls, objects);

                foreach (var coll in _validArrays)
                {
                    objects = CollectionExtensions.ToArray<object>(coll);
                    CollectionAssert.AreEquivalent(coll, objects);
                }
            }
            {
                // Valid ICollection of ints
                var objects = _intArrayAsICollection.ToArray<object>();
                CollectionAssert.AreEquivalent(_intArrayAsICollection, objects);

                objects = _intListAsICollection.ToArray<object>();
                CollectionAssert.AreEquivalent(_intListAsICollection, objects);
            }
            foreach (ICollection collection in _validICollectionsOfInts)
            {
                collection.ToArray<object>();
                var obj = collection.ToArray<object>();
                CollectionAssert.AllItemsAreInstancesOfType(obj, typeof(object));
                CollectionAssert.AreEquivalent(collection, obj);

                var ints = collection.ToArray<int>();
                CollectionAssert.AllItemsAreInstancesOfType(ints, typeof(int));
                CollectionAssert.AreEquivalent(collection, ints);

                long[] int64s=null;
                Assert.Throws<InvalidCastException>(() => { int64s = collection.ToArray<Int64>(); });
                int64s = collection.ToArray<Int64>(Convert.ToInt64);
                CollectionAssert.AllItemsAreInstancesOfType(int64s, typeof(Int64));
                CollectionAssert.AreEquivalent(collection, int64s);

                var strings = CollectionExtensions.ToArray<string>(collection, Convert.ToString);
                CollectionAssert.AllItemsAreInstancesOfType(strings, typeof(string));
                CollectionAssert.AllItemsAreInstancesOfType(strings, typeof(string));
                CollectionAssert.AllItemsAreUnique(collection);
                CollectionAssert.AllItemsAreNotNull(collection);
            }

            foreach (ICollection<int> collection in _validICollectionsInt)
            {
                var obj = (collection as ICollection).ToArray<object>();
                CollectionAssert.AllItemsAreInstancesOfType(obj, typeof(object));
                CollectionAssert.AreEquivalent(collection, obj);

                var ints = (collection as ICollection).ToArray<int>();
                CollectionAssert.AllItemsAreInstancesOfType(ints, typeof(int));
                CollectionAssert.AreEquivalent(collection, ints);

                long[] int64s=null;
                Assert.Throws<InvalidCastException>(() => { int64s = (collection as ICollection).ToArray<Int64>(); });
                int64s = (collection as ICollection).ToArray<Int64>(Convert.ToInt64);
                CollectionAssert.AllItemsAreInstancesOfType(int64s, typeof(Int64));
                CollectionAssert.AreEquivalent(collection, int64s);

                var strings = CollectionExtensions.ToArray<string>((collection as ICollection), Convert.ToString);
                CollectionAssert.AllItemsAreInstancesOfType(strings, typeof(string));
                CollectionAssert.AllItemsAreInstancesOfType(strings, typeof(string));
                CollectionAssert.AllItemsAreUnique(collection);
                CollectionAssert.AllItemsAreNotNull(collection);
            }
        }

        [Test]
        public void ToListTest()
        {
            // null collections
            Assert.AreEqual(CollectionExtensions.ToList<object>(_nullIntCollection as ICollection), new object[] { });
            Assert.AreEqual(CollectionExtensions.ToList<object>(_nullICollection), new object[] { });

            {
                // valid arrays
                var objects = CollectionExtensions.ToList<object>(_objectArray);
                CollectionAssert.AreEquivalent(_objectArray, objects);
                objects = CollectionExtensions.ToList<object>(_objectArrayWithNulls);
                CollectionAssert.AreEquivalent(_objectArrayWithNulls, objects);

                foreach (var coll in _validArrays)
                {
                    objects = CollectionExtensions.ToList<object>(coll);
                    CollectionAssert.AreEquivalent(coll, objects);
                    var strings = CollectionExtensions.ToList<string>(coll, Convert.ToString);
                    CollectionAssert.AllItemsAreInstancesOfType(strings,typeof(string));
                    CollectionAssert.AllItemsAreInstancesOfType(strings,typeof(string));
                }
            }
            {
                // Valid ICollection of ints
                var objects = _intArrayAsICollection.ToList<object>();
                CollectionAssert.AreEquivalent(_intArrayAsICollection, objects);

                objects = _intListAsICollection.ToList<object>();
                CollectionAssert.AreEquivalent(_intListAsICollection, objects);
            }
            foreach (ICollection collection in _validICollectionsOfInts)
            {
                collection.ToArray<object>();
                var obj = collection.ToList<object>();
                CollectionAssert.AllItemsAreInstancesOfType(obj, typeof(object));
                CollectionAssert.AreEquivalent(collection, obj);

                var ints = collection.ToList<int>();
                CollectionAssert.AllItemsAreInstancesOfType(ints, typeof(int));
                CollectionAssert.AreEquivalent(collection, ints);

                IList int64s = null;
                Assert.Throws<InvalidCastException>(() => { int64s = collection.ToList<Int64>(); });
                int64s = collection.ToList<Int64>(Convert.ToInt64);
                CollectionAssert.AllItemsAreInstancesOfType(int64s, typeof(Int64));
                CollectionAssert.AreEquivalent(collection, int64s);

                var strings = CollectionExtensions.ToList<string>(collection, Convert.ToString);
                CollectionAssert.AllItemsAreInstancesOfType(strings, typeof(string));
                CollectionAssert.AllItemsAreInstancesOfType(strings, typeof(string));
                CollectionAssert.AllItemsAreUnique(collection);
                CollectionAssert.AllItemsAreNotNull(collection);
            }

            foreach (ICollection<int> collection in _validICollectionsInt)
            {
                var obj = (collection as ICollection).ToList<object>();
                CollectionAssert.AllItemsAreInstancesOfType(obj, typeof(object));
                CollectionAssert.AreEquivalent(collection, obj);

                var ints = (collection as ICollection).ToList<int>();
                CollectionAssert.AllItemsAreInstancesOfType(ints, typeof(int));
                CollectionAssert.AreEquivalent(collection, ints);

                IList int64s = null;
                Assert.Throws<InvalidCastException>(() => { int64s = collection.ToList<Int64>(); });
                int64s = (collection as ICollection).ToList<Int64>(Convert.ToInt64);
                CollectionAssert.AllItemsAreInstancesOfType(int64s, typeof(Int64));
                CollectionAssert.AreEquivalent(collection, int64s);

                var strings = CollectionExtensions.ToList<string>(collection, Convert.ToString);
                CollectionAssert.AllItemsAreInstancesOfType(strings, typeof(string));
                CollectionAssert.AllItemsAreInstancesOfType(strings, typeof(string));
                CollectionAssert.AllItemsAreUnique(collection);
                CollectionAssert.AllItemsAreNotNull(collection);
            }
        }



        [Test]
        public void AsListTest()
        {
            // null collections
            Assert.AreEqual(CollectionExtensions.AsList<object>(_nullIntCollection as IEnumerable), new object[] { });
            Assert.AreEqual(CollectionExtensions.AsList<object>(_nullICollection), new object[] { });

            {
                // valid arrays
                var objects = CollectionExtensions.AsList<object>(_objectArray);
                CollectionAssert.AreEquivalent(_objectArray, objects);
                objects = CollectionExtensions.AsList<object>(_objectArrayWithNulls);
                CollectionAssert.AreEquivalent(_objectArrayWithNulls, objects);

                foreach (var coll in _validArrays)
                {
                    objects = CollectionExtensions.AsList<object>(coll);
                    CollectionAssert.AreEquivalent(coll, objects);
                    var strings = CollectionExtensions.AsList<string>(coll, Convert.ToString);
                    CollectionAssert.AllItemsAreInstancesOfType(strings, typeof(string));
                    CollectionAssert.AllItemsAreInstancesOfType(strings, typeof(string));
                }
            }
            {
                // Valid ICollection of ints
                var objects = _intArrayAsICollection.AsList<object>();
                CollectionAssert.AreEquivalent(_intArrayAsICollection, objects);

                objects = _intListAsICollection.AsList<object>();
                CollectionAssert.AreEquivalent(_intListAsICollection, objects);
            }
            foreach (ICollection collection in _validICollectionsOfInts)
            {
                collection.ToArray<object>();
                var obj = collection.AsList<object>();
                CollectionAssert.AllItemsAreInstancesOfType(obj, typeof(object));
                CollectionAssert.AreEquivalent(collection, obj);

                var ints = collection.AsList<int>();
                CollectionAssert.AllItemsAreInstancesOfType(ints, typeof(int));
                CollectionAssert.AreEquivalent(collection, ints);

                IList int64s = null;
                Assert.Throws<InvalidCastException>(() => { int64s = collection.AsList<Int64>(); });
                int64s = collection.AsList<Int64>(Convert.ToInt64);
                CollectionAssert.AllItemsAreInstancesOfType(int64s, typeof(Int64));
                CollectionAssert.AreEquivalent(collection, int64s);

                var strings = CollectionExtensions.AsList<string>(collection, Convert.ToString);
                CollectionAssert.AllItemsAreInstancesOfType(strings, typeof(string));
                CollectionAssert.AllItemsAreInstancesOfType(strings, typeof(string));
                CollectionAssert.AllItemsAreUnique(collection);
                CollectionAssert.AllItemsAreNotNull(collection);
            }

            foreach (ICollection<int> collection in _validICollectionsInt)
            {
                var obj = (collection as ICollection).AsList<object>();
                CollectionAssert.AllItemsAreInstancesOfType(obj, typeof(object));
                CollectionAssert.AreEquivalent(collection, obj);

                var ints = (collection as ICollection).AsList<int>();
                CollectionAssert.AllItemsAreInstancesOfType(ints, typeof(int));
                CollectionAssert.AreEquivalent(collection, ints);

                IList int64s = null;
                Assert.Throws<InvalidCastException>(() => { int64s = collection.AsList<Int64>(); });
                int64s = (collection as ICollection).AsList<Int64>(Convert.ToInt64);
                CollectionAssert.AllItemsAreInstancesOfType(int64s, typeof(Int64));
                CollectionAssert.AreEquivalent(collection, int64s);

                var strings = CollectionExtensions.AsList<string>(collection, Convert.ToString);
                CollectionAssert.AllItemsAreInstancesOfType(strings, typeof(string));
                CollectionAssert.AllItemsAreInstancesOfType(strings, typeof(string));
                CollectionAssert.AllItemsAreUnique(collection);
                CollectionAssert.AllItemsAreNotNull(collection);
            }
        }


        [Test]
        public void ConcatinateTest()
        {
            const string Delimiter = "$#";
            // null collections
            Assert.AreEqual(CollectionExtensions.Concatinate((_nullIntCollection as ICollection), Delimiter), "");
            Assert.AreEqual(CollectionExtensions.Concatinate(_nullICollection as ICollection, Delimiter), "");

            {
                // valid arrays
                var objects = CollectionExtensions.Concatinate(_objectArray,Delimiter);
                Assert.AreEqual(createDelimitedString(_objectArray,Delimiter), objects);
                objects = CollectionExtensions.Concatinate(_objectArrayWithNulls, Delimiter);
                Assert.AreEqual(createDelimitedString(_objectArrayWithNulls, Delimiter), objects);

                foreach (var coll in _validArrays)
                {
                    objects = CollectionExtensions.Concatinate(coll, Delimiter);
                    Assert.AreEqual(createDelimitedString(coll, Delimiter), objects);
                }
            }
            {
                // Valid ICollection of ints
                var stringResult = _intArrayAsICollection.Concatinate(Delimiter);
                Assert.AreEqual(createDelimitedString(_intArrayAsICollection, Delimiter), stringResult);

                stringResult = _intListAsICollection.Concatinate(Delimiter);
                Assert.AreEqual(createDelimitedString(_intListAsICollection, Delimiter), stringResult);
            }
            foreach (ICollection collection in _validICollectionsOfInts)
            {
                collection.ToArray<object>();
                var obj = collection.Concatinate(Delimiter);
                Assert.AreEqual(createDelimitedString(collection, Delimiter), obj);
            }

            foreach (ICollection<int> collection in _validICollectionsInt)
            {
                var obj = (collection as ICollection).Concatinate(Delimiter);
                Assert.AreEqual(createDelimitedString(collection, Delimiter), obj);
            }
        }


        [Test]
        public void RemoveElementsTest()
        {

        }

        [Test]
        public void CollectionsAreEqualTest()
        {
            // null collections
            Assert.AreEqual(CollectionExtensions.AsList<object>(_nullIntCollection as IEnumerable), new object[] { });
            Assert.AreEqual(CollectionExtensions.AsList<object>(_nullICollection), new object[] { });

            {
                // valid arrays
                var objects = CollectionExtensions.AsList<object>(_objectArray);
                CollectionAssert.AreEquivalent(_objectArray, objects);
                Assert.IsTrue(CollectionExtensions.CollectionsAreEqual(_objectArray, objects));
                Assert.IsTrue(objects.CollectionsAreEqual(_objectArray));
                Assert.IsTrue(_objectArray.CollectionsAreEqual(objects));

                objects = CollectionExtensions.AsList<object>(_objectArrayWithNulls);
                CollectionAssert.AreEquivalent(_objectArrayWithNulls, objects);
                Assert.IsTrue(CollectionExtensions.CollectionsAreEqual(_objectArrayWithNulls, objects));
                Assert.IsTrue(objects.CollectionsAreEqual(_objectArrayWithNulls));
                Assert.IsTrue(_objectArrayWithNulls.CollectionsAreEqual(objects));

                foreach (var coll in _validArrays)
                {
                    objects = CollectionExtensions.AsList<object>(coll);
                    CollectionAssert.AreEquivalent(coll, objects);
                    Assert.IsTrue(CollectionExtensions.CollectionsAreEqual(coll, objects));
                    Assert.IsTrue(objects.CollectionsAreEqual(coll));
                    Assert.IsTrue(coll.CollectionsAreEqual(objects));
                }
            }
            {
                // Valid ICollection of ints
                var objects = _intArrayAsICollection.AsList<object>();
                CollectionAssert.AreEquivalent(_intArrayAsICollection, objects);
                Assert.IsTrue(CollectionExtensions.CollectionsAreEqual(_intArrayAsICollection, objects));
                Assert.IsTrue(objects.CollectionsAreEqual(_intArrayAsICollection));
                Assert.IsTrue(_intArrayAsICollection.CollectionsAreEqual(objects));

                objects = _intListAsICollection.AsList<object>();
                CollectionAssert.AreEquivalent(_intListAsICollection, objects);
                Assert.IsTrue(CollectionExtensions.CollectionsAreEqual(_intListAsICollection, objects));
                Assert.IsTrue(objects.CollectionsAreEqual(_intListAsICollection));
                Assert.IsTrue(_intListAsICollection.CollectionsAreEqual(objects));
            }
            foreach (ICollection collection in _validICollectionsOfInts)
            {
                collection.ToArray<object>();
                var obj = collection.AsList<object>();
                CollectionAssert.AllItemsAreInstancesOfType(obj, typeof(object));
                CollectionAssert.AreEquivalent(collection, obj);
                Assert.IsTrue(CollectionExtensions.CollectionsAreEqual(collection, obj));
                Assert.IsTrue(obj.CollectionsAreEqual(collection));
                Assert.IsTrue(collection.CollectionsAreEqual(obj));
                Assert.IsTrue(CollectionExtensions.CollectionsAreEqual(collection, obj));
                Assert.IsTrue(obj.CollectionsAreEqual(collection));
                Assert.IsTrue(collection.CollectionsAreEqual(obj));
                obj.Add(obj[0]);
                Assert.IsFalse(collection.CollectionsAreEqual(obj),"Should fail due to length mis-match");
             
                var ints = collection.AsList<int>();
                CollectionAssert.AllItemsAreInstancesOfType(ints, typeof(int));
                CollectionAssert.AreEquivalent(collection, ints);
                Assert.IsTrue(CollectionExtensions.CollectionsAreEqual(collection, ints));
                Assert.IsTrue(ints.CollectionsAreEqual(collection));
                Assert.IsTrue(collection.CollectionsAreEqual(ints));

                IList int64s = null;
                Assert.Throws<InvalidCastException>(() => { int64s = collection.AsList<Int64>(); });
                int64s = collection.AsList<Int64>(Convert.ToInt64);
                CollectionAssert.AllItemsAreInstancesOfType(int64s, typeof(Int64));
                CollectionAssert.AreEquivalent(collection, int64s);
                Assert.IsFalse(CollectionExtensions.CollectionsAreEqual(collection, int64s));
                Assert.IsFalse(int64s.CollectionsAreEqual(collection));
                Assert.IsFalse(collection.CollectionsAreEqual(int64s));
            }

            foreach (ICollection<int> collection in _validICollectionsInt)
            {
                var obj = (collection as ICollection).AsList<object>();
                CollectionAssert.AllItemsAreInstancesOfType(obj, typeof(object));
                CollectionAssert.AreEquivalent(collection, obj);
                Assert.IsTrue(CollectionExtensions.CollectionsAreEqual(collection, obj));
                Assert.IsTrue(obj.CollectionsAreEqual(collection));
                Assert.IsTrue(collection.CollectionsAreEqual(obj));

                var ints = (collection as ICollection).AsList<int>();
                CollectionAssert.AllItemsAreInstancesOfType(ints, typeof(int));
                CollectionAssert.AreEquivalent(collection, ints);
                Assert.IsTrue(CollectionExtensions.CollectionsAreEqual(collection, ints));
                Assert.IsTrue(ints.CollectionsAreEqual(collection));
                Assert.IsTrue(collection.CollectionsAreEqual(ints));

                IList int64s = null;
                Assert.Throws<InvalidCastException>(() => { int64s = collection.AsList<Int64>(); });
                int64s = (collection as ICollection).AsList<Int64>(Convert.ToInt64);
                CollectionAssert.AllItemsAreInstancesOfType(int64s, typeof(Int64));
                CollectionAssert.AreEquivalent(collection, int64s);
                Assert.IsFalse(CollectionExtensions.CollectionsAreEqual(collection, int64s));
                Assert.IsFalse(int64s.CollectionsAreEqual(collection));
                Assert.IsFalse(collection.CollectionsAreEqual(int64s));

                var strings = CollectionExtensions.AsList<string>(collection, Convert.ToString);
                CollectionAssert.AllItemsAreInstancesOfType(strings, typeof(string));
                CollectionAssert.AllItemsAreNotNull(collection);
                Assert.IsFalse(CollectionExtensions.CollectionsAreEqual(collection, strings));
                Assert.IsFalse(strings.CollectionsAreEqual(collection));
                Assert.IsFalse(collection.CollectionsAreEqual(strings));
            }
        }

        [Test]
        public void CollectionsAreEqual_IEnumerable_int_Test()
        {
            Assert.IsTrue(Enumeration.CountTo(10).CollectionsAreEqual(Enumeration.CountTo(10)), "IEnumerable<int> 1...10");
            Assert.IsFalse(Enumeration.CountTo(9).CollectionsAreEqual(Enumeration.CountTo(10)), "IEnumerable<int> [9] != IEnumerable<int>[10]");
            Assert.IsFalse(Enumeration.CountTo(10).CollectionsAreEqual(Enumeration.CountTo(9)), "IEnumerable<int> [10] != IEnumerable<int>[9]");
        }
        [Test]
        public void CollectionsAreEqual_IEnumerable_double_Test()
        {
            Assert.IsTrue(Enumeration.CountTo(10.1).CollectionsAreEqual(Enumeration.CountTo(10.1)), "IEnumerable<int> 1...10");
            Assert.IsFalse(Enumeration.CountTo(9.0).CollectionsAreEqual(Enumeration.CountTo(10.0)), "IEnumerable<int> [9] != IEnumerable<int>[10]");
            Assert.IsFalse(Enumeration.CountTo(10.0).CollectionsAreEqual(Enumeration.CountTo(9.0)), "IEnumerable<int> [10] != IEnumerable<int>[9]");
        }
        [Test]
        public void CollectionsAreEqual_IEnumerable_string_Test()
        {
            var func = new Func<int,string>((d)=>d.ToString(CultureInfo.InvariantCulture));
            Assert.IsTrue(Enumeration.CountTo<string>(10,func).CollectionsAreEqual(Enumeration.CountTo(10,func)), "IEnumerable<int> 1...10");
            Assert.IsFalse(Enumeration.CountTo(9,func).CollectionsAreEqual(Enumeration.CountTo(10,func)), "IEnumerable<int> [9] != IEnumerable<int>[10]");
            Assert.IsFalse(Enumeration.CountTo(10,func).CollectionsAreEqual(Enumeration.CountTo(9,func)), "IEnumerable<int> [10] != IEnumerable<int>[9]");
        }

        private static string createDelimitedString(IEnumerable collection, string delimiter)
        {
            var builder = new StringBuilder(256);
            var str = collection.Cast<object>().Aggregate("", (current, item) => current + item?.ToString() + delimiter);
            builder.Append(str);
            builder.Length -= delimiter.Length;

            return builder.ToString();
        }
    }
}
