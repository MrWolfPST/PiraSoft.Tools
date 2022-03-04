namespace System.Data.Common;

public static class DbConnectionExtensions
{
    public static string Dump(this DbConnection target)
        => $"{target.DataSource}.{target.Database}";
}