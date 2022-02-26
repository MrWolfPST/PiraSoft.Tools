namespace System.Collections.Generic
{
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Merges dictionary with data of another dictionary
        /// </summary>
        /// <typeparam name="TKey">Type of dictionaries key</typeparam>
        /// <typeparam name="TValue">Type of dictionaries values</typeparam>
        /// <param name="target">The target <see cref="IDictionary{TKey, TValue}"/> of merge</param>
        /// <param name="other">The <see cref="IDictionary{TKey, TValue}"/> to merge</param>
        /// <param name="preserveDuplicates">If true, the duplicate items of <paramref name="target"/> will preserved, otherwhise will updated</param>
        /// <exception cref="ArgumentNullException">When <paramref name="target"/> or <paramref name="other"/> is null</exception>
        public static void Merge<TKey, TValue>(this IDictionary<TKey, TValue> target, IDictionary<TKey, TValue> other, bool preserveDuplicates = true)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            if(other == null)
                throw new ArgumentNullException(nameof(other));

            other.ForEach((i) => {
                if (!target.ContainsKey(i.Key))
                    target.Add(i.Key, i.Value);
                else if (!preserveDuplicates)
                    target[i.Key] = i.Value;
            });
        }
    }
}
