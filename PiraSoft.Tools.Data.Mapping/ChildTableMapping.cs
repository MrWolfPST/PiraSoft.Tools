using System.Collections;
using System.Data;
using System.Reflection;

namespace PiraSoft.Tools.Data.Mapping;

public class ChildTableMapping : MappingBase
{
    public ChildTableMapping(string? tableName) : this(tableName, null)
    { }

    public ChildTableMapping(string? tableName, TypeMappings? mappings) : this(tableName, mappings, null)
    { }

    public ChildTableMapping(string? TableName, TypeMappings? mappings, Func<object>? factory)
    {
        if (string.IsNullOrWhiteSpace(TableName))
        {
            throw new ArgumentException($"Parameter {nameof(TableName)} must contains a value.", nameof(TableName));
        }

        this.TableName = TableName;
        this.Mappings = mappings;
        this.Factory = factory;
    }

    public ChildTableMapping(int tableIndex) : this(tableIndex, null)
    { }

    public ChildTableMapping(int tableIndex, TypeMappings? mappings) : this(tableIndex, mappings, null)
    { }

    public ChildTableMapping(int tableIndex, TypeMappings? mappings, Func<object>? factory)
    {
        this.TableIndex = tableIndex;
        this.Mappings = mappings;
        this.Factory = factory;
    }

    public string? TableName { get; }

    public int? TableIndex { get; }

    public TypeMappings? Mappings { get; }

    public Func<object>? Factory { get; }

    protected internal override void Map(object target, PropertyInfo propertyInfo, DataRow row)
    {
        Type? itemsType = null;

        if (propertyInfo.PropertyType.IsArray)
        {
            itemsType = propertyInfo.PropertyType.GetElementType();
        }
        else if (propertyInfo.PropertyType.IsEnumerable())
        {
            itemsType = propertyInfo.PropertyType.GetGenericArguments().FirstOrDefault();
        }

        if (itemsType == null)
        {
            throw new InvalidOperationException($"Unable to detect elements type of {propertyInfo.PropertyType}.");
        }

        if (this.Mappings != null && itemsType != this.Mappings.Type)
        {
            throw new ArgumentException($"Type of {nameof(target)} is not the same of type specified in {nameof(this.Mappings)}.");
        }

        var childRows = this.GetRows(row);
        var factory = this.Factory
                ?? (() => Activator.CreateInstance(itemsType)
                ?? throw new InvalidOperationException($"Unable to create an instance of {itemsType} type."));
        var mapping = Mapper.EnsureTypeMappings(itemsType);
        var items = (IList?)Activator.CreateInstance(typeof(List<>).MakeGenericType(itemsType))
            ?? throw new InvalidOperationException($"Unable to create an instance of List<{itemsType}> type.");

        foreach (DataRow r in childRows)
        {
            items.Add(Mapper.Map(factory, r, mapping));
        }

        this.SetValue(target, propertyInfo, items);
    }

    protected override object? Convert(object? value, Type targetType)
    {
        if (targetType.IsArray)
        {
            return ((IList?)value)?.ToArray(targetType.GetElementType() ?? throw new InvalidOperationException($"Unable to detect elements type of {targetType}."));
        }
        else
        {
            return value;
        }
    }

    private DataRowCollection GetRows(DataRow row)
    {
        if (this.TableIndex.HasValue)
        {
            try
            {
                return row.Table?.DataSet?.Tables[this.TableIndex.Value]?.Rows ?? throw new ArgumentException($"Cannot find the table at index {this.TableIndex}.");
            }
            catch (IndexOutOfRangeException)
            {
                throw new ArgumentException($"Cannot find the table at index {this.TableIndex}.");
            }
        }
        else
        {
            return row.Table?.DataSet?.Tables[this.TableName]?.Rows ?? throw new ArgumentException($"Cannot find a table named {this.TableName}.");
        }
    }
}
