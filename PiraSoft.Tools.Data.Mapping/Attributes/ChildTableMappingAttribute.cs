namespace PiraSoft.Tools.Data.Mapping.Attributes;

public class ChildTableMappingAttribute : MappingAttribute
{
    private readonly ChildTableMapping _mapping;

    public ChildTableMappingAttribute(string tableName)
        => _mapping = new ChildTableMapping(tableName);

    public ChildTableMappingAttribute(int tableIndex)
        => _mapping = new ChildTableMapping(tableIndex);

    protected internal override MappingBase GetMapping()
        => _mapping;
}
