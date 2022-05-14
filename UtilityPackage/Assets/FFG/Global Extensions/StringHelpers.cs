using System.Collections;
using UnityEngine;

public static class StringHelpers
{
    /// <summary>
    /// Returns true if it's null or empty
    /// </summary>
    public static bool IsNullOrEmpty(this string thisString)
    {
        return string.IsNullOrEmpty(thisString);
    }

    /// <summary>
    /// Retunrs true if the string is valid
    /// </summary>
    public static bool IsValid(this string thisString)
    {
        return !string.IsNullOrEmpty(thisString);
    }
}
