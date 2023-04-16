namespace System.Collections.Generic;

/// <summary>
/// A set of <see cref="IListExtensions"/> extension methods
/// </summary>
public static class IListExtensions
{
    /// <summary>
    /// Creates an array from a <see cref="IList"/>.
    /// </summary>
    /// <typeparam name="T">The type of elements in the array.</typeparam>
    /// <param name="target">Current <see cref="IList"/>.</param>
    /// <returns>An array that contains the elements from the input sequence.</returns>
    public static Array ToArray<T>(this IList target)
        => target.ToArray(typeof(T));

    /// <summary>
    /// Creates an array from a <see cref="IList"/>.
    /// </summary>
    /// <param name="target">Current <see cref="IList"/>.</param>
    /// <param name="arrayType">The type of elements in the array.</param>
    /// <returns>An array that contains the elements from the input sequence.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="target"/> or <paramref name="arrayType"/> is null</exception>
    public static Array ToArray(this IList target, Type arrayType)
    {
        ArgumentNullException.ThrowIfNull(target);
        ArgumentNullException.ThrowIfNull(arrayType);
        
        var retVal = Array.CreateInstance(arrayType, target.Count);

        target.CopyTo(retVal, 0);

        return retVal;
    }

    /// <summary>
    /// Adds the elements of the specified collection to the end of the current <see cref="IList{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="target">Current <see cref="IList{T}"/>.</param>
    /// <param name="items">
    /// The collection whose elements should be added to the end of the current <see cref="IList{T}"/>.
    /// The collection itself cannot be null, but it can contain elements that are null, if type <typeparamref name="T"/> is a reference type.
    /// </param>
    public static void AddRange<T>(this IList<T> target, IEnumerable<T> items)
    {
        if (target is List<T> asList)
        {
            asList.AddRange(items);
        }
        else
        {
            items.ForEach(i => target.Add(i));
        }
    }

    /// <summary>
    /// Adds the elements of the specified collection to the end of the current <see cref="IList{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="target">Current <see cref="IList{T}"/>.</param>
    /// <param name="items">
    /// An array of <typeparamref name="T"/> that contains zero or more elements to be added to the end of the <see cref="IList{T}"/>.
    /// The array can contain elements that are null, if type <typeparamref name="T"/> is a reference type.
    /// </param>
    public static void AddRange<T>(this IList<T> target, params T[] items)
        => target.AddRange(items.AsEnumerable());

    /// <summary>
    /// Add the element to the end of the current <see cref="IList{T}"/> and return it.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="target">Current <see cref="IList{T}"/>.</param>
    /// <param name="item">Elements to be added to the end of the <see cref="IList{T}"/>.</param>
    /// <returns>The added item.</returns>
    public static T AddAndReturn<T>(this IList<T> target, T item)
    {
        target.Add(item);
        return item;
    }
}