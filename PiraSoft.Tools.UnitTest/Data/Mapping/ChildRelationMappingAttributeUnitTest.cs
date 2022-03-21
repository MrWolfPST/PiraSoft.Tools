using Microsoft.VisualStudio.TestTools.UnitTesting;
using PiraSoft.Tools.Data.Mapping;
using PiraSoft.Tools.Data.Mapping.Attributes;

namespace PiraSoft.Tools.UnitTest.Data.Mapping;

[TestClass]
public class ChildRelationMappingAttributeUnitTest
{
    [TestMethod]
    public void MappingValidation()
    {
        var attribute = new ChildRelationMappingAttribute("RelationName");
        var target = attribute.GetMapping() as ChildRelationMapping;

        Assert.IsNotNull(target);
        Assert.AreEqual("RelationName", target.RelationName);
    }
}