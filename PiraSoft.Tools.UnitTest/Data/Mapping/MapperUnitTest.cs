﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using PiraSoft.Tools.Data.Mapping;
using System;

namespace PiraSoft.Tools.UnitTest.Data.Mapping;

[TestClass]
public class MapperUnitTest
{
    [TestMethod]
    public void MapValidation()
    {
        var ds = new TestDataSet();
        var dt = ds.Parents;
        var target = new Parent();

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        Assert.ThrowsException<ArgumentNullException>(() => Mapper.Map((object)null, null, null), "target");
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
        Assert.ThrowsException<ArgumentNullException>(() => Mapper.Map(new object(), null, null), "row");
        Assert.ThrowsException<ArgumentNullException>(() => Mapper.Map(null, null), "factory");
#pragma warning disable CS8603 // Possible null reference return.
        Assert.ThrowsException<InvalidOperationException>(() => Mapper.Map(() => null, dt.NewRow()), "factory return");
        Assert.IsNull(Mapper.Map(() => new Parent(), null), "null row with factory");
#pragma warning restore CS8603 // Possible null reference return.
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

        Assert.ThrowsException<ArgumentException>(() => Mapper.Map(target, dt.NewRow(), TypeMappings.Generate<Category>()), "wrong mapping type");
    }

    [TestMethod]
    public void MapExplicit()
    {
        var dt = new TestDataTable();
        var target = new AnotherModel();
        var mappings = new TypeMappings(target.GetType());

        mappings.AddMapping(() => target.AnotherStringProperty, new ColumnMapping("AnotherStringColumn"));

        Mapper.Map(target, dt.Rows[0], mappings);

        Assert.AreEqual("AnotherStringValue", target.AnotherStringProperty);
    }

    [TestMethod]
    public void MapFactory()
    {
        var dt = new TestDataTable();

        var target = Mapper.Map(() => new SimpleModel(), dt.Rows[0]);

        Assert.IsNotNull(target);
        Assert.IsInstanceOfType(target, typeof(SimpleModel));
        Assert.AreEqual("StringValue", target.StringProperty);
        Assert.IsNull(target.ReadOnlyProperty);
    }

    [TestMethod]
    public void MapGeneric()
    {
        var dt = new TestDataTable();
        var target = Mapper.Map<SimpleModel>(dt.Rows[0]);

        Assert.IsNotNull(target);
        Assert.AreEqual("StringValue", target.StringProperty);
    }

    [TestMethod]
    public void MapGenericExplicit()
    {
        var dt = new TestDataTable();
        var mappings = new TypeMappings(typeof(AnotherModel));

        mappings.AddMapping("AnotherStringProperty", new ColumnMapping("AnotherStringColumn"));

        var target = Mapper.Map<AnotherModel>(dt.Rows[0], mappings);

        Assert.IsNotNull(target);
        Assert.AreEqual("AnotherStringValue", target.AnotherStringProperty);
    }

    [TestMethod]
    public void AddTypeMappingsGeneric()
    {
        Mapper.AddTypeMappings<Parent>();

        Assert.IsTrue(Mapper.TypeMappingsCatalog.ContainsKey(typeof(Parent)));
    }

    [TestMethod]
    public void AddTypeMappingsType()
    {
        Mapper.AddTypeMappings(typeof(Parent));

        Assert.IsTrue(Mapper.TypeMappingsCatalog.ContainsKey(typeof(Parent)));
    }

    [TestMethod]
    public void AddTypeMappings()
    {
        Mapper.AddTypeMappings(new TypeMappings(typeof(Parent)));

        Assert.IsTrue(Mapper.TypeMappingsCatalog.ContainsKey(typeof(Parent)));
    }

    [TestMethod]
    public void MapValueTypes()
    {
        //TODO: Aggiungere i check per gli altri tipi (enum varie, TimeOnly e DateOnly)
        var dt = new TestDataSet.ValueTypesDataTable();
        var row = dt.NewValueTypesRow();

        row.Boolean = true;
        row.Byte = byte.MaxValue;
        row.Char = 'X';
        row.DateTime = DateTime.MaxValue;
        row.DateTimeOffset = DateTimeOffset.MaxValue;
        row.Decimal = decimal.MaxValue;
        row.Double = double.MaxValue;
        row.Float = float.MaxValue;
        row.Guid = Guid.NewGuid();
        row.Int = int.MaxValue;
        row.Long = long.MaxValue;
        row.Short = short.MaxValue;
        row.SignedByte = sbyte.MinValue;
        row.String = "String";
        row.TimeSpan = TimeSpan.MaxValue;
        row.UnsignedInt = uint.MinValue;
        row.UnsignedLong = ulong.MinValue;
        row.UnsignedShort = ushort.MinValue;

        var target = Mapper.Map<ValueTypes>(row);

        Assert.IsNotNull(target);
        Assert.AreEqual(row.Boolean, target.Boolean);
        Assert.AreEqual(row.Byte, target.Byte);
        Assert.AreEqual(row.Char, target.Char);
        Assert.AreEqual(row.DateTime, target.DateTime);
        Assert.AreEqual(row.DateTimeOffset, target.DateTimeOffset);
        Assert.AreEqual(row.Decimal, target.Decimal);
        Assert.AreEqual(row.Double, target.Double);
        Assert.AreEqual(row.Float, target.Float);
        Assert.AreEqual(row.Guid, target.Guid);
        Assert.AreEqual(row.Int, target.Int);
        Assert.AreEqual(row.Long, target.Long);
        Assert.AreEqual(row.Short, target.Short);
        Assert.AreEqual(row.SignedByte, target.SignedByte);
        Assert.AreEqual(row.String, target.String);
        Assert.AreEqual(row.TimeSpan, target.TimeSpan);
        Assert.AreEqual(row.UnsignedInt, target.UnsignedInt);
        Assert.AreEqual(row.UnsignedLong, target.UnsignedLong);
        Assert.AreEqual(row.UnsignedShort, target.UnsignedShort);
    }
}