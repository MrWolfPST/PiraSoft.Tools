using PiraSoft.Tools.Data.Mapping;
using PiraSoft.Tools.Data.Mapping.Attributes;
using System.Collections.Generic;

namespace PiraSoft.Tools.UnitTest.Data.Mapping;

internal class WithAttributeParent
{
    [ColumnMapping("Id")]
    public int ParentId { get; set; }

    [ColumnMapping("Name")]
    public string? ParentName { get; set; }

    [ChildRelationMapping("ParentChildren")]
    public IEnumerable<Child>? Children { get; set; }

    [ChildRelationMapping("ParentChildren")]
    public Child[]? ChildrenArray { get; set; }

    [RelationMapping("ParentCategory")]
    public Category? Category { get; set; }
}

internal class WithAttributeChild
{
    public int Id { get; set; }

    public Parent? Parent { get; set; }

    public string? Description { get; set; }

    [CompositionMapping()]
    public Values? Value { get; set; }

    [CompositionMapping("Prefix", null, default, "PrefixValue1", "PrefixValue2")]
    public Values? Prefix { get; set; }

    [CompositionMapping(null, "Suffix", ColumnsCheckLogic.Any, "Value1Suffix", "Value2Suffix")]
    public Values? Suffix { get; set; }

    [CompositionMapping("Prefix", "Suffix", ColumnsCheckLogic.Any, "PrefixValue1Suffix", "PrefixValue2Suffix")]
    public Values? PrefixSuffix { get; set; }
}