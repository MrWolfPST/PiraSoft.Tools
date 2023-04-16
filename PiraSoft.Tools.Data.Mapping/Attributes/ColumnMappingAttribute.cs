namespace PiraSoft.Tools.Data.Mapping.Attributes;

/// <summary>
/// Specifies a <see cref="ColumnMapping"/> for the property.
/// </summary>
public class ColumnMappingAttribute : MappingAttribute
{
    private readonly ColumnMapping _mapping;

    /// <summary>
    /// Initializes a new instance of the <see cref="ColumnMappingAttribute"/> class.
    /// </summary>
    /// <param name="columnName">The name of the column.</param>
    public ColumnMappingAttribute(string columnName)
        => _mapping = new ColumnMapping(columnName);

    /// <summary>
    /// Get the mapping informations.
    /// </summary>
    /// <returns>The mapping informations.</returns>
    protected internal override MappingBase GetMapping()
        => _mapping;
}
