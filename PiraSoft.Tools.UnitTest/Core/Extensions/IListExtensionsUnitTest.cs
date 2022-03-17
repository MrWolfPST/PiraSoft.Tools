using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace PiraSoft.Tools.UnitTest.Core.Extensions;

[TestClass]
public class IListExtensionsUnitTest
{
    [TestMethod]
    public void AddRange()
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

    [TestMethod]
    public void ToArrayValidation()
    {
        IList? target = null;

#pragma warning disable CS8604 // Possible null reference argument.
        Assert.ThrowsException<ArgumentNullException>(() => target.ToArray<int>());
#pragma warning restore CS8604 // Possible null reference argument.

        target = new List<int>();

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        Assert.ThrowsException<ArgumentNullException>(() => target.ToArray(null));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
    }

    [TestMethod]
    public void ToArrayGeneric()
    {
        var target = (IList)new List<int>() { 1, 2, 3 };
        var result = target.ToArray<int>();

        Assert.IsInstanceOfType(result, typeof(int[]));
    }

    [TestMethod]
    public void ToArrayType()
    {
        var target = (IList)new List<int>() { 1, 2, 3 };
        var result = target.ToArray(typeof(int));

        Assert.IsInstanceOfType(result, typeof(int[]));
    }
}