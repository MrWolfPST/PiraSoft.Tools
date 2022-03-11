using Microsoft.VisualStudio.TestTools.UnitTesting;
using PiraSoft.Tools.Data.Mapping;
using PiraSoft.Tools.Data.Mapping.Attributes;
using System;

namespace PiraSoft.Tools.UnitTest.Data.Mapping;

[TestClass]
public class ColumnMappingAttributeUnitTest
{
    [TestMethod]
    public void MapValidation()
    {
        var target = new ColumnMappingAttribute("Column");

        Assert.IsInstanceOfType(target.GetMapping(), typeof(ColumnMapping));
    }
}