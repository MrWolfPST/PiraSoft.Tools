using Microsoft.VisualStudio.TestTools.UnitTesting;
using PiraSoft.Tools.Data.Mapping;
using System;

namespace PiraSoft.Tools.UnitTest.Data.Mapping;

[TestClass]
public class MapperUnitTest
{
    [TestMethod]
    public void MapValidation()
    {
        var ds = new TestDataSet();
        var dt = ds.Parents;
        var target = new Parent();

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        Assert.ThrowsException<ArgumentNullException>(() => Mapper.Map((object)null, null, null), "target");
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
        Assert.ThrowsException<ArgumentNullException>(() => Mapper.Map(new object(), null, null), "row");
        Assert.ThrowsException<ArgumentNullException>(() => Mapper.Map(null, null), "factory");
#pragma warning disable CS8603 // Possible null reference return.
        Assert.ThrowsException<InvalidOperationException>(() => Mapper.Map(() => null, dt.NewRow()), "factory return");
        Assert.IsNull(Mapper.Map(() => new Parent(), null), "null row with factory");
#pragma warning restore CS8603 // Possible null reference return.
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

        Assert.ThrowsException<ArgumentException>(() => Mapper.Map(target, dt.NewRow(), TypeMappings.Generate<Category>()), "wrong mapping type");
    }

    [TestMethod]
    public void MapExplicit()
    {
        var dt = new TestDataTable();
        var target = new AnotherModel();
        var mappings = new TypeMappings(target.GetType());

        mappings.AddMapping(() => target.AnotherStringProperty, new ColumnMapping("AnotherStringColumn"));

        Mapper.Map(target, dt.Rows[0], mappings);

        Assert.AreEqual("AnotherStringValue", target.AnotherStringProperty);
    }

    [TestMethod]
    public void MapFactory()
    {
        var dt = new TestDataTable();

        var target = Mapper.Map(() => new SimpleModel(), dt.Rows[0]);

        Assert.IsNotNull(target);
        Assert.IsInstanceOfType(target, typeof(SimpleModel));
        Assert.AreEqual("StringValue", target.StringProperty);
        Assert.IsNull(target.ReadOnlyProperty);
    }

    [TestMethod]
    public void MapGeneric()
    {
        var dt = new TestDataTable();
        var target = Mapper.Map<SimpleModel>(dt.Rows[0]);

        Assert.IsNotNull(target);
        Assert.AreEqual("StringValue", target.StringProperty);
    }

    [TestMethod]
    public void MapGenericExplicit()
    {
        var dt = new TestDataTable();
        var mappings = new TypeMappings(typeof(AnotherModel));

        mappings.AddMapping("AnotherStringProperty", new ColumnMapping("AnotherStringColumn"));

        var target = Mapper.Map<AnotherModel>(dt.Rows[0], mappings);

        Assert.IsNotNull(target);
        Assert.AreEqual("AnotherStringValue", target.AnotherStringProperty);
    }

    [TestMethod]
    public void AddTypeMappingsGeneric()
    {
        Mapper.AddTypeMappings<Parent>();

        Assert.IsTrue(Mapper.TypeMappingsCatalog.ContainsKey(typeof(Parent)));
    }

    [TestMethod]
    public void AddTypeMappingsType()
    {
        Mapper.AddTypeMappings(typeof(Parent));

        Assert.IsTrue(Mapper.TypeMappingsCatalog.ContainsKey(typeof(Parent)));
    }

    [TestMethod]
    public void AddTypeMappings()
    {
        Mapper.AddTypeMappings(new TypeMappings(typeof(Parent)));

        Assert.IsTrue(Mapper.TypeMappingsCatalog.ContainsKey(typeof(Parent)));
    }
}