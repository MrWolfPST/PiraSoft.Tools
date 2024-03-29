﻿namespace PiraSoft.Tools.Core.Extensions;

/// <summary>
/// A set of <see cref="Array"/> extension methods
/// </summary>
public static partial class ArrayExtensions
{
    /// <summary>
    /// Searches for the specified object and returns the index of the first occurrence within the entire one-dimensional .
    /// </summary>
    /// <param name="array">Current <see cref="Array"/>.</param>
    /// <param name="value">The object to locate in.</param>
    /// <returns>The index of the first occurrence of within the entire, if found; otherwise, the lower bound of the array minus 1.</returns>
    public static int IndexOf(this Array array, object value)
        => Array.IndexOf(array, value);

    /// <summary>
    /// Searches for the specified object and returns the index of the first occurrence within the range of elements in the one-dimensional  that extends from the specified index to the last element.
    /// </summary>
    /// <param name="array">Current <see cref="Array"/>.</param>
    /// <param name="value">The object to locate in .</param>
    /// <param name="startIndex">The starting index of the search. 0 (zero) is valid in an empty array.</param>
    /// <returns>The index of the first occurrence of  within the range of elements in  that extends from  to the last element, if found; otherwise, the lower bound of the array minus 1.</returns>
    public static int IndexOf(this Array array, object value, int startIndex)
        => Array.IndexOf(array, value, startIndex);

    /// <summary>
    /// Searches for the specified object and returns the index of the first occurrence within the range of elements in the one-dimensional  that starts at the specified index and contains the specified number of elements.
    /// </summary>
    /// <param name="array">Current <see cref="Array"/>.</param>
    /// <param name="value">The object to locate in .</param>
    /// <param name="startIndex">The starting index of the search. 0 (zero) is valid in an empty array.</param>
    /// <param name="count">The number of elements in the section to search.</param>
    /// <returns>The index of the first occurrence of  within the range of elements in  that starts at  and contains the number of elements specified in , if found; otherwise, the lower bound of the array minus 1.</returns>
    public static int IndexOf(this Array array, object value, int startIndex, int count)
        => Array.IndexOf(array, value, startIndex, count);
}
