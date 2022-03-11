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