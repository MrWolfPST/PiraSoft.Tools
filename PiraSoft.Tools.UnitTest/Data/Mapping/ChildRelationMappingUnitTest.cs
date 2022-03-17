using Microsoft.VisualStudio.TestTools.UnitTesting;
using PiraSoft.Tools.Data.Mapping;
using System;
using System.Linq;

namespace PiraSoft.Tools.UnitTest.Data.Mapping;

[TestClass]
public class ChildRelationMappingUnitTest
{
    [TestMethod]
    public void Validation()
    {
        Assert.ThrowsException<ArgumentException>(() => new ChildRelationMapping(null), "null");
        Assert.ThrowsException<ArgumentException>(() => new ChildRelationMapping(""), "empty");
        Assert.ThrowsException<ArgumentException>(() => new ChildRelationMapping(" "), "white space");

        var ds = new TestDataSet();
        var dt = ds.Parents;
        var target = new Parent();
        var mapping = new ChildRelationMapping("Relation", TypeMappings.Generate<Category>());

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

        mappings.AddMapping(() => target.Category, new ChildRelationMapping("ParentChildren"));

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

        mappings.AddMapping(() => target.Children, new ChildRelationMapping("ParentChildren"));

        ds.Popolate();

        Mapper.Map(target, dt.Rows[0], mappings);

        Assert.IsNotNull(target.Children);
        Assert.AreEqual(2, target.Children.Count());
        Assert.AreEqual(1, target.Children.First().Id);
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

        mappings.AddMapping(() => target.ChildrenArray, new ChildRelationMapping("ParentChildren"));
        childMapping.AddMapping(nameof(Child.Id), new ColumnMapping("Id"));
        childMapping.AddMapping(nameof(Child.Description), new ColumnMapping("Description"));

        ds.Popolate();

        Mapper.Map(target, dt.Rows[0], mappings);

        Assert.IsNotNull(target.ChildrenArray);
        Assert.AreEqual(2, target.ChildrenArray.Length);
        Assert.AreEqual(1, target.ChildrenArray.First().Id);
        Assert.IsNull(target.ChildrenArray.First().Value);
    }

    [TestMethod]
    public void MapRelatedRecordNotExists()
    {
        var ds = new TestDataSet();
        var dt = ds.Parents;
        var target = new Parent();
        var mappings = new TypeMappings(target.GetType());

        mappings.AddMapping(() => target.Children, new ChildRelationMapping("ParentChildren"));

        ds.Popolate();

        Mapper.Map(target, dt.Rows[2], mappings);

        Assert.IsNotNull(target.Children);
        Assert.IsFalse(target.Children.Any());
    }

    [TestMethod]
    public void MapRelatedRelationNotExists()
    {
        var ds = new TestDataSet();
        var dt = ds.Parents;
        var target = new Parent();
        var mappings = new TypeMappings(target.GetType());

        mappings.AddMapping(() => target.Children, new ChildRelationMapping("AnotherRelation"));

        ds.Popolate();

        Assert.ThrowsException<ArgumentException>(() => Mapper.Map(target, dt.Rows[1], mappings));
    }
}