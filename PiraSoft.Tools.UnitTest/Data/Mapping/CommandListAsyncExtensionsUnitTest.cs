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
public class CommandListAsyncExtensionsUnitTest
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
        connectionManagerMock.GetDataTableAsync(Arg.Is("StoredProcedure"),
            Arg.Is(CommandType.StoredProcedure),
            Arg.Is<int?>(v => v == null),
            Arg.Is<IEnumerable<SqlParameter>>(v => !v.Any()))
            .Returns(Task.FromResult((DataTable)dt));
    }

    [TestMethod]
    public async Task ListFactoryAsync()
    {
        var target = await _command.ListAsync(() => (object)new SimpleModel());

        Assert.IsNotNull(target);
        Assert.AreEqual(2, target.Count());
    }

    [TestMethod]
    public async Task ListFactoryExplicitAsync()
    {
        var mappings = new TypeMappings(typeof(AnotherModel));

        mappings.AddMapping("AnotherStringProperty", new ColumnMapping("AnotherStringColumn"));

        var target = await _command.ListAsync(() => (object)new AnotherModel(), mappings);

        Assert.IsNotNull(target);
        Assert.AreEqual(2, target.Count());
    }

    [TestMethod]
    public async Task ListGenericAsync()
    {
        var target = await _command.ListAsync<SimpleModel, SqlDataReader, SqlParameter>();

        Assert.IsNotNull(target);
        Assert.AreEqual(2, target.Count());
    }

    [TestMethod]
    public async Task ListGenericExplicitAsync()
    {
        var mappings = new TypeMappings(typeof(AnotherModel));

        mappings.AddMapping("AnotherStringProperty", new ColumnMapping("AnotherStringColumn"));

        var target = await _command.ListAsync<AnotherModel, SqlDataReader, SqlParameter>(mappings);

        Assert.IsNotNull(target);
        Assert.AreEqual(2, target.Count());
    }

    [TestMethod]
    public async Task ListGenericFactoryAsync()
    {
        var target = await _command.ListAsync(() => new SimpleModel());

        Assert.IsNotNull(target);
        Assert.AreEqual(2, target.Count());
    }

    [TestMethod]
    public async Task ListGericFactoryExplicitAsync()
    {
        var mappings = new TypeMappings(typeof(AnotherModel));

        mappings.AddMapping("AnotherStringProperty", new ColumnMapping("AnotherStringColumn"));

        var target = await _command.ListAsync(() => new AnotherModel(), mappings);

        Assert.IsNotNull(target);
        Assert.AreEqual(2, target.Count());
    }
}
