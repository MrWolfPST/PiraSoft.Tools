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
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        Assert.ThrowsException<ArgumentNullException>(() => Mapper.Map((object)null, null, null), "target");
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
        Assert.ThrowsException<ArgumentNullException>(() => Mapper.Map(new object(), null, null), "row");
        Assert.ThrowsException<ArgumentNullException>(() => Mapper.Map(null, null), "factory");
#pragma warning disable CS8603 // Possible null reference return.
        Assert.ThrowsException<InvalidOperationException>(() => Mapper.Map(() => null, null), "factory return");
#pragma warning restore CS8603 // Possible null reference return.
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

        var ds = new TestDataSet();
        var dt = ds.Parents;
        var target = new Parent();

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
#pragma warning disable CS8604 // Possible null reference argument.
        Assert.ThrowsException<ArgumentException>(() => Mapper.Map(target, null, TypeMappings.Generate<Category>()), "wrong mapping type");
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
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

        Assert.IsInstanceOfType(target, typeof(SimpleModel));
        Assert.AreEqual("StringValue", ((SimpleModel)target).StringProperty);
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