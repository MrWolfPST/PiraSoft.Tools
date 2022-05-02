namespace System.Data.Common;

/// <summary>
/// A set of <see cref="DbConnectionExtensions"/> extension methods.
/// </summary>
public static class DbConnectionExtensions
{
    /// <summary>
    /// Returns the string representation of <see cref="DbConnection"/> object.
    /// </summary>
    /// <param name="target">The <see cref="DbConnection"/> object.</param>
    /// <returns>The string representation of <see cref="DbConnection"/> object.</returns>
    public static string Dump(this DbConnection target)
        => $"{target.DataSource}.{target.Database}";
}