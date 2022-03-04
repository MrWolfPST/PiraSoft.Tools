using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace PiraSoft.Tools.UnitTest.Core.Extensions;

[TestClass]
public class ArrayExtensionsUnitTest
{
    [TestMethod]
    public void IndexOf()
    {
        int[]? target = new int[] { 10, 20, 30, 40, 50, 10, 20, 30, 40 };

        Assert.AreEqual(2, target.IndexOf(30));
    }

    [TestMethod]
    public void IndexOfFrom()
    {
        int[]? target = new int[] { 10, 20, 30, 40, 50, 10, 20, 30, 40 };

        Assert.AreEqual(5, target.IndexOf(10, 2));
    }

    [TestMethod]
    public void IndexOfFromTo()
    {
        int[]? target = new int[] { 10, 20, 30, 40, 50, 10, 20, 30, 40 };

        Assert.AreEqual(-1, target.IndexOf(10, 2, 2));
    }
}