using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using System.Threading.Tasks;

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
        Assert.AreEqual("protected", target.GetNotPublicFieldValue<string>("ProtectedField"));
        Assert.AreEqual("internal", target.GetNotPublicFieldValue<string>("_internalField"));
    }

    [TestMethod]
    public async Task ExecuteAndReturnAsync()
    {
        var check = 0;
        var target = new { Val = 1 };
        var result = await target.ExecuteAndReturnAsync(o => Task.Run(() => check = o.Val));

        Assert.AreEqual(1, check);
        Assert.IsInstanceOfType(result, target.GetType());
    }

    [TestMethod]
    public async Task CancellableExecuteAndReturnAsync()
    {
        var check = 0;
        var target = new { Val = 1 };
        var result = await target.ExecuteAndReturnAsync((o, c) => Task.Run(() => check = o.Val), CancellationToken.None);

        Assert.AreEqual(1, check);
        Assert.IsInstanceOfType(result, target.GetType());
    }
}

internal class PrivateFieldClass
{
    private string _privateField = "private";
    protected string ProtectedField = "protected";
    internal string _internalField = "internal";

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S2292:Trivial properties should be auto-implemented")]
    public string PrivateField { get => _privateField; set => _privateField = value; }
}