using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Visyn.Core.Collection
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
