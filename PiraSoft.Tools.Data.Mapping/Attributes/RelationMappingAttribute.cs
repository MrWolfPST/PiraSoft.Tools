namespace PiraSoft.Tools.Data.Mapping.Attributes;

/// <summary>
/// Specifies a <see cref="RelationMapping"/> for the property.
/// </summary>
public class RelationMappingAttribute : MappingAttribute
{
    private readonly RelationMapping _mapping;

    /// <summary>
    /// Initializes a new instance of the <see cref="RelationMappingAttribute"/> class.
    /// </summary>
    /// <param name="relationName">The name of relation.</param>
    public RelationMappingAttribute(string relationName)
        => _mapping = new RelationMapping(relationName);

    /// <summary>
    /// Get the mapping informations.
    /// </summary>
    /// <returns>The mapping informations.</returns>
    protected internal override MappingBase GetMapping()
        => _mapping;
}
