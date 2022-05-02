using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PiraSoft.Tools.Data;
using PiraSoft.Tools.Data.Mapping;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PiraSoft.Tools.UnitTest.Data.Mapping;

[TestClass]
public class IConnectionManagerGetAsyncExtensionsUnitTest
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
        _connectionManagerMock.GetDataRowAsync(Arg.Any<string>(),
            Arg.Any<CommandType>(),
            Arg.Any<int?>(),
            Arg.Any<IEnumerable<SqlParameter>?>())
            .Returns(Task.FromResult((DataRow?)dt.Rows[0]));
    }

    [TestMethod]
    public async Task GetFactoryAsync()
    {
        var target = (SimpleModel?)await _connectionManagerMock.GetAsync(() => (object)new SimpleModel(), "StoredProcedure", CommandType.StoredProcedure);

        Assert.IsNotNull(target);
        Assert.AreEqual("StringValue", target.StringProperty);
    }

    [TestMethod]
    public async Task GetFactoryExplicitAsync()
    {
        var mappings = new TypeMappings(typeof(AnotherModel));

        mappings.AddMapping("AnotherStringProperty", new ColumnMapping("AnotherStringColumn"));

        var target = (AnotherModel?)await _connectionManagerMock.GetAsync(() => (object)new AnotherModel(), mappings, "StoredProcedure", CommandType.StoredProcedure);

        Assert.IsNotNull(target);
        Assert.AreEqual("AnotherStringValue", target.AnotherStringProperty);
    }

    [TestMethod]
    public async Task GetGenericAsync()
    {
        var target = await _connectionManagerMock.GetAsync<SimpleModel, SqlDataReader, SqlParameter>("StoredProcedure", CommandType.StoredProcedure);

        Assert.IsNotNull(target);
        Assert.AreEqual("StringValue", target.StringProperty);
    }

    [TestMethod]
    public async Task GetGenericExplicitAsync()
    {
        var mappings = new TypeMappings(typeof(AnotherModel));

        mappings.AddMapping("AnotherStringProperty", new ColumnMapping("AnotherStringColumn"));

        var target = await _connectionManagerMock.GetAsync<AnotherModel, SqlDataReader, SqlParameter>(mappings, "StoredProcedure", CommandType.StoredProcedure);

        Assert.IsNotNull(target);
        Assert.AreEqual("AnotherStringValue", target.AnotherStringProperty);
    }

    [TestMethod]
    public async Task GetGenericFactoryAsync()
    {
        var target = await _connectionManagerMock.GetAsync(() => new SimpleModel(), "StoredProcedure", CommandType.StoredProcedure);

        Assert.IsNotNull(target);
        Assert.AreEqual("StringValue", target.StringProperty);
    }

    [TestMethod]
    public async Task GetGericFactoryExplicitAsync()
    {
        var mappings = new TypeMappings(typeof(AnotherModel));

        mappings.AddMapping("AnotherStringProperty", new ColumnMapping("AnotherStringColumn"));

        var target = await _connectionManagerMock.GetAsync(() => new AnotherModel(), mappings, "StoredProcedure", CommandType.StoredProcedure);

        Assert.IsNotNull(target);
        Assert.AreEqual("AnotherStringValue", target.AnotherStringProperty);
    }
}
