using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Visyn.Collection.MoreLinq
{
    public static partial class MoreEnumerable
    {
        //
        // Summary:
        //     Returns a sequence consisting of the head element and the given tail elements.
        //
        // Parameters:
        //   head:
        //     Head element of the new sequence.
        //
        //   tail:
        //     All elements of the tail. Must not be null.
        //
        // Type parameters:
        //   T:
        //     Type of sequence
        //
        // Returns:
        //     A sequence consisting of the head elements and the given tail elements.
        //
        // Remarks:
        //     This operator uses deferred execution and streams its results.
        public static IEnumerable<T> Concat<T>(this T head, IEnumerable<T> tail)
        {
            yield return head;
            foreach (var item in tail) yield return item;
        }
 
        // Summary:
        //     Returns a sequence consisting of the head elements and the given tail element.
        //
        // Parameters:
        //   head:
        //     All elements of the head. Must not be null.
        //
        //   tail:
        //     Tail element of the new sequence.
        //
        // Type parameters:
        //   T:
        //     Type of sequence
        //
        // Returns:
        //     A sequence consisting of the head elements and the given tail element.
        //
        // Remarks:
        //     This operator uses deferred execution and streams its results.
        public static IEnumerable<T> Concat<T>(this IEnumerable<T> head, T tail)
        {
            foreach (var item in head) yield return item;
            yield return tail;
        }

        // Summary:
        //     Returns a sequence consisting of the head elements followed by the given tail element.
        //
        // Parameters:
        //   head:
        //     All elements of the head. Must not be null.
        //
        //   tail:
        //     Tail elements of the new sequence. Can be null.
        //
        // Type parameters:
        //   T:
        //     Type of sequence
        //
        // Returns:
        //     A sequence consisting of the head elements followed by the tail elements.
        //
        // Remarks:
        //     This operator uses deferred execution and streams its results.
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
