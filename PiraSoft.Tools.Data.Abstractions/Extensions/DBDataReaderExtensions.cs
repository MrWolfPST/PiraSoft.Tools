namespace System.Data.Common;

public static class DBDataReaderExtensions
{
    public static DataRow ToDataRow(this DbDataReader target)
    {
        var schema = target.GetColumnSchema();
        var dt = new DataTable();

        schema.ForEach(i => dt.Columns.Add(new DataColumn()
        {
            AllowDBNull = i.AllowDBNull.GetValueOrDefault(),
            ColumnName = i.ColumnName,
            DataType = i.DataType,
            MaxLength = i.ColumnSize.GetValueOrDefault(-1),
            AutoIncrement = i.IsAutoIncrement.GetValueOrDefault(),
            ReadOnly = i.IsReadOnly.GetValueOrDefault(),
            Unique = i.IsUnique.GetValueOrDefault()
        }));

        var retVal = dt.Rows.Add();

        schema.ForEach(i => retVal[i.ColumnName] = target[i.ColumnName]);

        return retVal;
    }
}
