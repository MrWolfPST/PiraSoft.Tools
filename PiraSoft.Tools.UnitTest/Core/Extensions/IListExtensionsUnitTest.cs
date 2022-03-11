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
        var targetCollection = new Collection<int>() { 1, 2 };

        targetCollection.AddRange(Enumerable.Range(4, 5));

        Assert.AreEqual(7, targetCollection.Count, "AddRange IEnumerable");

        targetCollection = new Collection<int>() { 1, 2 };

        targetCollection.AddRange(4, 5, 6, 7, 8);

        Assert.AreEqual(7, targetCollection.Count, "AddRange ParamArray");

        var targetList = new List<int>() { 1, 2 };

        targetList.AddRange(4, 5, 6, 7, 8);

        Assert.AreEqual(7, targetList.Count, "AddRange List");
    }
}