namespace System.Data.Common;

public static class DbCommandExtensions
{
    public static string Dump(this DbCommand target)
    {
        var returnParameter = target.Parameters.AsEnumerable().FirstOrDefault(p => p.Direction == ParameterDirection.ReturnValue);

        return $"{target.Connection?.Dump()?.Append(" => ")}{returnParameter?.Dump()}{target.CommandText} {string.Join(", ", from p in target.Parameters.AsEnumerable() where p.Direction != ParameterDirection.ReturnValue && p.Dump() != null select p.Dump())}".Trim();
    }
}
