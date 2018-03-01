using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
