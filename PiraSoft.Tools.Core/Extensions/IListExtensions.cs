namespace System.Collections.Generic
{
    /// <summary>
    /// A set of <see cref="IListExtensions"/> extension methods
    /// </summary>
    public static class IListExtensions
    {
        /// <summary>
        /// Adds the elements of the specified collection to the end of the <see cref="IList{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="target">The <see cref="IList{T}"/> where elements should be added.</param>
        /// <param name="items">
        /// The collection whose elements should be added to the end of the <see cref="IList{T}"/>.
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
        /// Adds the elements of the specified collection to the end of the <see cref="IList{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="target">The <see cref="IList{T}"/> where elements should be added.</param>
        /// <param name="items">
        /// An array of <typeparamref name="T"/> that contains zero or more elements to be added to the end of the <see cref="IList{T}"/>.
        /// The array can contain elements that are null, if type <typeparamref name="T"/> is a reference type.
        /// </param>
        public static void AddRange<T>(this IList<T> target, params T[] items)
            => target.AddRange(items.AsEnumerable());
    }
}