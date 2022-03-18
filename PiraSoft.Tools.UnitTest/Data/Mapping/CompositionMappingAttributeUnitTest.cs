using Microsoft.VisualStudio.TestTools.UnitTesting;
using PiraSoft.Tools.Data.Mapping;
using PiraSoft.Tools.Data.Mapping.Attributes;

namespace PiraSoft.Tools.UnitTest.Data.Mapping;

[TestClass]
public class CompositionMappingAttributeUnitTest
{
    [TestMethod]
    public void ConstructorSimple()
    {
        var attribute = new CompositionMappingAttribute();
        var target = attribute.GetMapping() as CompositionMapping;

        Assert.IsNotNull(target);
        Assert.IsNull(target.Prefix, nameof(CompositionMapping.Prefix));
        Assert.IsNull(target.Suffix, nameof(CompositionMapping.Suffix));
        Assert.AreEqual(ColumnsCheckLogic.All, target.ColumnCheckLogic, nameof(CompositionMapping.ColumnCheckLogic));
        Assert.IsNull(target.ColumnsToCheck, nameof(CompositionMapping.ColumnsToCheck));
    }

    [TestMethod]
    public void ConstructorPrefix()
    {
        var attribute = new CompositionMappingAttribute("Prefix");
        var target = attribute.GetMapping() as CompositionMapping;

        Assert.IsNotNull(target);
        Assert.AreEqual("Prefix", target.Prefix, nameof(CompositionMapping.Prefix));
        Assert.IsNull(target.Suffix, nameof(CompositionMapping.Suffix));
        Assert.AreEqual(ColumnsCheckLogic.All, target.ColumnCheckLogic, nameof(CompositionMapping.ColumnCheckLogic));
        Assert.IsNull(target.ColumnsToCheck, nameof(CompositionMapping.ColumnsToCheck));
    }

    [TestMethod]
    public void ConstructorPrefixSuffix()
    {
        var attribute = new CompositionMappingAttribute("Prefix", "Suffix");
        var target = attribute.GetMapping() as CompositionMapping;

        Assert.IsNotNull(target);
        Assert.AreEqual("Prefix", target.Prefix, nameof(CompositionMapping.Prefix));
        Assert.AreEqual("Suffix", target.Suffix, nameof(CompositionMapping.Suffix));
        Assert.AreEqual(ColumnsCheckLogic.All, target.ColumnCheckLogic, nameof(CompositionMapping.ColumnCheckLogic));
        Assert.IsNull(target.ColumnsToCheck, nameof(CompositionMapping.ColumnsToCheck));
    }

    [TestMethod]
    public void ConstructorComplete()
    {
        var attribute = new CompositionMappingAttribute("Prefix", "Suffix", ColumnsCheckLogic.Any, "Column1", "Column2");
        var target = attribute.GetMapping() as CompositionMapping;

        Assert.IsNotNull(target);
        Assert.AreEqual("Prefix", target.Prefix, nameof(CompositionMapping.Prefix));
        Assert.AreEqual("Suffix", target.Suffix, nameof(CompositionMapping.Suffix));
        Assert.AreEqual(ColumnsCheckLogic.Any, target.ColumnCheckLogic, nameof(CompositionMapping.ColumnCheckLogic));
        Assert.AreEqual(2, target.ColumnsToCheck?.Count, nameof(CompositionMapping.ColumnsToCheck));
    }
}