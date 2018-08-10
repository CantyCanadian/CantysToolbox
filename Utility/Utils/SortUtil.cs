using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// To determine which sorting algorithm is perfect for you, use the provided Big O Notation provided and replace n with your average data set size.
/// Best means best case scenario (list is already sorted), average is the best value to compare, worst is if there is nothing properly sorted, memory is the footprint of the algorithm on the ram.
/// </summary>
public class SortUtil : MonoBehaviour
{
    public static void SelectionSort<T>(ref T[] source) where T : IComparable
    {
        T temp;
        int minimumKey;

        for (int j = 0; j < source.Length - 1; j++)
        {
            minimumKey = j;

            for (int k = j + 1; k < source.Length; k++)
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
    }

    public static void InsertionSort<T>(ref T[] source) where T : IComparable
    {
        for (int i = 1; i < source.Length; i++)
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
    }

    public static void HeapSort<T>(ref T[] source) where T : IComparable
    {
        T temp;

        for (int i = 5; i >= 0; i--)
        {
            HeapAdjust(ref source, i, 9);
        }

        for (int i = 8; i >= 0; i--)
        {
            temp = source[i + 1];
            source[i + 1] = source[0];
            source[0] = temp;
            HeapAdjust(ref source, 0, i);
        }
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

    public static void MergeSort<T>(ref T[] source) where T : IComparable
    {
        MergeRecursive(ref source, 0, source.Length - 1);
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
        int tempPosition = left;
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
            source[right] = temp[right];
            right--;
        }
    }

    public static void QuickSort<T>(ref T[] source, int left, int right) where T : IComparable
    {
        QuickRecursive(ref source, 0, source.Length - 1);
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

    public static void BubbleSort<T>(ref T[] source) where T : IComparable
    {
        T temp;

        for (int j = 0; j <= source.Length - 2; j++)
        {
            for (int i = 0; i <= source.Length - 2; i++)
            {
                if (GreaterThan(source[i], source[i + 1]))
                {
                    temp = source[i + 1];
                    source[i + 1] = source[i];
                    source[i] = temp;
                }
            }
        }
    }

    public static void ShellSort<T>(ref T[] source) where T : IComparable
    {
        int n = source.Length;
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
    }

    public static void CombSort<T>(ref T[] source) where T : IComparable
    {

    }

    public static void BucketSort<T>(ref T[] source) where T : IComparable
    {

    }

    public static void RadixSort<T>(ref T[] source) where T : IComparable
    {

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
