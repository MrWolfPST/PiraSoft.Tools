using System.Data;

namespace PiraSoft.Tools.Data.Mapping;

public class ChildRelationMapping : ChildMappingBase
{
    public ChildRelationMapping(string? relationName) : this(relationName, null)
    { }

    public ChildRelationMapping(string? relationName, TypeMappings? mappings) : this(relationName, mappings, null)
    { }

    public ChildRelationMapping(string? relationName, TypeMappings? mappings, Func<object>? factory)
        : base(mappings, factory)
    {
        if (string.IsNullOrWhiteSpace(relationName))
        {
            throw new ArgumentException($"Parameter {nameof(relationName)} must contains a value.", nameof(relationName));
        }

        this.RelationName = relationName;
    }

    public string RelationName { get; }

    protected override IEnumerable<DataRow> GetChildRows(DataRow row)
    {
        if (!row.Table.ChildRelations.Contains(this.RelationName))
        {
            throw new ArgumentException($"Unable to find the {this.RelationName} relation.");
        }

        return row.GetChildRows(this.RelationName);
    }
}
