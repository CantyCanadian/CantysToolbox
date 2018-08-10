using System.Collections.Generic;
using UnityEngine;

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
}
