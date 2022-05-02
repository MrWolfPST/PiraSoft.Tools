using Microsoft.VisualStudio.TestTools.UnitTesting;
using PiraSoft.Tools.Data.Mapping;
using System;
using System.Data;

namespace PiraSoft.Tools.UnitTest.Data.Mapping;

[TestClass]
public class RowExtensionsUnitTest
{
    [TestMethod]
    public void MapFactory()
    {
        var dt = new TestDataTable();

        var target = (SimpleModel?)dt.Rows[0].Map(() => (object)new SimpleModel());

        Assert.IsNotNull(target);
        Assert.AreEqual("StringValue", target.StringProperty);
    }

    [TestMethod]
    public void MapFactoryExplicit()
    {
        var dt = new TestDataTable();
        var mappings = new TypeMappings(typeof(AnotherModel));

        mappings.AddMapping("AnotherStringProperty", new ColumnMapping("AnotherStringColumn"));

        var target = (AnotherModel?)dt.Rows[0].Map(()=> (object)new AnotherModel(), mappings);

        Assert.IsNotNull(target);
        Assert.AreEqual("AnotherStringValue", target.AnotherStringProperty);
    }

    [TestMethod]
    public void MapTarget()
    {
        var dt = new TestDataTable();

        var target = new SimpleModel();

        dt.Rows[0].Map(target);

        Assert.AreEqual("StringValue", target.StringProperty);
    }

    [TestMethod]
    public void MapTargetExplicit()
    {
        var dt = new TestDataTable();
        var target = new AnotherModel();
        var mappings = new TypeMappings(target.GetType());

        mappings.AddMapping(() => target.AnotherStringProperty, new ColumnMapping("AnotherStringColumn"));

        dt.Rows[0].Map(target, mappings);

        Assert.AreEqual("AnotherStringValue", target.AnotherStringProperty);
    }

    [TestMethod]
    public void MapGeneric()
    {
        var dt = new TestDataTable();

        var target = dt.Rows[0].Map<SimpleModel>();

        Assert.IsNotNull(target);
        Assert.AreEqual("StringValue", target.StringProperty);
    }

    [TestMethod]
    public void MapGenericExplicit()
    {
        var dt = new TestDataTable();
        var mappings = new TypeMappings(typeof(AnotherModel));

        mappings.AddMapping("AnotherStringProperty", new ColumnMapping("AnotherStringColumn"));

        var target = dt.Rows[0].Map<AnotherModel>(mappings);

        Assert.IsNotNull(target);
        Assert.AreEqual("AnotherStringValue", target.AnotherStringProperty);
    }

    [TestMethod]
    public void MapGenericFactory()
    {
        var dt = new TestDataTable();

        var target = dt.Rows[0].Map(() => new SimpleModel());

        Assert.IsNotNull(target);
        Assert.AreEqual("StringValue", target.StringProperty);
    }

    [TestMethod]
    public void MapGenericFactoryExplicit()
    {
        var dt = new TestDataTable();
        var mappings = new TypeMappings(typeof(AnotherModel));

        mappings.AddMapping("AnotherStringProperty", new ColumnMapping("AnotherStringColumn"));

        var target = dt.Rows[0].Map(() => new AnotherModel(), mappings);

        Assert.IsNotNull(target);
        Assert.AreEqual("AnotherStringValue", target.AnotherStringProperty);
    }
}
