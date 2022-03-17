using System.Data;
using System.Reflection;

namespace PiraSoft.Tools.Data.Mapping;

public class RelationMapping : MappingBase
{
    public RelationMapping(string? relationName) : this(relationName, null)
    { }

    public RelationMapping(string? relationName, TypeMappings? mappings) : this(relationName, mappings, null)
    { }

    public RelationMapping(string? relationName, TypeMappings? mappings, Func<object>? factory)
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
        if (this.Mappings != null && propertyInfo.PropertyType != this.Mappings.Type)
        {
            throw new ArgumentException($"Type of {propertyInfo.Name} is not the same of type specified in {nameof(this.Mappings)}.");
        }

        if (!row.Table.ParentRelations.Contains(this.RelationName))
        {
            throw new ArgumentException($"Unable to find the {this.RelationName} relation.");
        }

        var relatedRow = row.GetParentRow(this.RelationName);
        var factory = this.Factory
                ?? (() => Activator.CreateInstance(propertyInfo.PropertyType)
                ?? throw new InvalidOperationException($"Unable to create an instance of {propertyInfo.PropertyType} type."));

        if (relatedRow != null)
        {
            this.SetValue(target, propertyInfo, Mapper.Map(factory, relatedRow, this.Mappings));
        }
    }
}
