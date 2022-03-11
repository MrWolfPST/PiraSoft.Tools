using Microsoft.VisualStudio.TestTools.UnitTesting;
using PiraSoft.Tools.Data.Mapping;
using System;
using System.Data;
using System.IO;
using System.Reflection;

namespace PiraSoft.Tools.UnitTest.Data.Mapping;

[TestClass]
public class MappingBaseUnitTest
{
    internal class MappingBaseWrapper : MappingBase
    {
        protected internal override void Map(object target, PropertyInfo propertyInfo, DataRow row)
            => throw new NotImplementedException();

        public object? ConvertWrapper(object? value, Type targetType)
            => base.Convert(value, targetType);

        public void SetValueWrapper(object target, PropertyInfo propertyInfo, object? value)
            => base.SetValue(target, propertyInfo, value);
    }

    [TestMethod]
    public void Convert()
    {
        var target = new MappingBaseWrapper();

        Assert.AreEqual(2, target.ConvertWrapper("2", typeof(int)));
    }

    [TestMethod]
    public void ConvertNull()
    {
        var target = new MappingBaseWrapper();

        Assert.IsNull(target.ConvertWrapper(null, typeof(string)));
    }

    [TestMethod]
    public void ConvertDbNull()
    {
        var target = new MappingBaseWrapper();

        Assert.IsNull(target.ConvertWrapper(DBNull.Value, typeof(string)));
    }

    [TestMethod]
    public void ConvertEnumString()
    {
        var target = new MappingBaseWrapper();

        Assert.AreEqual(ConnectionState.Connecting, target.ConvertWrapper("Connecting", typeof(ConnectionState)));
    }

    [TestMethod]
    public void ConvertEnumInt()
    {
        var target = new MappingBaseWrapper();

        Assert.AreEqual(ConnectionState.Connecting, target.ConvertWrapper(2, typeof(ConnectionState)));
    }

    [TestMethod]
    public void ConvertStream()
    {
        var target = new MappingBaseWrapper();
        var value = target.ConvertWrapper(Array.Empty<byte>(), typeof(Stream));

        Assert.IsInstanceOfType(value, typeof(MemoryStream));
    }

    [TestMethod]
    public void ConvertInvalidCast()
    {
        var target = new MappingBaseWrapper();

        Assert.ThrowsException<InvalidCastException>(() => target.ConvertWrapper(target, typeof(MemoryStream)));
    }

    [TestMethod]
    public void SetValueException()
    {
        var target = new MappingBaseWrapper();
        var model = new SimpleModel();
        var propertyInfo = model.GetType().GetProperty("ReadOnlyProperty") ?? throw new Exception();

        Assert.ThrowsException<ArgumentException>(() => target.SetValueWrapper(model, propertyInfo, ""));
    }
}