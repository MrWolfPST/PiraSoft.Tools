namespace System.Data.Common;

public static class DBCommandExtensions
{
    public static string Dump(this DbCommand target)
#pragma warning disable CS8604 // Possible null reference argument.
        => $"{target.Connection.Dump()}.{target.CommandText} {string.Join(", ", from p in target.Parameters.AsEnumerable() where p.Dump() != null select p.Dump())}";
#pragma warning restore CS8604 // Possible null reference argument.
}
