using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PiraSoft.Tools.Core.UnitTest;
[TestClass]
public class DictionaryExtensionsUnitTest
{
    [TestMethod]
    public void MergeDistinct()
    {
        var target = new Dictionary<int, string>() 
        {
            {1, "Target One" },
            {2, "Target Two" }
        };

        var other = new Dictionary<int, string>()
        {
            {3, "Other Three" },
            {4, "Other Four" }
        };

        target.Merge(other);

        Assert.AreEqual(4, target.Count);
    }

    [TestMethod]
    public void MergePreserve()
    {
        var target = new Dictionary<int, string>()
        {
            {1, "Target One" },
            {2, "Target Two" }
        };

        var other = new Dictionary<int, string>()
        {
            {2, "Other Two" },
            {3, "Other Three" }
        };

        target.Merge(other);

        Assert.AreEqual(3, target.Count);
        Assert.AreEqual("Target Two", target[2]);
    }

    [TestMethod]
    public void MergeUpdate()
    {
        var target = new Dictionary<int, string>()
        {
            {1, "Target One" },
            {2, "Target Two" }
        };

        var other = new Dictionary<int, string>()
        {
            {2, "Other Two" },
            {3, "Other Three" }
        };

        target.Merge(other, false);

        Assert.AreEqual(3, target.Count);
        Assert.AreEqual("Other Two", target[2]);
    }
}