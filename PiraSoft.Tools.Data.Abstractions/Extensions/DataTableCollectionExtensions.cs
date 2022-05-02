namespace System.Data;

/// <summary>
/// A set of <see cref="DataTableCollection"/> extension methods.
/// </summary>
public static class DataTableCollectionExtensions
{
    /// <summary>
    /// Returns the input typed as <see cref="IEnumerable{T}"/>.
    /// </summary>
    /// <param name="target">The sequence to type as <see cref="IEnumerable{T}"/>.</param>
    /// <returns>The input sequence typed as <see cref="IEnumerable{T}"/>.</returns>
    public static IEnumerable<DataTable> AsEnumerable(this DataTableCollection target)
    {
        foreach (DataTable item in target)
        {
            yield return item;
        }
    }
}
