namespace System.Data.Common;

/// <summary>
/// A set of <see cref="DbCommandExtensions"/> extension methods.
/// </summary>
public static class DbCommandExtensions
{
    /// <summary>
    /// Returns the string representation of <see cref="DbCommand"/> object.
    /// </summary>
    /// <param name="target">The <see cref="DbCommand"/> object.</param>
    /// <returns>The string representation of <see cref="DbCommand"/> object.</returns>
    public static string Dump(this DbCommand target)
    {
        var returnParameter = target.Parameters.AsEnumerable().FirstOrDefault(p => p.Direction == ParameterDirection.ReturnValue);

        return $"{target.Connection?.Dump()?.Append(" => ")}{returnParameter?.Dump()}{target.CommandText} {string.Join(", ", from p in target.Parameters.AsEnumerable() where p.Direction != ParameterDirection.ReturnValue && p.Dump() != null select p.Dump())}".Trim();
    }
}
