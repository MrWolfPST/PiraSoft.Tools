using Microsoft.VisualStudio.TestTools.UnitTesting;
using PiraSoft.Tools.Data.Mapping;
using System;
using System.Collections.Generic;

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
    }

    [TestMethod]
    public void MapRelatedExists()
    {
        var ds = new TestDataSet();
        var dt = ds.Parents;
        var target = new Parent();

        ds.Popolate();

        Mapper.Map(target, dt.Rows[0], new Dictionary<string, MappingBase>() { { nameof(Parent.Category), new RelationMapping("ParentCategory") } });

        Assert.IsNotNull(target.Category);
        Assert.AreEqual(CategoryValue.Code1, target.Category.Code);
        Assert.AreEqual(CategoryValue.Code2, target.Category.Value);
    }

    [TestMethod]
    public void MapColumnNotExists()
    {
        var ds = new TestDataSet();
        var dt = ds.Parents;
        var target = new Parent();

        ds.Popolate();

        Mapper.Map(target, dt.Rows[1], new Dictionary<string, MappingBase>() { { nameof(Parent.Category), new RelationMapping("ParentCategory") } });

        Assert.IsNull(target.Category);
    }
}