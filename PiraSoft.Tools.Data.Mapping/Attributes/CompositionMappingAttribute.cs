namespace PiraSoft.Tools.Data.Mapping.Attributes;

public class CompositionMappingAttribute : MappingAttribute
{
    private readonly CompositionMapping _mapping;

    public CompositionMappingAttribute() : this(null)
    { }

    public CompositionMappingAttribute(string? prefix) : this(prefix, null)
    { }

    public CompositionMappingAttribute(string? prefix, string? suffix, ColumnsCheckLogic columnCheckMode = ColumnsCheckLogic.All, params string[] checkColumns)
        => _mapping = new CompositionMapping(prefix, suffix, null, columnCheckMode, checkColumns);

    protected internal override MappingBase GetMapping()
        => _mapping;
}
