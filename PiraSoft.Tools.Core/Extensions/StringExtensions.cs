namespace System;

/// <summary>
/// A set of <see cref="string"/> extension methods
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Append the second string only if the start string is not empty.
    /// </summary>
    /// <param name="target">The start <see cref="string"/>.</param>
    /// <param name="second">The <see cref="string"/> to append.</param>
    /// <param name="isNullOrEmpty">If true, check if the start string is null or empty; otherwise, only if is null.</param>
    /// <returns>The concatenation of the start string and second string if start string is not empty; otherwise, null.</returns>
    public static string? Append(this string? target, string second, bool isNullOrEmpty = false)
        => target == null || (isNullOrEmpty && string.IsNullOrEmpty(target)) ? target : target + second;
}
