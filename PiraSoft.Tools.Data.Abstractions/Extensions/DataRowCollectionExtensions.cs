namespace System.Data;

public static class DataRowCollectionExtensions
{
    public static IEnumerable<DataRow> AsEnumerable(this DataRowCollection target)
    {
        foreach (DataRow item in target)
        {
            yield return item;
        }
    }
}
