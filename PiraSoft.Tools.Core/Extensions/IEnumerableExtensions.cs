namespace System.Collections.Generic
{
    public static class IEnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> target, Action<T> action)
        {
            foreach (var i in target)
                action(i);
        }
    }
}