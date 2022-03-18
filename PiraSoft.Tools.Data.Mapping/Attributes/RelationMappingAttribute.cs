namespace PiraSoft.Tools.Data.Mapping.Attributes;

public class RelationMappingAttribute : MappingAttribute
{
    private readonly RelationMapping _mapping;

    public RelationMappingAttribute(string relationName)
        => _mapping = new RelationMapping(relationName);

    protected internal override MappingBase GetMapping()
        => _mapping;
}
