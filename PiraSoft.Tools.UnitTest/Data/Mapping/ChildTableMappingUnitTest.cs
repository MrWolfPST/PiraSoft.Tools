using Microsoft.VisualStudio.TestTools.UnitTesting;
using PiraSoft.Tools.Data.Mapping;
using System;
using System.Linq;

namespace PiraSoft.Tools.UnitTest.Data.Mapping;

[TestClass]
public class ChildTableMappingUnitTest
{
    [TestMethod]
    public void Validation()
    {
        Assert.ThrowsException<ArgumentException>(() => new ChildTableMapping(null), "null");
        Assert.ThrowsException<ArgumentException>(() => new ChildTableMapping(""), "empty");
        Assert.ThrowsException<ArgumentException>(() => new ChildTableMapping(" "), "white space");

        var ds = new TestDataSet();
        var dt = ds.Parents;
        var target = new Parent();
        var mapping = new ChildTableMapping("Relation", TypeMappings.Generate<Category>());

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
#pragma warning disable CS8604 // Possible null reference argument.
        Assert.ThrowsException<ArgumentException>(() => mapping.Map(target, target.GetType().GetProperty(nameof(Parent.Children)), null), "wrong mapping type");
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
    }

    [TestMethod]
    public void MapNotListProperty()
    {
        var ds = new TestDataSet();
        var dt = ds.Parents;
        var target = new Parent();
        var mappings = new TypeMappings(target.GetType());

        mappings.AddMapping(() => target.Category, new ChildTableMapping("Children"));

        ds.Popolate();

        Assert.ThrowsException<InvalidOperationException>(() => Mapper.Map(target, dt.Rows[0], mappings));
    }

    [TestMethod]
    public void MapRelatedRecordExistsImplicit()
    {
        var ds = new TestDataSet();
        var dt = ds.Parents;
        var target = new Parent();
        var mappings = new TypeMappings(target.GetType());

        mappings.AddMapping(() => target.Children, new ChildTableMapping("Children"));

        ds.Popolate();

        Mapper.Map(target, dt.Rows[0], mappings);

        Assert.IsNotNull(target.Children);
        Assert.AreEqual(3, target.Children.Count());
        Assert.AreEqual(3, target.Children.Last().Id);
        Assert.IsNull(target.Children.First().Value);
    }

    [TestMethod]
    public void MapRelatedRecordExistsExplicit()
    {
        var ds = new TestDataSet();
        var dt = ds.Parents;
        var target = new Parent();
        var mappings = new TypeMappings(target.GetType());
        var childMapping = new TypeMappings(typeof(Child));

        mappings.AddMapping(() => target.ChildrenArray, new ChildTableMapping(1));
        childMapping.AddMapping(nameof(Child.Id), new ColumnMapping("Id"));
        childMapping.AddMapping(nameof(Child.Description), new ColumnMapping("Description"));

        ds.Popolate();

        Mapper.Map(target, dt.Rows[0], mappings);

        Assert.IsNotNull(target.ChildrenArray);
        Assert.AreEqual(3, target.ChildrenArray.Length);
        Assert.AreEqual(3, target.ChildrenArray.Last().Id);
    }

    [TestMethod]
    public void MapTableNameNotExists()
    {
        var ds = new TestDataSet();
        var dt = ds.Parents;
        var target = new Parent();
        var mappings = new TypeMappings(target.GetType());

        mappings.AddMapping(() => target.Children, new ChildTableMapping("AnotherTable"));

        ds.Popolate();

        Assert.ThrowsException<ArgumentException>(() => Mapper.Map(target, dt.Rows[1], mappings));
    }

    [TestMethod]
    public void MapTableIndexNotExists()
    {
        var ds = new TestDataSet();
        var dt = ds.Parents;
        var target = new Parent();
        var mappings = new TypeMappings(target.GetType());

        mappings.AddMapping(() => target.Children, new ChildTableMapping(10));

        ds.Popolate();

        Assert.ThrowsException<ArgumentException>(() => Mapper.Map(target, dt.Rows[1], mappings));
    }
}