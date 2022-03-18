namespace PiraSoft.Tools.Data.Mapping.Attributes;

public class ChildRelationMappingAttribute : MappingAttribute
{
    private readonly ChildRelationMapping _mapping;

    public ChildRelationMappingAttribute(string relationName)
        => _mapping = new ChildRelationMapping(relationName);

    protected internal override MappingBase GetMapping()
        => _mapping;
}
