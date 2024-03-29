﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using PiraSoft.Tools.Data.Mapping;
using PiraSoft.Tools.Data.Mapping.Attributes;

namespace PiraSoft.Tools.UnitTest.Data.Mapping;

[TestClass]
public class RelationMappingAttributeUnitTest
{
    [TestMethod]
    public void MappingValidation()
    {
        var attribute = new RelationMappingAttribute("RelationName");
        var target = attribute.GetMapping() as RelationMapping;

        Assert.IsNotNull(target);
        Assert.AreEqual("RelationName", target.RelationName);
    }
}