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
public class IConnectionManagerGetExtensionsUnitTest
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
        connectionManagerMock.GetDataRow(Arg.Is("StoredProcedure"),
            Arg.Is(CommandType.StoredProcedure),
            Arg.Is<int?>(v => v == null),
            Arg.Is<IEnumerable<SqlParameter>>(v => !v.Any()))
            .Returns(dt.Rows[0]);
    }

    [TestMethod]
    public void GetFactoryAsync()
    {
        var target = (SimpleModel?)_command.Get(() => (object)new SimpleModel());

        Assert.IsNotNull(target);
        Assert.AreEqual("StringValue", target.StringProperty);
    }

    [TestMethod]
    public void GetFactoryExplicitAsync()
    {
        var mappings = new TypeMappings(typeof(AnotherModel));

        mappings.AddMapping("AnotherStringProperty", new ColumnMapping("AnotherStringColumn"));

        var target = (AnotherModel?)_command.Get(() => (object)new AnotherModel(), mappings);

        Assert.IsNotNull(target);
        Assert.AreEqual("AnotherStringValue", target.AnotherStringProperty);
    }

    [TestMethod]
    public void GetGenericAsync()
    {
        var target = _command.Get<SimpleModel, SqlDataReader, SqlParameter>();

        Assert.IsNotNull(target);
        Assert.AreEqual("StringValue", target.StringProperty);
    }

    [TestMethod]
    public void GetGenericExplicitAsync()
    {
        var mappings = new TypeMappings(typeof(AnotherModel));

        mappings.AddMapping("AnotherStringProperty", new ColumnMapping("AnotherStringColumn"));

        var target = _command.Get<AnotherModel, SqlDataReader, SqlParameter>(mappings);

        Assert.IsNotNull(target);
        Assert.AreEqual("AnotherStringValue", target.AnotherStringProperty);
    }

    [TestMethod]
    public void GetGenericFactoryAsync()
    {
        var target = _command.Get(() => new SimpleModel());

        Assert.IsNotNull(target);
        Assert.AreEqual("StringValue", target.StringProperty);
    }

    [TestMethod]
    public void GetGericFactoryExplicitAsync()
    {
        var mappings = new TypeMappings(typeof(AnotherModel));

        mappings.AddMapping("AnotherStringProperty", new ColumnMapping("AnotherStringColumn"));

        var target = _command.Get(() => new AnotherModel(), mappings);

        Assert.IsNotNull(target);
        Assert.AreEqual("AnotherStringValue", target.AnotherStringProperty);
    }
}
