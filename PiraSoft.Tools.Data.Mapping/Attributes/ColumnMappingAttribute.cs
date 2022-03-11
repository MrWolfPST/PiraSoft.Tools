namespace PiraSoft.Tools.Data.Mapping.Attributes;


public class ColumnMappingAttribute : MappingAttribute
{
    private readonly ColumnMapping _mapping;

    public ColumnMappingAttribute(string columnName)
        => _mapping = new ColumnMapping(columnName);

    protected internal override MappingBase GetMapping()
        => _mapping;
}
