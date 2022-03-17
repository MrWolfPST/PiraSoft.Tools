using Microsoft.VisualStudio.TestTools.UnitTesting;
using PiraSoft.Tools.Data.Mapping;
using System;

namespace PiraSoft.Tools.UnitTest.Data.Mapping;

[TestClass]
public class ColumnMappingUnitTest
{
    [TestMethod]
    public void Validation()
    {
        Assert.ThrowsException<ArgumentException>(() => new ColumnMapping(null), "null");
        Assert.ThrowsException<ArgumentException>(() => new ColumnMapping(""), "empty");
        Assert.ThrowsException<ArgumentException>(() => new ColumnMapping(" "), "white space");
    }

    [TestMethod]
    public void MapColumnExists()
    {
        var dt = new TestDataTable();
        var target = new SimpleModel();

        Mapper.Map(target, dt.Rows[0]);

        Assert.AreEqual("StringValue", target.StringProperty);
    }

    [TestMethod]
    public void MapColumnNotExists()
    {
        var dt = new TestDataTable();
        var target = new AnotherModel();

        Mapper.Map(target, dt.Rows[0]);

        Assert.IsNull(target.AnotherStringProperty);
    }
}