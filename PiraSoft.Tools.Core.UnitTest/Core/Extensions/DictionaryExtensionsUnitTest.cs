using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace PiraSoft.Tools.UnitTest.Core.Extensions;

[TestClass]
public class DictionaryExtensionsUnitTest
{
    [TestMethod]
    public void MergeDistinct()
    {
        Dictionary<int, string>? target = new Dictionary<int, string>()
        {
            {1, "Target One" },
            {2, "Target Two" }
        };

        Dictionary<int, string>? other = new Dictionary<int, string>()
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
        Dictionary<int, string>? target = new Dictionary<int, string>()
        {
            {1, "Target One" },
            {2, "Target Two" }
        };

        Dictionary<int, string>? other = new Dictionary<int, string>()
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
        Dictionary<int, string>? target = new Dictionary<int, string>()
        {
            {1, "Target One" },
            {2, "Target Two" }
        };

        Dictionary<int, string>? other = new Dictionary<int, string>()
        {
            {2, "Other Two" },
            {3, "Other Three" }
        };

        target.Merge(other, false);

        Assert.AreEqual(3, target.Count);
        Assert.AreEqual("Other Two", target[2]);
    }
}