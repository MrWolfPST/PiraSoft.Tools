using System.Collections.Generic;

namespace PiraSoft.Tools.UnitTest.Data.Mapping;

internal class SimpleModel
{
    public string? StringProperty { get; set; }

    public string? ReadOnlyProperty { get; }
}

internal class AnotherModel : SimpleModel
{
    public string? AnotherStringProperty { get; set; }
}

internal class Parent
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public IEnumerable<Child>? Children { get; set; }

    public Category? Category { get; set; }

    public Category? SecondCategory { get; set; }
}

internal class Child
{
    public int Id { get; set; }

    public Parent? Parent { get; set; }

    public Values? First { get; set; }

    public Values? Second { get; set; }

    public Values? Last { get; set; }
}

internal class Values
{
    public string? Value1 { get; set; }

    public string? Value2 { get; set; }
}

internal class Category
{
    public CategoryValue? Code { get; set; }

    public CategoryValue? Value { get; set; }
}

internal enum CategoryValue
{
    Code0 = 0,
    Code1 = 1,
    Code2 = 2
}