namespace PiraSoft.Tools.Data.Mapping.Attributes;

/// <summary>
/// Specifies a <see cref="ChildTableMapping"/> for the property.
/// </summary>
public class ChildTableMappingAttribute : MappingAttribute
{
    private readonly ChildTableMapping _mapping;

    /// <summary>
    /// Initializes a new instance of the <see cref="ChildTableMappingAttribute"/> class.
    /// </summary>
    /// <param name="tableName">The name of the table.</param>
    public ChildTableMappingAttribute(string tableName)
        => _mapping = new ChildTableMapping(tableName);

    /// <summary>
    /// Initializes a new instance of the <see cref="ChildTableMappingAttribute"/> class.
    /// </summary>
    /// <param name="tableIndex">The index of the table.</param>
    public ChildTableMappingAttribute(int tableIndex)
        => _mapping = new ChildTableMapping(tableIndex);

    /// <summary>
    /// Get the mapping informations.
    /// </summary>
    /// <returns>The mapping informations.</returns>
    protected internal override MappingBase GetMapping()
        => _mapping;
}
