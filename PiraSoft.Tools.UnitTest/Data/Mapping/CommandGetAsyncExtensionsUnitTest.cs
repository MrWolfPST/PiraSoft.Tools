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
public class CommandGetAsyncExtensionsUnitTest
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private Command<SqlDataReader, SqlParameter> _command;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    [TestInitialize]
    public void Init()
    {
        var connectionManagerMock = Substitute.For<IConnectionManager<SqlDataReader, SqlParameter>>();
        _command = connectionManagerMock.StoredProcedure("StoredProcedure");
        var dt = new TestDataTable();

        connectionManagerMock.Factory.Returns(SqlClientFactory.Instance);
        connectionManagerMock.GetDataRowAsync(Arg.Is("StoredProcedure"),
            Arg.Is(CommandType.StoredProcedure),
            Arg.Is<int?>(v => v == null),
            Arg.Is<IEnumerable<SqlParameter>>(v => !v.Any()))
            .Returns(Task.FromResult<DataRow?>(dt.Rows[0]));
    }

    [TestMethod]
    public async Task GetFactoryAsync()
    {
        var target = (SimpleModel?)await _command.GetAsync(() => (object)new SimpleModel());

        Assert.IsNotNull(target);
        Assert.AreEqual("StringValue", target.StringProperty);
    }

    [TestMethod]
    public async Task GetFactoryExplicitAsync()
    {
        var mappings = new TypeMappings(typeof(AnotherModel));

        mappings.AddMapping("AnotherStringProperty", new ColumnMapping("AnotherStringColumn"));

        var target = (AnotherModel?)await _command.GetAsync(() => (object)new AnotherModel(), mappings);

        Assert.IsNotNull(target);
        Assert.AreEqual("AnotherStringValue", target.AnotherStringProperty);
    }

    [TestMethod]
    public async Task GetGenericAsync()
    {
        var target = await _command.GetAsync<SimpleModel, SqlDataReader, SqlParameter>();

        Assert.IsNotNull(target);
        Assert.AreEqual("StringValue", target.StringProperty);
    }

    [TestMethod]
    public async Task GetGenericExplicitAsync()
    {
        var mappings = new TypeMappings(typeof(AnotherModel));

        mappings.AddMapping("AnotherStringProperty", new ColumnMapping("AnotherStringColumn"));

        var target = await _command.GetAsync<AnotherModel, SqlDataReader, SqlParameter>(mappings);

        Assert.IsNotNull(target);
        Assert.AreEqual("AnotherStringValue", target.AnotherStringProperty);
    }

    [TestMethod]
    public async Task GetGenericFactoryAsync()
    {
        var target = await _command.GetAsync(() => new SimpleModel());

        Assert.IsNotNull(target);
        Assert.AreEqual("StringValue", target.StringProperty);
    }

    [TestMethod]
    public async Task GetGericFactoryExplicitAsync()
    {
        var mappings = new TypeMappings(typeof(AnotherModel));

        mappings.AddMapping("AnotherStringProperty", new ColumnMapping("AnotherStringColumn"));

        var target = await _command.GetAsync(() => new AnotherModel(), mappings);

        Assert.IsNotNull(target);
        Assert.AreEqual("AnotherStringValue", target.AnotherStringProperty);
    }
}
