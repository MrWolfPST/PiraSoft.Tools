using System.Data.Common;

namespace System.Data;

/// <summary>
/// A set of <see cref="DataTableExtensions"/> extension methods.
/// </summary>
public static class DataTableExtensions
{
    /// <summary>
    /// Adds rows in the target <see cref="DataTable"/> to match those in the data source.
    /// </summary>
    /// <param name="target">The <see cref="DataTable"/> where adds rows.</param>
    /// <param name="reader">The data source.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static async Task LoadAsync(this DataTable target, DbDataReader reader, CancellationToken cancellationToken = default)
    {
        var schema = await reader.GetColumnSchemaAsync(cancellationToken);

        if (schema == null)
            return;

        schema.ForEach(i => target.Columns.Add(new DataColumn()
        {
            AllowDBNull = i.AllowDBNull.GetValueOrDefault(),
            ColumnName = i.ColumnName,
            DataType = i.DataType,
            MaxLength = i.DataType == typeof(string) ? i.ColumnSize.GetValueOrDefault(-1) : -1,
            AutoIncrement = i.IsAutoIncrement.GetValueOrDefault(),
            ReadOnly = i.IsReadOnly.GetValueOrDefault(),
            Unique = i.IsUnique.GetValueOrDefault()
        }));

        while (await reader.ReadAsync(cancellationToken))
        {
            var row = target.NewRow();
            target.Columns.AsEnumerable().ForEach(i => row[i.ColumnName] = reader[i.ColumnName]);
            target.Rows.Add(row);
        }
    }
}
