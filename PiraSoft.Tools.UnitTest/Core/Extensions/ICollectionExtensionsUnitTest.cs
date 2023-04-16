using Microsoft.VisualStudio.TestTools.UnitTesting;
using PiraSoft.Tools.Core.Extensions;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace PiraSoft.Tools.UnitTest.Core.Extensions;

[TestClass]
public class ICollectionExtensionsUnitTest
{
    [TestMethod]
    public void AddAndReturn()
    {
        var target = new Collection<string>();
        var item = target.AddAndReturn("test");

        Assert.AreEqual(1, target.Count);
        Assert.AreEqual("test", item);
        Assert.AreEqual(item, target.FirstOrDefault());
    }
}