using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PiraSoft.Tools.Data;
using PiraSoft.Tools.Data.Mapping;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace PiraSoft.Tools.UnitTest.Data.Mapping;

[TestClass]
public class CommandGetExtensionsUnitTest
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private IConnectionManager<SqlDataReader, SqlParameter> _connectionManagerMock;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    [TestInitialize]
    public void Init()
    {
        _connectionManagerMock = Substitute.For<IConnectionManager<SqlDataReader, SqlParameter>>();
        var dt = new TestDataTable();

        _connectionManagerMock.Factory.Returns(SqlClientFactory.Instance);
        _connectionManagerMock.GetDataRow(Arg.Any<string>(),
            Arg.Any<CommandType>(),
            Arg.Any<int?>(),
            Arg.Any<IEnumerable<SqlParameter>?>())
            .Returns(dt.Rows[0]);
    }

    [TestMethod]
    public void GetFactoryAsync()
    {
        var target = (SimpleModel?)_connectionManagerMock.Get(() => (object)new SimpleModel(), "StoredProcedure", CommandType.StoredProcedure);

        Assert.IsNotNull(target);
        Assert.AreEqual("StringValue", target.StringProperty);
    }

    [TestMethod]
    public void GetFactoryExplicitAsync()
    {
        var mappings = new TypeMappings(typeof(AnotherModel));

        mappings.AddMapping("AnotherStringProperty", new ColumnMapping("AnotherStringColumn"));

        var target = (AnotherModel?)_connectionManagerMock.Get(() => (object)new AnotherModel(), mappings, "StoredProcedure", CommandType.StoredProcedure);

        Assert.IsNotNull(target);
        Assert.AreEqual("AnotherStringValue", target.AnotherStringProperty);
    }

    [TestMethod]
    public void GetGenericAsync()
    {
        var target = _connectionManagerMock.Get<SimpleModel, SqlDataReader, SqlParameter>("StoredProcedure", CommandType.StoredProcedure);

        Assert.IsNotNull(target);
        Assert.AreEqual("StringValue", target.StringProperty);
    }

    [TestMethod]
    public void GetGenericExplicitAsync()
    {
        var mappings = new TypeMappings(typeof(AnotherModel));

        mappings.AddMapping("AnotherStringProperty", new ColumnMapping("AnotherStringColumn"));

        var target = _connectionManagerMock.Get<AnotherModel, SqlDataReader, SqlParameter>(mappings, "StoredProcedure", CommandType.StoredProcedure);

        Assert.IsNotNull(target);
        Assert.AreEqual("AnotherStringValue", target.AnotherStringProperty);
    }

    [TestMethod]
    public void GetGenericFactoryAsync()
    {
        var target = _connectionManagerMock.Get(() => new SimpleModel(), "StoredProcedure", CommandType.StoredProcedure);

        Assert.IsNotNull(target);
        Assert.AreEqual("StringValue", target.StringProperty);
    }

    [TestMethod]
    public void GetGericFactoryExplicitAsync()
    {
        var mappings = new TypeMappings(typeof(AnotherModel));

        mappings.AddMapping("AnotherStringProperty", new ColumnMapping("AnotherStringColumn"));

        var target = _connectionManagerMock.Get(() => new AnotherModel(), mappings, "StoredProcedure", CommandType.StoredProcedure);

        Assert.IsNotNull(target);
        Assert.AreEqual("AnotherStringValue", target.AnotherStringProperty);
    }
}
