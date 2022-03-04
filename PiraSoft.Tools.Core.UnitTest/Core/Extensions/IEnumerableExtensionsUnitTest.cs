using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace PiraSoft.Tools.UnitTest.Core.Extensions;

[TestClass]
public class IEnumerableExtensionsUnitTest
{
    [TestMethod]
    public void ForEach()
    {
        List<int>? target = new List<int>() { 1, 2, 3, 4 };
        List<int>? test = new List<int>();

        target.ForEach(i => test.Add(i + 1));

        Assert.AreEqual(4, test.Count);
        Assert.AreEqual(2, test[0]);
        Assert.AreEqual(3, test[1]);
        Assert.AreEqual(4, test[2]);
        Assert.AreEqual(5, test[3]);
    }
}