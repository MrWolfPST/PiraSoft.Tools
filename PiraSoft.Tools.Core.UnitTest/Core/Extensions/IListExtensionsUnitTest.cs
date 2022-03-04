using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace PiraSoft.Tools.UnitTest.Core.Extensions;

[TestClass]
public class IListExtensionsUnitTest
{
    [TestMethod]
    public void AddRangeIEnumerable()
    {
        Collection<int>? target = new Collection<int>() { 1, 2 };

        target.AddRange(Enumerable.Range(4, 5));

        Assert.AreEqual(7, target.Count);
    }

    [TestMethod]
    public void AddRangeParamArray()
    {
        Collection<int>? target = new Collection<int>() { 1, 2 };

        target.AddRange(4, 5, 6, 7, 8);

        Assert.AreEqual(7, target.Count);
    }

    [TestMethod]
    public void AddRangeList()
    {
        List<int>? target = new List<int>() { 1, 2 };

        target.AddRange(4, 5, 6, 7, 8);

        Assert.AreEqual(7, target.Count);
    }
}