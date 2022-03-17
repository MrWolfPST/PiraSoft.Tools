namespace PiraSoft.Tools.Data.Mapping.Attributes;


public class CompositionMappingAttribute : MappingAttribute
{
    private readonly CompositionMapping _mapping;

    public CompositionMappingAttribute() : this(null)
    { }

    public CompositionMappingAttribute(string? prefix) : this(prefix, null)
    { }

    public CompositionMappingAttribute(string? prefix, string? suffix)
        => _mapping = new CompositionMapping(prefix, suffix);

    protected internal override MappingBase GetMapping()
        => _mapping;
}
