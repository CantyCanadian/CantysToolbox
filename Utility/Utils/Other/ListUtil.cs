///====================================================================================================
///
///     ListUtil by
///     - CantyCanadian
///
///====================================================================================================

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Canty
{
    public static class ListUtil
    {
        /// <summary>
        /// Create a generic list containing every integer values between min (inclusive) and max (exclusive).
        /// </summary>
        /// <param name="min">Minimum value (inclusive).</param>
        /// <param name="max">Maximum value (exclusive).</param>
        /// <returns>Generic list with all the integer values.</returns>
        public static List<int> RangePopulate(int min, int max)
        {
            List<int> result = new List<int>();

            for (int i = min; i < max; i++)
            {
                result.Add(i);
            }

            return result;
        }
    }
}
