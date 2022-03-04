namespace System.Data.Common;

public static class DbParameterCollectionExtensions
{
    public static IEnumerable<DbParameter> AsEnumerable(this DbParameterCollection target)
    {
        foreach (DbParameter item in target)
        {
            yield return item;
        }
    }
}