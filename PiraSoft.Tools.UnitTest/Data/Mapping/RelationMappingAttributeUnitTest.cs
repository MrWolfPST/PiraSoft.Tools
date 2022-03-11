using Microsoft.VisualStudio.TestTools.UnitTesting;
using PiraSoft.Tools.Data.Mapping;
using PiraSoft.Tools.Data.Mapping.Attributes;
using System;

namespace PiraSoft.Tools.UnitTest.Data.Mapping;

[TestClass]
public class RelationMappingAttributeUnitTest
{
    [TestMethod]
    public void MappingValidation()
    {
        var target = new RelationMappingAttribute("RelationName");

        Assert.IsInstanceOfType(target.GetMapping(), typeof(RelationMapping));
    }
}