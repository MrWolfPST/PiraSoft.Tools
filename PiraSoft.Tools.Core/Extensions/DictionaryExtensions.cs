namespace System.Collections.Generic
{
    /// <summary>
    /// A set of <see cref="IDictionary{TKey, TValue}"/> extension methods
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Merges dictionary with data of another dictionary
        /// </summary>
        /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
        /// <param name="target">The target <see cref="IDictionary{TKey, TValue}"/> of merge.</param>
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
    }
}
