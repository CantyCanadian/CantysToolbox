///====================================================================================================
///
///     ICollectionExtension by
///     - CantyCanadian
///
///====================================================================================================

using System.Collections.Generic;
using UnityEngine;

namespace Canty
{
    /// <summary>
    /// Inherits from IEnumerable. Contains utility functions that applies to any containers that can be counted.
    /// </summary>
    public static class ICollectionExtension
    {
        /// <summary>
        /// Gets a random object from the ICollection.
        /// </summary>
        /// <returns>Random object.</returns>
        public static I GetRandom<I>(this ICollection<I> target)
        {
            int index = Random.Range(0, target.Count);
            I result = default(I);

            foreach (I item in target)
            {
                if (index == 0)
                {
                    result = item;
                }
                else
                {
                    index--;
                }
            }

            return result;
        }

        /// <summary>
        /// Converts ICollection to array.
        /// </summary>
        /// <returns>Array containing values inside ICollection.</returns>
        public static I[] ToArray<I>(this ICollection<I> target)
        {
            I[] result = new I[target.Count];
            target.CopyTo(result, 0);
            return result;
        }

        /// <summary>
        /// Converts the ICollection from type I to type NI using a passed in conversion function and returns it as the passed in generic ICollection list.
        /// </summary>
        /// <param name="valueConverter">Conversion function taking a item I and returning an item NI.</param>
        /// <returns>IEnumerable containing original list, but converted.</returns>
        public static NC ConvertUsing<I, NI, NC>(this ICollection<I> target, System.Func<I, NI> valueConverter)
            where NC : ICollection<NI>, new()
        {
            NC result = new NC();

            foreach (I value in target)
            {
                result.Add(valueConverter.Invoke(value));
            }

            return result;
        }
    }
}