using Microsoft.VisualStudio.TestTools.UnitTesting;
using PiraSoft.Tools.Data.Mapping;

namespace PiraSoft.Tools.UnitTest.Data.Mapping;

[TestClass]
public class CompositionMappingUnitTest
{
    [TestMethod]
    public void MapImplicit()
    {
        var ds = new TestDataSet();
        var target = new Child();
        var mappings = new TypeMappings(target.GetType());

        mappings.AddMapping(() => target.Value, new CompositionMapping());
        mappings.AddMapping(() => target.Prefix, new CompositionMapping("Prefix"));
        mappings.AddMapping(() => target.Suffix, new CompositionMapping(null, "Suffix"));
        mappings.AddMapping(() => target.PrefixSuffix, new CompositionMapping("Prefix", "Suffix"));

        ds.Popolate();

        Mapper.Map(target, ds.Children.Rows[0], mappings);

        Assert.IsNotNull(target.Value);
        Assert.AreEqual("Value1.1.1", target.Value?.Value1, "Value.Value1");
        Assert.AreEqual("Value2.1.1", target.Value?.Value2, "Value.Value2");
        Assert.AreEqual("Value3.1.1", target.Value?.Value3, "Value.Value3");
        Assert.AreEqual("PrefixValue1.1.1", target.Prefix?.Value1, "Prefix.Value1");
        Assert.AreEqual("PrefixValue2.1.1", target.Prefix?.Value2, "Prefix.Value2");
        Assert.AreEqual("PrefixValue3.1.1", target.Prefix?.Value3, "Prefix.Value3");
        Assert.AreEqual("Value1Suffix.1.1", target.Suffix?.Value1, "Suffix.Value1");
        Assert.AreEqual("Value2Suffix.1.1", target.Suffix?.Value2, "Suffix.Value2");
        Assert.AreEqual("Value3Suffix.1.1", target.Suffix?.Value3, "Suffix.Value3");
        Assert.AreEqual("PrefixValue1Suffix.1.1", target.PrefixSuffix?.Value1, "PrefixSuffix.Value1");
        Assert.AreEqual("PrefixValue2Suffix.1.1", target.PrefixSuffix?.Value2, "PrefixSuffix.Value2");
        Assert.AreEqual("PrefixValue3Suffix.1.1", target.PrefixSuffix?.Value3, "PrefixSuffix.Value3");
    }

    [TestMethod]
    public void MapFactory()
    {
        var ds = new TestDataSet();
        var target = new Child();
        var mappings = new TypeMappings(target.GetType());

        mappings.AddMapping(() => target.Value, new CompositionMapping(() => new Values()));
        mappings.AddMapping(() => target.Prefix, new CompositionMapping("Prefix", () => new Values()));

        ds.Popolate();

        Mapper.Map(target, ds.Children.Rows[0], mappings);

        Assert.IsNotNull(target.Value);
        Assert.AreEqual("Value1.1.1", target.Value?.Value1, "Value.Value1");
        Assert.AreEqual("Value2.1.1", target.Value?.Value2, "Value.Value2");
        Assert.AreEqual("Value3.1.1", target.Value?.Value3, "Value.Value3");
        Assert.AreEqual("PrefixValue1.1.1", target.Prefix?.Value1, "Prefix.Value1");
        Assert.AreEqual("PrefixValue2.1.1", target.Prefix?.Value2, "Prefix.Value2");
        Assert.AreEqual("PrefixValue3.1.1", target.Prefix?.Value3, "Prefix.Value3");
        Assert.IsNull(target.Suffix);
        Assert.IsNull(target.PrefixSuffix);
    }

    [TestMethod]
    public void MapCheckColums()
    {
        var ds = new TestDataSet();
        var target = new Child();
        var mappings = new TypeMappings(target.GetType());

        mappings.AddMapping(() => target.Value, new CompositionMapping());
        mappings.AddMapping(() => target.Prefix, new CompositionMapping("Prefix", null, null, default, "PrefixValue1", "PrefixValue2"));
        mappings.AddMapping(() => target.Suffix, new CompositionMapping(null, "Suffix", null, ColumnsCheckLogic.Any, "Value1Suffix", "Value2Suffix"));
        mappings.AddMapping(() => target.PrefixSuffix, new CompositionMapping("Prefix", "Suffix", null, ColumnsCheckLogic.Any, "PrefixValue1Suffix", "PrefixValue2Suffix"));

        ds.Popolate();

        Mapper.Map(target, ds.Children.Rows[1], mappings);

        Assert.IsNull(target.Value, "Value");
        Assert.IsNull(target.Prefix, "Prefix");
        Assert.IsNull(target.Suffix, "Suffix");
        Assert.IsNotNull(target.PrefixSuffix, "PrefixSuffix");
    }

}