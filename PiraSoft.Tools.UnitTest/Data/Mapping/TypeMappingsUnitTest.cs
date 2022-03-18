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

        Assert.AreEqual(typeof(Parent), target.Type, "Target type");
        Assert.AreEqual(2, target.Mappings.Count, "Mapping count");
    }

    [TestMethod]
    public void GenerateType()
    {
        var target = TypeMappings.Generate(typeof(WithAttributeParent));

        Assert.AreEqual(typeof(WithAttributeParent), target.Type, "Parent mapping type");
        Assert.AreEqual(5, target.Mappings.Count, "Parent mapping count");
        Assert.IsNotNull(target.GetMapping(nameof(WithAttributeParent.ParentId)) as ColumnMapping, "Parent mapping ParetId");
        Assert.IsNotNull(target.GetMapping(nameof(WithAttributeParent.ParentName)) as ColumnMapping, "Parent mapping ParentName");
        Assert.IsNotNull(target.GetMapping(nameof(WithAttributeParent.Children)) as ChildRelationMapping, "Parent mapping Children");
        Assert.IsNotNull(target.GetMapping(nameof(WithAttributeParent.ChildrenArray)) as ChildRelationMapping, "Parent mapping ChildrenArray");
        Assert.IsNotNull(target.GetMapping(nameof(WithAttributeParent.Category)) as RelationMapping, "Parent mapping Children");

        target = TypeMappings.Generate(typeof(WithAttributeChild));

        Assert.AreEqual(typeof(WithAttributeChild), target.Type, "Child target type");
        Assert.AreEqual(6, target.Mappings.Count, "Child mapping count");
        Assert.IsNotNull(target.GetMapping(nameof(WithAttributeChild.Value)) as CompositionMapping, "Child mapping Value");
        Assert.IsNotNull(target.GetMapping(nameof(WithAttributeChild.Prefix)) as CompositionMapping, "Child mapping Prefix");
        Assert.IsNotNull(target.GetMapping(nameof(WithAttributeChild.Suffix)) as CompositionMapping, "Child mapping Suffix");
        Assert.IsNotNull(target.GetMapping(nameof(WithAttributeChild.PrefixSuffix)) as CompositionMapping, "Child mapping PrefixSuffix");
    }
}