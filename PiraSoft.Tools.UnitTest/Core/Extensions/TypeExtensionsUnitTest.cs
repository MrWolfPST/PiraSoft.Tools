using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace PiraSoft.Tools.UnitTest.Core.Extensions;

[TestClass]
public class TypeExtensionsUnitTest
{
    internal class BaseClass
    { }

    internal class DerivedClass : BaseClass
    { }

    [TestMethod]
    public void IsEnumerable()
    {
        var list = new List<object>();
        var array = Array.Empty<object>();
        var obj = new object();

        Assert.IsTrue(list.GetType().IsEnumerable());
        Assert.IsTrue(array.GetType().IsEnumerable());
        Assert.IsFalse(obj.GetType().IsEnumerable());
    }

    [TestMethod]
    public void IsNumericDatatype()
    {
        Assert.IsTrue(typeof(byte).IsNumericDatatype());
        Assert.IsTrue(typeof(sbyte).IsNumericDatatype());
        Assert.IsTrue(typeof(ushort).IsNumericDatatype());
        Assert.IsTrue(typeof(uint).IsNumericDatatype());
        Assert.IsTrue(typeof(ulong).IsNumericDatatype());
        Assert.IsTrue(typeof(short).IsNumericDatatype());
        Assert.IsTrue(typeof(int).IsNumericDatatype());
        Assert.IsTrue(typeof(long).IsNumericDatatype());
        Assert.IsTrue(typeof(decimal).IsNumericDatatype());
        Assert.IsTrue(typeof(double).IsNumericDatatype());
        Assert.IsTrue(typeof(float).IsNumericDatatype());
        Assert.IsFalse(typeof(string).IsNumericDatatype());
        Assert.IsFalse(typeof(bool).IsNumericDatatype());
        Assert.IsFalse(typeof(char).IsNumericDatatype());
        Assert.IsFalse(typeof(DateTime).IsNumericDatatype());
        Assert.IsFalse(typeof(object).IsNumericDatatype());
    }

    [TestMethod]
    public void IsSameOrSubclassGeneric()
    {
        var baseObject = new BaseClass();
        var derivedObject = new DerivedClass();

        Assert.IsTrue(baseObject.GetType().IsSameOrSubclass<BaseClass>());
        Assert.IsFalse(baseObject.GetType().IsSameOrSubclass<DerivedClass>());
        Assert.IsFalse(baseObject.GetType().IsSameOrSubclass<List<object>>());

        Assert.IsTrue(derivedObject.GetType().IsSameOrSubclass<BaseClass>());
        Assert.IsTrue(derivedObject.GetType().IsSameOrSubclass<DerivedClass>());
        Assert.IsFalse(derivedObject.GetType().IsSameOrSubclass<List<object>>());
    }

    [TestMethod]
    public void IsSameOrSubclass()
    {
        var baseObject = new BaseClass();
        var derivedObject = new DerivedClass();

        Assert.IsTrue(baseObject.GetType().IsSameOrSubclass(typeof(BaseClass)));
        Assert.IsFalse(baseObject.GetType().IsSameOrSubclass(typeof(DerivedClass)));
        Assert.IsFalse(baseObject.GetType().IsSameOrSubclass(typeof(List<object>)));

        Assert.IsTrue(derivedObject.GetType().IsSameOrSubclass(typeof(BaseClass)));
        Assert.IsTrue(derivedObject.GetType().IsSameOrSubclass(typeof(DerivedClass)));
        Assert.IsFalse(derivedObject.GetType().IsSameOrSubclass(typeof(List<object>)));
    }

    [TestMethod]
    public void IsNullableType()
    {
        Assert.IsTrue(typeof(int?).IsNullableType());
        Assert.IsFalse(typeof(int).IsNullableType());
    }
}