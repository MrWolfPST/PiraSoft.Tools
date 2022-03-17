using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace PiraSoft.Tools.UnitTest.Core.Extensions;

[TestClass]
public class IDictionaryExtensionsUnitTest
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

    [TestMethod]
    public void TryGetValue()
    {
        var target = new Dictionary<int, string>()
        {
            {1, "Target One" },
            {2, "Target Two" }
        };

        Assert.IsNull(target.TryGetValue(5));
        Assert.AreEqual("Target Two", target.TryGetValue(2));
    }

    [TestMethod]
    public void AddOrUpdateAdd()
    {
        var target = new Dictionary<string, object>();

        target.AddOrUpdate("Key1", new object());

        Assert.AreEqual(1, target.Count);
    }

    [TestMethod]
    public void AddOrUpdateUpdate()
    {
        var target = new Dictionary<string, object>();

        target.AddOrUpdate("Key1", new object());
        target.AddOrUpdate("Key1", new object());

        Assert.AreEqual(1, target.Count);
    }
}