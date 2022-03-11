using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace PiraSoft.Tools.UnitTest.Core.Extensions;

[TestClass]
public class ArrayExtensionsUnitTest
{
    [TestMethod]
    public void IndexOf()
    {
        var target = new int[] { 10, 20, 30, 40, 50, 10, 20, 30, 40 };

        Assert.AreEqual(2, target.IndexOf(30), "IndexOf(value)");
        Assert.AreEqual(5, target.IndexOf(10, 2), "IndexOf(value, from)");
        Assert.AreEqual(-1, target.IndexOf(10, 2, 2), "IndexOf(value, from, count)");
    }
}