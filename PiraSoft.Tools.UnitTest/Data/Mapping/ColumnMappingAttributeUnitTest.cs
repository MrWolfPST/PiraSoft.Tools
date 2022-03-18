using Microsoft.VisualStudio.TestTools.UnitTesting;
using PiraSoft.Tools.Data.Mapping;
using PiraSoft.Tools.Data.Mapping.Attributes;

namespace PiraSoft.Tools.UnitTest.Data.Mapping;

[TestClass]
public class ColumnMappingAttributeUnitTest
{
    [TestMethod]
    public void MappingValidation()
    {
        var attribute = new ColumnMappingAttribute("Column");
        var target = attribute.GetMapping() as ColumnMapping;

        Assert.IsNotNull(target);
        Assert.AreEqual("Column", target.ColumnName);
    }
}