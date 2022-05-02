using System.Collections;
using System.Data;
using System.Reflection;

namespace PiraSoft.Tools.Data.Mapping;

public class ChildTableMapping : ChildMappingBase
{
    public ChildTableMapping(string? tableName) : this(tableName, null)
    { }

    public ChildTableMapping(string? tableName, TypeMappings? mappings) : this(tableName, mappings, null)
    { }

    public ChildTableMapping(string? TableName, TypeMappings? mappings, Func<object>? factory)
        :base(mappings, factory)
    {
        if (string.IsNullOrWhiteSpace(TableName))
        {
            throw new ArgumentException($"Parameter {nameof(TableName)} must contains a value.", nameof(TableName));
        }

        this.TableName = TableName;
    }

    public ChildTableMapping(int tableIndex) : this(tableIndex, null)
    { }

    public ChildTableMapping(int tableIndex, TypeMappings? mappings) : this(tableIndex, mappings, null)
    { }

    public ChildTableMapping(int tableIndex, TypeMappings? mappings, Func<object>? factory)
        : base(mappings, factory)
    {
        this.TableIndex = tableIndex;
    }

    public string? TableName { get; }

    public int? TableIndex { get; }

    protected override IEnumerable<DataRow> GetChildRows(DataRow row)
    {
        if (this.TableIndex.HasValue)
        {
            try
            {
                return row.Table?.DataSet?.Tables[this.TableIndex.Value]?.Rows?.AsEnumerable()
                    ?? throw new ArgumentException($"Cannot find the table at index {this.TableIndex}.");
            }
            catch (IndexOutOfRangeException)
            {
                throw new ArgumentException($"Cannot find the table at index {this.TableIndex}.");
            }
        }
        else
        {
            return row.Table?.DataSet?.Tables[this.TableName]?.Rows?.AsEnumerable()
                ?? throw new ArgumentException($"Cannot find a table named {this.TableName}.");
        }
    }
}
