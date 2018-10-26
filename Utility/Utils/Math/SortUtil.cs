///====================================================================================================
///
///     SortUtil by
///     - CantyCanadian
///     - CShartStar
///
///====================================================================================================

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Canty
{
    /// <summary>
    /// To determine which sorting algorithm is perfect for you, use the provided Big O Notation provided and replace n with your average data set size.
    /// <para>Best means best case scenario (list is already sorted), average is the best value to compare, worst is if there is nothing properly sorted and everything is the furthest it can be from its right position, memory is the footprint of the algorithm on the ram.</para>
    /// <para>If an algorithm is stable, it means that equal values will stay in the same order as they were. Unstable algorithms can't assure that. Use stable sorting if you need to sort twice using two different parameters.</para>
    /// <para>When in doubt, use Quicksort. It's a popular sorting algorithm which is overall good, although, it is unstable.</para>
    /// </summary>
    public static class SortUtil
    {
        /// <summary>
        /// Information : https://en.wikipedia.org/wiki/Selection_sort | Best, Average and Worst : O(n^2). Memory : O(1). Unstable. | Highly inneficient. Good for small list when there isn't much memory available.
        /// </summary>
        /// <returns>Sorted collection.</returns>
        public static IList<T> SelectionSort<T>(IList<T> source) where T : IComparable
        {
            T temp;
            int minimumKey;

            for (int j = 0; j < source.Count - 1; j++)
            {
                minimumKey = j;

                for (int k = j + 1; k < source.Count; k++)
                {
                    if (SmallerThan(source[k], source[minimumKey]))
                    {
                        minimumKey = k;
                    }
                }

                temp = source[minimumKey];
                source[minimumKey] = source[j];
                source[j] = temp;
            }

            return source;
        }

        /// <summary>
        /// Information : https://en.wikipedia.org/wiki/Insertion_sort | Best : O(n). Average and Worst : O(n^2). Memory : O(1). Stable. | Highly inneficient. Good for adding values to pre-sorted arrays.
        /// </summary>
        /// <returns>Sorted collection.</returns>
        public static IList<T> InsertionSort<T>(IList<T> source) where T : IComparable
        {
            for (int i = 1; i < source.Count; i++)
            {
                T item = source[i];
                int insert = 0;

                for (int j = i - 1; j >= 0 && insert != 1;)
                {
                    if (SmallerThan(item, source[j]))
                    {
                        source[j + 1] = source[j];
                        j--;
                        source[j + 1] = item;
                    }
                    else
                    {
                        insert = 1;
                    }
                }
            }

            return source;
        }

        /// <summary>
        /// Information : https://en.wikipedia.org/wiki/Heapsort | Best : O(n). Average and Worst : O(nlogn). Memory : O(1). Unstable. | Efficient worst-case scenario. Great for larger, unsorted arrays.
        /// </summary>
        /// <returns>Sorted collection.</returns>
        public static IList<T> HeapSort<T>(IList<T> source) where T : IComparable
        {
            T[] result = new T[source.Count];
            source.CopyTo(result, 0);

            T temp;

            for (int i = 5; i >= 0; i--)
            {
                HeapAdjust(ref result, i, 9);
            }

            for (int i = 8; i >= 0; i--)
            {
                temp = source[i + 1];
                source[i + 1] = source[0];
                source[0] = temp;
                HeapAdjust(ref result, 0, i);
            }

            return result;
        }

        private static void HeapAdjust<T>(ref T[] source, int i, int n) where T : IComparable
        {
            T temp = source[i];
            int j = 2 * i;
            while (j <= n)
            {
                if (j < n && SmallerThan(source[j], source[j + 1]))
                {
                    j++;
                }

                if (GreaterThanOrEqual(temp, source[j]))
                {
                    break;
                }

                source[j / 2] = source[j];
                j *= 2;
            }

            source[j / 2] = temp;
        }

        /// <summary>
        /// Information : https://en.wikipedia.org/wiki/Merge_sort | Best, Average and Worst : O(nlogn). Memory : O(n). Stable. | Same efficiency accross the board, except in memory. Great for sorting where memory isn't an issue. Also, perfect for parallelization.
        /// </summary>
        /// <returns>Sorted collection.</returns>
        public static IList<T> MergeSort<T>(IList<T> source) where T : IComparable
        {
            T[] result = new T[source.Count];
            source.CopyTo(result, 0);
            MergeRecursive(ref result, 0, source.Count - 1);
            return result;
        }

        private static void MergeRecursive<T>(ref T[] source, int left, int right) where T : IComparable
        {
            int mid;

            if (right > left)
            {
                mid = (right + left) / 2;
                MergeRecursive(ref source, left, mid);
                MergeRecursive(ref source, (mid + 1), right);

                MergeGroup(ref source, left, (mid + 1), right);
            }
        }

        private static void MergeGroup<T>(ref T[] source, int left, int mid, int right) where T : IComparable
        {
            int leftEnd = mid - 1;
            int numberElements = right - left + 1;
            int tempPosition = 0;
            T[] temp = new T[numberElements];

            while ((left <= leftEnd) && (mid <= right))
            {
                if (SmallerThanOrEqual(source[left], source[mid]))
                {
                    temp[tempPosition++] = source[left++];
                }
                else
                {
                    temp[tempPosition++] = source[mid++];
                }
            }

            while (left <= leftEnd)
            {
                temp[tempPosition++] = source[left++];
            }

            while (mid <= right)
            {
                temp[tempPosition++] = source[mid++];
            }

            for (int i = 0; i < numberElements; i++)
            {
                tempPosition--;
                source[right] = temp[tempPosition];
                right--;
            }
        }

        /// <summary>
        /// Information : https://en.wikipedia.org/wiki/Quicksort | Best and Average : O(nlogn). Worst : O(n^2). Memory : O(n). Unstable. | Very efficient. When in doubt, use this one.
        /// </summary>
        /// <returns>Sorted collection.</returns>
        public static IList<T> QuickSort<T>(IList<T> source) where T : IComparable
        {
            T[] result = new T[source.Count];
            source.CopyTo(result, 0);
            QuickRecursive(ref result, 0, source.Count - 1);
            return result;
        }

        private static void QuickRecursive<T>(ref T[] source, int left, int right) where T : IComparable
        {
            if (left < right)
            {
                int pivot = QuickPartition(ref source, left, right);

                if (pivot > 1)
                {
                    QuickRecursive(ref source, left, pivot - 1);
                }

                if (pivot + 1 < right)
                {
                    QuickRecursive(ref source, pivot + 1, right);
                }
            }
        }

        private static int QuickPartition<T>(ref T[] source, int left, int right) where T : IComparable
        {
            T pivot = source[left];

            while (true)
            {
                while (SmallerThan(source[left], pivot))
                {
                    left++;
                }

                while (GreaterThan(source[right], pivot))
                {
                    right--;
                }

                if (left < right)
                {
                    T temp = source[right];
                    source[right] = source[left];
                    source[left] = temp;
                }
                else
                {
                    return right;
                }
            }
        }

        /// <summary>
        /// Information : https://en.wikipedia.org/wiki/Shellsort | Best : O(nlogn). Average, Worst : Unknown. Memory : O(1). Unstable. | Semi-efficient. Use mainly when memory is a luxury.
        /// </summary>
        /// <returns>Sorted collection.</returns>
        public static IList<T> ShellSort<T>(IList<T> source) where T : IComparable
        {
            int n = source.Count;
            int gap = n / 2;
            T temp;

            while (gap > 0)
            {
                for (int i = 0; i + gap < n; i++)
                {
                    int j = i + gap;
                    temp = source[j];

                    while (j - gap >= 0 && SmallerThan(temp, source[j - gap]))
                    {
                        source[j] = source[j - gap];
                        j = j - gap;
                    }

                    source[j] = temp;
                }

                gap = gap / 2;
            }

            return source;
        }

        // OEIS A108870 - Tokuda's Sequence
        private static int ShellSequence(int n)
        {
            return Mathf.CeilToInt((9 * (9 / 4) ^ n - 4) / 5);
        }

        /// <summary>
        /// Information : https://en.wikipedia.org/wiki/Comb_sort | Best : O(nlogn). Average, Worst : O(n^2). Memory : O(1). Unstable. | Semi-efficient. Good for quickly eliminating small values at the end of arrays.
        /// </summary>
        /// <returns>Sorted collection.</returns>
        public static IList<T> CombSort<T>(IList<T> source) where T : IComparable
        {
            double gap = source.Count;
            bool swaps = true;

            while (gap > 1 || swaps)
            {
                gap /= 1.247330950103979;

                if (gap < 1)
                {
                    gap = 1;
                }

                int i = 0;
                swaps = false;

                while (i + gap < source.Count)
                {
                    int igap = i + (int) gap;

                    if (GreaterThan(source[i], source[igap]))
                    {
                        T temp = source[i];
                        source[i] = source[igap];
                        source[igap] = temp;
                        swaps = true;
                    }

                    ++i;
                }
            }

            return source;
        }

        private static bool GreaterThan<T>(T lhs, T rhs) where T : IComparable
        {
            return lhs.CompareTo(rhs) > 0;
        }

        private static bool SmallerThan<T>(T lhs, T rhs) where T : IComparable
        {
            return lhs.CompareTo(rhs) < 0;
        }

        private static bool GreaterThanOrEqual<T>(T lhs, T rhs) where T : IComparable
        {
            return lhs.CompareTo(rhs) >= 0;
        }

        private static bool SmallerThanOrEqual<T>(T lhs, T rhs) where T : IComparable
        {
            return lhs.CompareTo(rhs) <= 0;
        }
    }
}