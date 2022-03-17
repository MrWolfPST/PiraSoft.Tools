using System.Data;
using System.Diagnostics;
using System.Reflection;

namespace PiraSoft.Tools.Data.Mapping;

public class ColumnMapping : MappingBase
{
    public ColumnMapping(string? columnName)
    {
        if (string.IsNullOrWhiteSpace(columnName))
        {
            throw new ArgumentException($"Parameter {nameof(columnName)} must contains a value.", nameof(columnName));
        }

        this.ColumnName = columnName;
    }

    public string ColumnName { get; internal set; }

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
