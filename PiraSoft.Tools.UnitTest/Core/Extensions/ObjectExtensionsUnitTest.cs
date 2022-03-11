using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace PiraSoft.Tools.UnitTest.Core.Extensions;

[TestClass]
public class ObjectExtensionsUnitTest
{
    [TestMethod]
    public void GetNotImplementedException()
        => Assert.IsInstanceOfType(new object().GetNotImplementedException(), typeof(NotImplementedException));

    [TestMethod]
    public void GetNotSupportedException()
        => Assert.IsInstanceOfType(new object().GetNotSupportedException(), typeof(NotSupportedException));

    [TestMethod]
    public void ThrowNotImplementedException()
        => Assert.ThrowsException<NotImplementedException>(() => new object().ThrowNotImplementedException());

    [TestMethod]
    public void ThrowNotSupportedException()
        => Assert.ThrowsException<NotSupportedException>(() => new object().ThrowNotSupportedException());

    [TestMethod]
    public void GetNotPublicFieldValue()
    {
        var target = new PrivateFieldClass();

        Assert.AreEqual("private", target.GetNotPublicFieldValue<string>("_privateField"));
        Assert.AreEqual("protected", target.GetNotPublicFieldValue<string>("protectedField"));
        Assert.AreEqual("internal", target.GetNotPublicFieldValue<string>("_internalField"));
    }
}

internal class PrivateFieldClass
{
    private string _privateField = "private";
    protected string protectedField = "protected";
    internal string _internalField = "internal";

    public string PrivateField { get => _privateField; set => _privateField = value; }
}