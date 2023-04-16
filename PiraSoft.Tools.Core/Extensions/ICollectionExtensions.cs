namespace PiraSoft.Tools.Core.Extensions;

/// <summary>
/// A set of <see cref="ICollection{T}"/> extension methods
/// </summary>
public static class ICollectionExtensions
{
    /// <summary>
    /// Add the element to the end of the current <see cref="ICollection{T}"/> and return it.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="target">Current <see cref="ICollection{T}"/>.</param>
    /// <param name="item">Elements to be added to the end of the <see cref="ICollection{T}"/>.</param>
    /// <returns>The added item.</returns>
    public static T AddAndReturn<T>(this ICollection<T> target, T item)
    {
        target.Add(item);
        return item;
    }
}
