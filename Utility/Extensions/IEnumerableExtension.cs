///====================================================================================================
///
///     IEnumerableExtension by
///     - CantyCanadian
///
///====================================================================================================

using System.Collections.Generic;
using UnityEngine;

namespace Canty
{
    /// <summary>
    /// Contains utility functions that applies to any containers that can be iterated upon.
    /// </summary>
    public static class IEnumerableExtension
    {
        /// <summary>
        /// Applies a passed function to every member of the IEnumerable. Passed in function cannot change the item reference.
        /// </summary>
        /// <param name="action">Function with a single Item argument.</param>
        public static void DoOnAll<I>(this IEnumerable<I> target, System.Action<I> action)
        {
            foreach (I item in target)
            {
                action.Invoke(item);
            }
        }

        /// <summary>
        /// Applies a passed function to every member of the 2D IEnumerable. Passed in function cannot change the item reference.
        /// </summary>
        /// <param name="action">Function with a single Item argument.</param>
        public static void DoOnAll<I, J>(this IEnumerable<I> target, System.Func<J, J> action) where I : IEnumerable<J>
        {
            foreach (I itemX in target)
            {
                foreach (J itemY in itemX)
                {
                    action.Invoke(itemY);
                }
            }
        }

        /// <summary>
        /// Return a debug string containing every values, separated by commas.
        /// </summary>
        /// <returns>String containing every values.</returns>
        public static string ToDebugString<I>(this IEnumerable<I> target)
        {
            string result = "";

            foreach (I item in target)
            {
                result += item.ToString() + ", ";
            }

            result = result.Remove(result.Length - 3, 2);

            return result;
        }

        /// <summary>
        /// Removes any duplicates inside a IEnumerable.
        /// </summary>
        /// <returns>Modified object.</returns>
        public static IEnumerable<I> RemoveDuplicates<I>(this IEnumerable<I> target)
        {
            List<I> result = new List<I>();

            foreach (I item in target)
            {
                if (!result.Contains(item))
                {
                    result.Add(item);
                }
            }

            return result;
        }

        /// <summary>
        /// Extracts any duplicates inside a IEnumerable.
        /// </summary>
        /// <returns>IEnumerable containing any duplicates (but without any duplicate items).</returns>
        public static IEnumerable<I> ExtractDuplicates<I>(this IEnumerable<I> target)
        {
            Dictionary<I, bool> dupes = new Dictionary<I, bool>();

            foreach (I item in target)
            {
                if (dupes.ContainsKey(item))
                {
                    dupes[item] = true;
                }
                else
                {
                    dupes.Add(item, false);
                }
            }

            List<I> result = new List<I>();

            foreach (KeyValuePair<I, bool> kv in dupes)
            {
                if (kv.Value)
                {
                    result.Add(kv.Key);
                }
            }

            return result;
        }

        /// <summary>
        /// Extracts duplicates inside a IEnumerable. However, the item must be found over [dupeCount] times before being extracted.
        /// </summary>
        /// <param name="dupeCount">How many times the item needs to be found before being extracted.</param>
        /// <returns>IEnumerable containing duplicates (but without any duplicate items).</returns>
        public static IEnumerable<I> ExtractDuplicates<I>(this IEnumerable<I> target, int dupeCount)
        {
            Dictionary<I, int> dupes = new Dictionary<I, int>();

            foreach (I i in target)
            {
                if (dupes.ContainsKey(i))
                {
                    dupes[i]++;
                }
                else
                {
                    dupes.Add(i, 1);
                }
            }

            List<I> result = new List<I>();

            foreach (KeyValuePair<I, int> kv in dupes)
            {
                if (kv.Value >= dupeCount)
                {
                    result.Add(kv.Key);
                }
            }

            return result;
        }

        /// <summary>
        /// Utility function for the cases when using an IEnumerable containing components. Calls SetActive on the attached GameObject.
        /// </summary>
        /// <param name="flag">Flag passed to SetActive</param>
        public static void SetAllActive<I>(this IEnumerable<I> target, bool flag) where I : Component
        {
            foreach (I obj in target)
            {
                obj.gameObject.SetActive(flag);
            }
        }

        /// <summary>
        /// Utility function for the cases when using an IEnumerable containing game objects. Calls SetActive on each.
        /// </summary>
        /// <param name="flag">Flag passed to SetActive</param>
        public static void SetAllActive(this IEnumerable<GameObject> target, bool flag)
        {
            foreach (GameObject obj in target)
            {
                obj.SetActive(flag);
            }
        }
    }
}