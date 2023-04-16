using Microsoft.VisualStudio.TestTools.UnitTesting;
using PiraSoft.Tools.Data.Mapping;
using System;

namespace PiraSoft.Tools.UnitTest.Data.Mapping;

[TestClass]
public class RelationMappingUnitTest
{
    [TestMethod]
    public void Validation()
    {
        Assert.ThrowsException<ArgumentException>(() => new RelationMapping(null), "null");
        Assert.ThrowsException<ArgumentException>(() => new RelationMapping(""), "empty");
        Assert.ThrowsException<ArgumentException>(() => new RelationMapping(" "), "white space");

        var target = new Parent();
        var mapping = new RelationMapping("Relation", TypeMappings.Generate<Category>());

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
#pragma warning disable CS8604 // Possible null reference argument.
        Assert.ThrowsException<ArgumentException>(() => mapping.Map(target, target.GetType().GetProperty(nameof(Parent.Name)), null), "wrong mapping type");
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
    }

    [TestMethod]
    public void MapRelatedRecordExistsImplicit()
    {
        var ds = new TestDataSet();
        var dt = ds.Parents;
        var target = new Parent();
        var mappings = new TypeMappings(target.GetType());

        mappings.AddMapping(() => target.Category, new RelationMapping("ParentCategory"));

        ds.Popolate();

        Mapper.Map(target, dt.Rows[0], mappings);

        Assert.IsNotNull(target.Category);
        Assert.AreEqual(CategoryValue.Code1, target.Category.Code);
        Assert.AreEqual(CategoryValue.Code2, target.Category.Value);
    }

    [TestMethod]
    public void MapRelatedRecordExistsExplicit()
    {
        var ds = new TestDataSet();
        var dt = ds.Parents;
        var target = new Parent();
        var mappings = new TypeMappings(target.GetType());
        var categoryMappings = new TypeMappings(typeof(Category));

        categoryMappings.AddMapping(nameof(Category.Code), new ColumnMapping("Value"));
        categoryMappings.AddMapping(nameof(Category.Value), new ColumnMapping("Code"));
        mappings.AddMapping(() => target.Category, new RelationMapping("ParentCategory", categoryMappings));

        ds.Popolate();

        Mapper.Map(target, dt.Rows[0], mappings);

        Assert.IsNotNull(target.Category);
        Assert.AreEqual(CategoryValue.Code2, target.Category.Code);
        Assert.AreEqual(CategoryValue.Code1, target.Category.Value);
    }

    [TestMethod]
    public void MapRelatedRecordNotExists()
    {
        var ds = new TestDataSet();
        var dt = ds.Parents;
        var target = new Parent();
        var mappings = new TypeMappings(target.GetType());

        mappings.AddMapping(() => target.Category, new RelationMapping("ParentCategory"));

        ds.Popolate();

        Mapper.Map(target, dt.Rows[1], mappings);

        Assert.IsNull(target.Category);
    }

    [TestMethod]
    public void MapRelatedRelationNotExists()
    {
        var ds = new TestDataSet();
        var dt = ds.Parents;
        var target = new Parent();
        var mappings = new TypeMappings(target.GetType());

        mappings.AddMapping(() => target.Category, new RelationMapping("AnotherRelation"));

        ds.Popolate();

        Assert.ThrowsException<ArgumentException>(() => Mapper.Map(target, dt.Rows[1], mappings));
    }
}