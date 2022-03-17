using System.Collections;
using System.Data;
using System.Reflection;

namespace PiraSoft.Tools.Data.Mapping;

public class ChildRelationMapping : MappingBase
{
    public ChildRelationMapping(string? relationName) : this(relationName, null)
    { }

    public ChildRelationMapping(string? relationName, TypeMappings? mappings) : this(relationName, mappings, null)
    { }

    public ChildRelationMapping(string? relationName, TypeMappings? mappings, Func<object>? factory)
    {
        if (string.IsNullOrWhiteSpace(relationName))
        {
            throw new ArgumentException($"Parameter {nameof(relationName)} must contains a value.", nameof(relationName));
        }

        this.RelationName = relationName;
        this.Mappings = mappings;
        this.Factory = factory;
    }

    public string RelationName { get; }

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

        if (!row.Table.ChildRelations.Contains(this.RelationName))
        {
            throw new ArgumentException($"Unable to find the {this.RelationName} relation.");
        }

        var childRows = row.GetChildRows(this.RelationName);
        var factory = this.Factory
                ?? (() => Activator.CreateInstance(itemsType)
                ?? throw new InvalidOperationException($"Unable to create an instance of {itemsType} type."));
        var mapping = Mapper.EnsureTypeMappings(itemsType);
        var items = (IList?)Activator.CreateInstance(typeof(List<>).MakeGenericType(itemsType))
            ?? throw new InvalidOperationException($"Unable to create an instance of List<{itemsType}> type.");

        childRows.ForEach(i => items.Add(Mapper.Map(factory, i, mapping)));

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
}
