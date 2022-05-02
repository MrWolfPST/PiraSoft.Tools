namespace System.Data;

/// <summary>
/// A set of <see cref="DataColumnCollection"/> extension methods.
/// </summary>
public static class DataColumnCollectionExtensions
{
    /// <summary>
    /// Returns the input typed as <see cref="IEnumerable{T}"/>.
    /// </summary>
    /// <param name="target">The sequence to type as <see cref="IEnumerable{T}"/>.</param>
    /// <returns>The input sequence typed as <see cref="IEnumerable{T}"/>.</returns>
    public static IEnumerable<DataColumn> AsEnumerable(this DataColumnCollection target)
    {
        foreach (DataColumn item in target)
        {
            yield return item;
        }
    }
}
