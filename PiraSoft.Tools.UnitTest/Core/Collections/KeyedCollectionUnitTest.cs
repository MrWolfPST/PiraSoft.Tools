using Microsoft.VisualStudio.TestTools.UnitTesting;
using PiraSoft.Tools.Core.Collections;
using System;

namespace PiraSoft.Tools.UnitTest.Core.Extensions;

[TestClass]
public class KeyedCollectionUnitTest
{
    internal class Model
    {
        public Model(string key)
            => this.Key = key;

        public string Key { get; }

        public string? Value { get; set; }
    }

    [TestMethod]
    public void Validation()
    {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        Assert.ThrowsException<ArgumentNullException>(()=> new KeyedCollection<string, Model>(null));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
    }

    [TestMethod]
    public void GetKeyForItemDelegate()
    {
        var target = new KeyedCollection<string, Model>(i => i.Key);

        target.Add(new Model("Key") { Value = "Value" });

        Assert.AreEqual("Value", target["Key"].Value);
    }
}