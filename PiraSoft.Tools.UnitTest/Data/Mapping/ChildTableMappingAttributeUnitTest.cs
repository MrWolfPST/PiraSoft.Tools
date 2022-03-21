using Microsoft.VisualStudio.TestTools.UnitTesting;
using PiraSoft.Tools.Data.Mapping;
using PiraSoft.Tools.Data.Mapping.Attributes;

namespace PiraSoft.Tools.UnitTest.Data.Mapping;

[TestClass]
public class ChildTableMappingAttributeUnitTest
{
    [TestMethod]
    public void MappingValidationTableName()
    {
        var attribute = new ChildTableMappingAttribute("TableName");
        var target = attribute.GetMapping() as ChildTableMapping;

        Assert.IsNotNull(target);
        Assert.AreEqual("TableName", target.TableName);
    }

    [TestMethod]
    public void MappingValidationTableIndex()
    {
        var attribute = new ChildTableMappingAttribute(1);
        var target = attribute.GetMapping() as ChildTableMapping;

        Assert.IsNotNull(target);
        Assert.AreEqual(1, target.TableIndex);
    }
}