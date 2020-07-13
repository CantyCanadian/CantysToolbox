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
        public static int[] RangePopulate(int min, int max)
        {
            int[] result = new int[max - min];

            for (int i = min; i < max; i++)
            {
                result[i - min] = i;
            }

            return result;
        }
    }
}
