namespace System.Collections.Generic
{
    /// <summary>
    /// A set of <see cref="IDictionary{TKey, TValue}"/> extension methods
    /// </summary>
    public static class IDictionaryExtensions
    {
        /// <summary>
        /// Merges current <see cref="IDictionary{TKey, TValue}"/> with data of another <see cref="IDictionary{TKey, TValue}"/>.
        /// </summary>
        /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
        /// <param name="target">Current <see cref="IDictionary{TKey, TValue}"/>.</param>
        /// <param name="other">The <see cref="IDictionary{TKey, TValue}"/> to merge.</param>
        /// <param name="preserveDuplicates">If true, the duplicate items of <paramref name="target"/> will preserved, otherwhise will updated.</param>
        /// <exception cref="ArgumentNullException">When <paramref name="target"/> or <paramref name="other"/> is null.</exception>
        public static void Merge<TKey, TValue>(this IDictionary<TKey, TValue> target, IDictionary<TKey, TValue> other, bool preserveDuplicates = true)
            => other.ForEach((i) =>
            {
                if (!target.ContainsKey(i.Key))
                {
                    target.Add(i.Key, i.Value);
                }
                else if (!preserveDuplicates)
                {
                    target[i.Key] = i.Value;
                }
            });

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
        /// <param name="target">Current <see cref="IDictionary{TKey, TValue}"/>.</param>
        /// <param name="key">The key whose value to get.</param>
        /// <returns>The value associated with the specified key, if the key is found; otherwise, the default value for the <typeparamref name="TValue"/> type.</returns>
        public static TValue? TryGetValue<TKey, TValue>(this IDictionary<TKey, TValue> target, TKey key)
            => target.TryGetValue(key, out var retVal) ? retVal : default;

        /// <summary>
        /// Sets the element with the specified key if exists; otherwhise, adds an element with the provided key and value.
        /// </summary>
        /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
        /// <param name="target">Current <see cref="IDictionary{TKey, TValue}"/>.</param>
        /// <param name="key">The key of the element to add or set.</param>
        /// <param name="value">The object to use as the value of the element to add or set.</param>
        /// <exception cref="ArgumentNullException">key is null.</exception>
        public static void AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> target, TKey key, TValue value)
        {
            if (target.ContainsKey(key))
            {
                target[key] = value;
            }
            else
            {
                target.Add(key, value);
            }
        }
    }
}
