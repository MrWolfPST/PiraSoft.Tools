namespace PiraSoft.Tools.Data.Mapping.Attributes;

/// <summary>
/// Specifies a <see cref="ChildRelationMapping"/> for the property.
/// </summary>
public class ChildRelationMappingAttribute : MappingAttribute
{
    private readonly ChildRelationMapping _mapping;

    /// <summary>
    /// Initializes a new instance of the <see cref="ChildRelationMappingAttribute"/> class.
    /// </summary>
    /// <param name="relationName">The name of relation.</param>
    public ChildRelationMappingAttribute(string relationName)
        => _mapping = new ChildRelationMapping(relationName);

    /// <summary>
    /// Get the mapping informations.
    /// </summary>
    /// <returns>The mapping informations.</returns>
    protected internal override MappingBase GetMapping()
        => _mapping;
}
