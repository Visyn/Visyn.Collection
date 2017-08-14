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
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Visyn.Collection
{
    public static class ConcurrentQueueExtensions
    {
        public static void Clear<T>(this ConcurrentQueue<T> queue)
        {
            if (queue == null) throw new NullReferenceException($"{nameof(ConcurrentQueueExtensions)}.{nameof(Clear)} {nameof(queue)} can not be null!");
            while (!queue.IsEmpty)
            {
                T item;
                queue.TryDequeue(out item);
            }
        }


        public static T Dequeue<T>(this ConcurrentQueue<T> queue)
        {
            if (queue == null) throw new NullReferenceException($"{nameof(ConcurrentQueueExtensions)}.{nameof(Dequeue)} {nameof(queue)} can not be null!");
            T result;
            return queue.TryDequeue(out result) ? result : default(T);
        }



        public static int TryDequeueAll<T>(this ConcurrentQueue<T> queue, out List<T> items)
        {
            if (queue == null) throw new NullReferenceException($"{nameof(ConcurrentQueueExtensions)}.{nameof(TryDequeueAll)} {nameof(queue)} can not be null!");

            int count = 0;
            items= new List<T>(queue.Count);
            while (!queue.IsEmpty)
            {
                T item;
                if (queue.TryDequeue(out item))
                {
                    items.Add(item);
                    count++;
                }
            }
            return count;
        }

        public static void AddRange<T>(this ConcurrentQueue<T> queue, IEnumerable<T> items )
        {
            if (queue == null) throw new NullReferenceException($"{nameof(ConcurrentQueueExtensions)}.{nameof(Clear)} {nameof(AddRange)} can not be null!");

            if (items == null) return;
            foreach(var item in items)
                queue.Enqueue(item);
        }
    }
}
