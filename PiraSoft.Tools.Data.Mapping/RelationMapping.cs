using System.Data;
using System.Reflection;

namespace PiraSoft.Tools.Data.Mapping;

/// <summary>
/// The class for parent mapping based on a <see cref="DataRelation"/>.
/// </summary>
public class RelationMapping : MappingBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RelationMapping"/> class.
    /// </summary>
    /// <param name="relationName">The name of relation.</param>
    /// <exception cref="ArgumentException"><paramref name="relationName"/> not contains a value.</exception>
    public RelationMapping(string? relationName) : this(relationName, null)
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="RelationMapping"/> class.
    /// </summary>
    /// <param name="relationName">The name of relation.</param>
    /// <param name="mappings">Mapping data for child objects type.</param>
    /// <exception cref="ArgumentException"><paramref name="relationName"/> not contains a value.</exception>
    public RelationMapping(string? relationName, TypeMappings? mappings) : this(relationName, mappings, null)
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="RelationMapping"/> class.
    /// </summary>
    /// <param name="relationName">The name of relation.</param>
    /// <param name="mappings">Mapping data for child objects type.</param>
    /// <param name="factory">Delegate for child objects construction.</param>
    /// <exception cref="ArgumentException"><paramref name="relationName"/> not contains a value.</exception>
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

    /// <summary>
    /// The name of relation.
    /// </summary>
    public string RelationName { get; }

    /// <summary>
    /// Mapping data for child objects type.
    /// </summary>
    public TypeMappings? Mappings { get; }

    /// <summary>
    /// Delegate for child objects construction.
    /// </summary>
    public Func<object>? Factory { get; }

    /// <summary>
    /// Map data into property of target.
    /// </summary>
    /// <param name="target">Target object to popolate.</param>
    /// <param name="propertyInfo"><see cref="PropertyInfo"/> that identity property to popolate.</param>
    /// <param name="row"><see cref="DataRow"/> that contains data.</param>
    /// <exception cref="ArgumentException">The type of target property is not the same ot type specified in <see cref="Mappings"/> or a parent relation with name specified in <see cref="RelationName"/> was not found in <paramref name="row"/>.</exception>
    /// <exception cref="InvalidOperationException">Unable to create an instance of target property type.</exception>
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
