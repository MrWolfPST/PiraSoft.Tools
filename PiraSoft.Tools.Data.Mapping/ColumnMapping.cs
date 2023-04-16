using System.Data;
using System.Diagnostics;
using System.Reflection;

namespace PiraSoft.Tools.Data.Mapping;

/// <summary>
/// The class for column mapping
/// </summary>
public class ColumnMapping : MappingBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ColumnMapping"/> class.
    /// </summary>
    /// <param name="columnName">The name of the column.</param>
    /// <exception cref="ArgumentException"><paramref name="columnName"/> not contains a value.</exception>
    public ColumnMapping(string? columnName)
    {
        if (string.IsNullOrWhiteSpace(columnName))
        {
            throw new ArgumentException($"Parameter {nameof(columnName)} must contains a value.", nameof(columnName));
        }

        this.ColumnName = columnName;
    }

    /// <summary>
    /// The name of the column.
    /// </summary>
    public string ColumnName { get; internal set; }

    /// <summary>
    /// Map data into property of target.
    /// </summary>
    /// <param name="target">Target object to popolate.</param>
    /// <param name="propertyInfo"><see cref="PropertyInfo"/> that identity property to popolate.</param>
    /// <param name="row"><see cref="DataRow"/> that contains data.</param>
    /// <remarks>If a column with name specified in <see cref="ColumnName"/> was not found, a warning line will be write in debug output.</remarks>
    protected internal override void Map(object target, PropertyInfo propertyInfo, DataRow row)
    {
        if (row.Table.Columns.Contains(this.ColumnName))
        {
            this.SetValue(target, propertyInfo, row[this.ColumnName]);
        }
        else
        {
            Debug.WriteLine($"Warning: Unable find column {this.ColumnName} for property {propertyInfo.DeclaringType}.{propertyInfo.Name}.");
        }
    }
}
