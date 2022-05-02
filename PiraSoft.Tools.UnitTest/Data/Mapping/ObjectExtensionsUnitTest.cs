using Microsoft.VisualStudio.TestTools.UnitTesting;
using PiraSoft.Tools.Data.Mapping;
using System;

namespace PiraSoft.Tools.UnitTest.Data.Mapping;

[TestClass]
public class ObjectExtensionsUnitTest
{
    [TestMethod]
    public void Map()
    {
        var dt = new TestDataTable();
        var target = new SimpleModel();

        target.Map(dt.Rows[0]);

        Assert.AreEqual("StringValue", target.StringProperty);
    }

    [TestMethod]
    public void MapExplicit()
    {
        var dt = new TestDataTable();
        var mappings = new TypeMappings(typeof(AnotherModel));
        var target = new AnotherModel();

        mappings.AddMapping("AnotherStringProperty", new ColumnMapping("AnotherStringColumn"));

        target.Map(dt.Rows[0], mappings);

        Assert.AreEqual("AnotherStringValue", target.AnotherStringProperty);
    }
}
