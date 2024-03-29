﻿namespace System.Data.Common;

/// <summary>
/// A set of <see cref="DbParameterCollection"/> extension methods.
/// </summary>
public static class DbParameterCollectionExtensions
{
    /// <summary>
    /// Returns the input typed as <see cref="IEnumerable{T}"/>.
    /// </summary>
    /// <param name="target">The sequence to type as <see cref="IEnumerable{T}"/>.</param>
    /// <returns>The input sequence typed as <see cref="IEnumerable{T}"/>.</returns>
    public static IEnumerable<DbParameter> AsEnumerable(this DbParameterCollection target)
    {
        foreach (DbParameter item in target)
        {
            yield return item;
        }
    }
}