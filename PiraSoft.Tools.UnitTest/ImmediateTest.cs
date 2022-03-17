using Microsoft.VisualStudio.TestTools.UnitTesting;
using PiraSoft.Tools.Data.Mapping;
using PiraSoft.Tools.Data.Mapping.Attributes;
using System;

namespace PiraSoft.Tools.UnitTest.Data.Mapping;

[TestClass]
public class ImmediateTests
{
    [TestMethod]
    public void Immediate()
    {
        //Assert.IsTrue(typeof(string).IsValueType, "string");
        Assert.IsTrue(typeof(DateTime).IsValueType, "datetime");
    }
}