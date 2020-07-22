///====================================================================================================
///
///     StringExtension by
///     - CantyCanadian
///
///====================================================================================================

using System;
using UnityEngine;
#pragma warning disable 168

namespace Canty
{
    public static class StringExtension
    {
        /// <summary>
        /// Cut a string to the value of maxChars, adding ... at the end.
        /// </summary>
        /// <param name="maxChars">Maximum number of characters to truncate.</param>
        public static string Truncate(this string target, int maxChars)
        {
            return target.Length <= maxChars ? target : target.Substring(0, maxChars) + "...";
        }

        /// <summary>
        /// Calls the generic conversion method for IConvertible, converting a string to another simple type (like int, float, etc).
        /// </summary>
        /// <typeparam name="T">Return type (must inherit from IConvertible).</typeparam>
        public static T ConvertTo<T>(this string target) where T : IConvertible
        {
            try
            {
                if (!string.IsNullOrEmpty(target))
                {
                    return (T)Convert.ChangeType(target, typeof(T));
                }
                else
                {
                    return default(T);
                }
            }
            catch (FormatException e)
            {
                Debug.LogError($"StringExtension ConvertTo<{typeof(T).ToString()}> : Given value of [{target}] cannot be converted. Returning default value for type.");
                return default(T);
            }
        }
    }
}