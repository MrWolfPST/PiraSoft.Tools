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
public class CommandListExtensionsUnitTest
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
        connectionManagerMock.GetDataTable(Arg.Is("StoredProcedure"),
            Arg.Is(CommandType.StoredProcedure),
            Arg.Is<int?>(v => v == null),
            Arg.Is<IEnumerable<SqlParameter>>(v => !v.Any()))
            .Returns(dt);
    }

    [TestMethod]
    public void ListFactory()
    {
        var target = _command.List(() => (object)new SimpleModel());

        Assert.IsNotNull(target);
        Assert.AreEqual(2, target.Count());
    }

    [TestMethod]
    public void ListFactoryExplicit()
    {
        var mappings = new TypeMappings(typeof(AnotherModel));

        mappings.AddMapping("AnotherStringProperty", new ColumnMapping("AnotherStringColumn"));

        var target = _command.List(() => (object)new AnotherModel(), mappings);

        Assert.IsNotNull(target);
        Assert.AreEqual(2, target.Count());
    }

    [TestMethod]
    public void ListGeneric()
    {
        var target = _command.List<SimpleModel, SqlDataReader, SqlParameter>();

        Assert.IsNotNull(target);
        Assert.AreEqual(2, target.Count());
    }

    [TestMethod]
    public void ListGenericExplicit()
    {
        var mappings = new TypeMappings(typeof(AnotherModel));

        mappings.AddMapping("AnotherStringProperty", new ColumnMapping("AnotherStringColumn"));

        var target = _command.List<AnotherModel, SqlDataReader, SqlParameter>(mappings);

        Assert.IsNotNull(target);
        Assert.AreEqual(2, target.Count());
    }

    [TestMethod]
    public void ListGenericFactory()
    {
        var target = _command.List(() => new SimpleModel());

        Assert.IsNotNull(target);
        Assert.AreEqual(2, target.Count());
    }

    [TestMethod]
    public void ListGericFactoryExplicit()
    {
        var mappings = new TypeMappings(typeof(AnotherModel));

        mappings.AddMapping("AnotherStringProperty", new ColumnMapping("AnotherStringColumn"));

        var target = _command.List(() => new AnotherModel(), mappings);

        Assert.IsNotNull(target);
        Assert.AreEqual(2, target.Count());
    }
}
