using Microsoft.VisualStudio.TestTools.UnitTesting;
using PiraSoft.Tools.Data.Mapping;
using System;
using System.Collections.Generic;

namespace PiraSoft.Tools.UnitTest.Data.Mapping;

[TestClass]
public class TypeMappingsUnitTest
{
    [TestMethod]
    public void GetMappingPredicate()
    {
        var parent = new Parent();
        var target = TypeMappings.Generate<Parent>();

        Assert.IsNotNull(target.GetMapping(() => parent.Name));
        Assert.IsNull(target.GetMapping(() => parent.Category));
    }

    [TestMethod]
    public void GetMappingString()
    {
        var target = TypeMappings.Generate<Parent>();

        Assert.IsNotNull(target.GetMapping("Name"));
        Assert.IsNull(target.GetMapping("Category"));
    }

    [TestMethod]
    public void AddMappingPredicate()
    {
        var parent = new Parent();
        var target = new TypeMappings(typeof(Parent));

        target.AddMapping(() => parent.Category, new ColumnMapping("Category"));

        var mapping = target.GetMapping(() => parent.Category);

        Assert.IsInstanceOfType(mapping, typeof(ColumnMapping));
    }

    [TestMethod]
    public void AddMappingName()
    {
        var parent = new Parent();
        var target = new TypeMappings(typeof(Parent));

        target.AddMapping("Category", new ColumnMapping("Category"));

        var mapping = target.GetMapping(() => parent.Category);

        Assert.IsInstanceOfType(mapping, typeof(ColumnMapping));
    }

    [TestMethod]
    public void AddMappingNotFoud()
    {
        var parent = new Parent();
        var target = new TypeMappings(typeof(Parent));

        Assert.ThrowsException<InvalidOperationException>(() => target.AddMapping("Property", new ColumnMapping("Category")));
    }

    [TestMethod]
    public void AddMappingDuplicate()
    {
        var parent = new Parent();
        var target = new TypeMappings(typeof(Parent));

        target.AddMapping("Category", new ColumnMapping("Category"));

        Assert.ThrowsException<ArgumentException>(() => target.AddMapping("Category", new ColumnMapping("Category")));
    }

    [TestMethod]
    public void UpdateMappingPredicate()
    {
        var parent = new Parent();
        var target = new TypeMappings(typeof(Parent));

        target.AddMapping(() => parent.Category, new RelationMapping("Relation"));
        target.UpdateMapping(() => parent.Category, new ColumnMapping("Category"));

        var mapping = target.GetMapping(() => parent.Category);

        Assert.IsInstanceOfType(mapping, typeof(ColumnMapping));
    }

    [TestMethod]
    public void UpdateMappingName()
    {
        var parent = new Parent();
        var target = new TypeMappings(typeof(Parent));

        target.AddMapping(() => parent.Category, new RelationMapping("Relation"));
        target.UpdateMapping("Category", new ColumnMapping("Category"));

        var mapping = target.GetMapping(() => parent.Category);

        Assert.IsInstanceOfType(mapping, typeof(ColumnMapping));
    }

    [TestMethod]
    public void UpdateMappingNotFoud()
    {
        var parent = new Parent();
        var target = new TypeMappings(typeof(Parent));

        target.AddMapping(() => parent.Category, new RelationMapping("Relation"));
        Assert.ThrowsException<InvalidOperationException>(() => target.UpdateMapping("Property", new ColumnMapping("Category")));
    }

    [TestMethod]
    public void UpdateMappingInexistent()
    {
        var parent = new Parent();
        var target = new TypeMappings(typeof(Parent));

        Assert.ThrowsException<KeyNotFoundException>(() => target.UpdateMapping("Category", new ColumnMapping("Category")));
    }

    [TestMethod]
    public void AddOrUpdateMappingPredicate()
    {
        var parent = new Parent();
        var target = new TypeMappings(typeof(Parent));

        target.AddOrUpdateMapping(() => parent.Category, new RelationMapping("Relation"));

        var mapping = target.GetMapping(() => parent.Category);

        Assert.IsInstanceOfType(mapping, typeof(RelationMapping));

        target.AddOrUpdateMapping(() => parent.Category, new ColumnMapping("Category"));

        mapping = target.GetMapping(() => parent.Category);

        Assert.IsInstanceOfType(mapping, typeof(ColumnMapping));
    }

    [TestMethod]
    public void AddOrUpdateMappingName()
    {
        var parent = new Parent();
        var target = new TypeMappings(typeof(Parent));

        target.AddMapping("Category", new RelationMapping("Relation"));

        var mapping = target.GetMapping(() => parent.Category);

        Assert.IsInstanceOfType(mapping, typeof(RelationMapping));

        target.UpdateMapping("Category", new ColumnMapping("Category"));

        mapping = target.GetMapping(() => parent.Category);

        Assert.IsInstanceOfType(mapping, typeof(ColumnMapping));
    }

    [TestMethod]
    public void AddOrUpdateMappingNotFoud()
    {
        var parent = new Parent();
        var target = new TypeMappings(typeof(Parent));

        Assert.ThrowsException<InvalidOperationException>(() => target.AddOrUpdateMapping("Property", new ColumnMapping("Category")));
    }

    [TestMethod]
    public void GenerateGenerics()
    {
        var target = TypeMappings.Generate<Parent>();

        Assert.AreEqual(typeof(Parent), target.Type);
        Assert.AreEqual(2, target.Mappings.Count);
    }

    [TestMethod]
    public void GenerateType()
    {
        var target = TypeMappings.Generate(typeof(Parent));

        Assert.AreEqual(typeof(Parent), target.Type);
        Assert.AreEqual(2, target.Mappings.Count);
    }
}