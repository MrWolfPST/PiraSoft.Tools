﻿namespace System.Collections.Generic;

/// <summary>
/// A set of <see cref="IEnumerable{T}"/> extension methods
/// </summary>
public static class IEnumerableExtensions
{
    /// <summary>
    /// Performs the specified <see cref="Action{T}"/> on each element of the <see cref="IEnumerable{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <param name="target">Current <see cref="IEnumerable{T}"/>.</param>
    /// <param name="action">The <see cref="Action{T}"/> delegate to perform on each element of the <see cref="IEnumerable{T}"/>.</param>
    /// <exception cref="ArgumentNullException"><paramref name="action"/> is null.</exception>
    /// <exception cref="InvalidOperationException">An element in the collection has been modified.</exception>
    public static void ForEach<T>(this IEnumerable<T> target, Action<T> action)
        => target.ToList().ForEach(action);
}