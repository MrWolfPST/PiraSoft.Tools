using Microsoft.VisualStudio.TestTools.UnitTesting;
using PiraSoft.Tools.Data.Mapping;

namespace PiraSoft.Tools.UnitTest.Data.Mapping;

[TestClass]
public class CompositionUnitTest
{
    [TestMethod]
    public void MapImplicit()
    {
        var ds = new TestDataSet();
        var target = new Child();
        var mappings = new TypeMappings(target.GetType());

        mappings.AddMapping(() => target.Value, new CompositionMapping());
        mappings.AddMapping(() => target.Prefix, new CompositionMapping("Prefix"));
        mappings.AddMapping(() => target.Suffix, new CompositionMapping("Suffix"));
        mappings.AddMapping(() => target.PrefixSuffix, new CompositionMapping("Prefix", "Suffix"));

        ds.Popolate();

        Mapper.Map(target, ds.Children.Rows[0], mappings);

        Assert.IsNotNull(target.Value);
        Assert.AreEqual("Value1.1.1", target.Value?.Value1);
        Assert.AreEqual("Value2.1.1", target.Value?.Value2);
        Assert.AreEqual("Value1.1.1", target.Prefix?.Value1);
        Assert.AreEqual("Value2.1.1", target.Prefix?.Value2);
        Assert.AreEqual("Value1.1.1", target.Suffix?.Value1);
        Assert.AreEqual("Value2.1.1", target.Suffix?.Value2);
        Assert.AreEqual("Value1.1.1", target.PrefixSuffix?.Value1);
        Assert.AreEqual("Value2.1.1", target.PrefixSuffix?.Value2);
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
        Assert.AreEqual("Value1.1.1", target.Value.Value1);
        Assert.AreEqual("Value2.1.1", target.Value.Value2);
        Assert.AreEqual("Value1.1.1", target.Prefix?.Value1);
        Assert.AreEqual("Value2.1.1", target.Prefix?.Value2);
    }
}