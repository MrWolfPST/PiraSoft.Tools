namespace PiraSoft.Tools.Data.Mapping.Attributes;

/// <summary>
/// Specifies a <see cref="CompositionMapping"/> for the property.
/// </summary>
public class CompositionMappingAttribute : MappingAttribute
{
    private readonly CompositionMapping _mapping;

    /// <summary>
    /// Initializes a new instance of the <see cref="CompositionMappingAttribute"/> class.
    /// </summary>
    public CompositionMappingAttribute() : this(null)
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="CompositionMappingAttribute"/> class.
    /// </summary>
    /// <param name="prefix"></param>
    public CompositionMappingAttribute(string? prefix) : this(prefix, null)
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="CompositionMappingAttribute"/> class.
    /// </summary>
    /// <param name="prefix">Prefix to add to the column name.</param>
    /// <param name="suffix">Suffix to append to the column name.</param>
    /// <param name="columnCheckLogic">Type of column presence and valorization check.</param>
    /// <param name="columnsToCheck">List of columns names to check for presence ad not null value.</param>
    public CompositionMappingAttribute(string? prefix, string? suffix, ColumnsCheckLogic columnCheckLogic = ColumnsCheckLogic.All, params string[] columnsToCheck)
        => _mapping = new CompositionMapping(prefix, suffix, null, columnCheckLogic, columnsToCheck);

    /// <summary>
    /// Get the mapping informations.
    /// </summary>
    /// <returns>The mapping informations.</returns>
    protected internal override MappingBase GetMapping()
        => _mapping;
}
