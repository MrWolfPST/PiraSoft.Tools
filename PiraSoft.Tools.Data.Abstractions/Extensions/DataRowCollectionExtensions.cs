namespace System.Data;

/// <summary>
/// A set of <see cref="DataRowCollection"/> extension methods.
/// </summary>
public static class DataRowCollectionExtensions
{
    /// <summary>
    /// Returns the input typed as <see cref="IEnumerable{T}"/>.
    /// </summary>
    /// <param name="target">The sequence to type as <see cref="IEnumerable{T}"/>.</param>
    /// <returns>The input sequence typed as <see cref="IEnumerable{T}"/>.</returns>
    public static IEnumerable<DataRow> AsEnumerable(this DataRowCollection target)
    {
        foreach (DataRow item in target)
        {
            yield return item;
        }
    }
}
