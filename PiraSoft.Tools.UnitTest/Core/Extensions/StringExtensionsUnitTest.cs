using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace PiraSoft.Tools.UnitTest.Core.Extensions;

[TestClass]
public class StringExtensionsUnitTest
{
    [TestMethod]
    public void Append()
    {
        string? target = null;

        Assert.IsNull(target.Append("Second"), "Append to null");

        target = string.Empty;

        Assert.AreEqual(string.Empty, target.Append("Second", true), "Append to empty check IsNullOrEmpty");
        Assert.AreEqual("Second", target.Append("Second"), "Append to empty check only null");

        target = "First";

        Assert.AreEqual("FirstSecond", target.Append("Second"), "Append to not empty");
    }
}