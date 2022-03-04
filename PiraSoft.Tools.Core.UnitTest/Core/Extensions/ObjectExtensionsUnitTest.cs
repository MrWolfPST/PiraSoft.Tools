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
}