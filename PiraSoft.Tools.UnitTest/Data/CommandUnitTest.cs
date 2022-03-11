using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PiraSoft.Tools.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PiraSoft.Tools.UnitTest.Data;

[TestClass]
public class CommandUnitTest
{
    [TestMethod]
    public void WithTimeOut()
    {
        var connectionManagerMock = Substitute.For<IConnectionManager<SqlDataReader, SqlParameter>>();
        var command = connectionManagerMock.StoredProcedure("StoredProcedure");

        Assert.ThrowsException<ArgumentOutOfRangeException>(() => command.WithTimeOut(-1));

        command.WithTimeOut(30);
        command.ExecuteNonQuery();

        connectionManagerMock.Received().ExecuteNonQuery(Arg.Is("StoredProcedure"),
            Arg.Is(CommandType.StoredProcedure),
            Arg.Is<int?>(v => v == 30),
            Arg.Is<IEnumerable<SqlParameter>>(v => !v.Any()));
    }

    [TestMethod]
    public void WithParameter()
    {
        var connectionManagerMock = Substitute.For<IConnectionManager<SqlDataReader, SqlParameter>>();
        var command = connectionManagerMock.StoredProcedure("StoredProcedure");

        connectionManagerMock.Factory.Returns(SqlClientFactory.Instance);

        command.WithParameter("@parameter", "value");
        command.ExecuteNonQuery();

        connectionManagerMock.Received().ExecuteNonQuery(Arg.Is("StoredProcedure"),
            Arg.Is(CommandType.StoredProcedure),
            Arg.Is<int?>(v => v == null),
            Arg.Is<IEnumerable<SqlParameter>>(v => v.SingleOrDefault(i => i.ParameterName == "@parameter"
            && i.Value.ToString() == "value"
            && i.Direction == ParameterDirection.Input) != null));
    }

    [TestMethod]
    public void WithParameterNull()
    {
        var connectionManagerMock = Substitute.For<IConnectionManager<SqlDataReader, SqlParameter>>();
        var command = connectionManagerMock.StoredProcedure("StoredProcedure");

        connectionManagerMock.Factory.Returns(SqlClientFactory.Instance);

        command.WithParameter("@parameter", null, false);
        command.ExecuteNonQuery();

        connectionManagerMock.Received().ExecuteNonQuery(Arg.Is("StoredProcedure"),
            Arg.Is(CommandType.StoredProcedure),
            Arg.Is<int?>(v => v == null),
            Arg.Is<IEnumerable<SqlParameter>>(v => v.SingleOrDefault(i => i.ParameterName == "@parameter"
            && i.Value == null
            && i.Direction == ParameterDirection.Input) != null));
    }

    [TestMethod]
    public void WithParameterNullDbNull()
    {
        var connectionManagerMock = Substitute.For<IConnectionManager<SqlDataReader, SqlParameter>>();
        var command = connectionManagerMock.StoredProcedure("StoredProcedure");

        connectionManagerMock.Factory.Returns(SqlClientFactory.Instance);

        command.WithParameter("@parameter", null);
        command.ExecuteNonQuery();

        connectionManagerMock.Received().ExecuteNonQuery(Arg.Is("StoredProcedure"),
            Arg.Is(CommandType.StoredProcedure),
            Arg.Is<int?>(v => v == null),
            Arg.Is<IEnumerable<SqlParameter>>(v => v.SingleOrDefault(i => i.ParameterName == "@parameter"
            && i.Value == DBNull.Value
            && i.Direction == ParameterDirection.Input) != null));
    }

    [TestMethod]
    public void WithOutputParameter()
    {
        var connectionManagerMock = Substitute.For<IConnectionManager<SqlDataReader, SqlParameter>>();
        var command = connectionManagerMock.StoredProcedure("StoredProcedure");

        connectionManagerMock.Factory.Returns(SqlClientFactory.Instance);

        object? field = null;
        command.WithOutputParameter("@parameter", v => field = v);
        command.ExecuteNonQuery();

        connectionManagerMock.Received().ExecuteNonQuery(Arg.Is("StoredProcedure"),
            Arg.Is(CommandType.StoredProcedure),
            Arg.Is<int?>(v => v == null),
            Arg.Is<IEnumerable<SqlParameter>>(v => v.SingleOrDefault(i => i.ParameterName == "@parameter"
            && i.Value == DBNull.Value
            && i.Direction == ParameterDirection.Output) != null));
    }

    [TestMethod]
    public void WithOutputParameterInput()
    {
        var connectionManagerMock = Substitute.For<IConnectionManager<SqlDataReader, SqlParameter>>();
        var command = connectionManagerMock.StoredProcedure("StoredProcedure");

        connectionManagerMock.Factory.Returns(SqlClientFactory.Instance);

        object? field = null;
        command.WithOutputParameter("@parameter", v => field = v, DBNull.Value);
        command.ExecuteNonQuery();

        connectionManagerMock.Received().ExecuteNonQuery(Arg.Is("StoredProcedure"),
            Arg.Is(CommandType.StoredProcedure),
            Arg.Is<int?>(v => v == null),
            Arg.Is<IEnumerable<SqlParameter>>(v => v.SingleOrDefault(i => i.ParameterName == "@parameter"
            && i.Value == DBNull.Value
            && i.Direction == ParameterDirection.InputOutput) != null));
    }

    [TestMethod]
    public void WithReturn()
    {
        var connectionManagerMock = Substitute.For<IConnectionManager<SqlDataReader, SqlParameter>>();
        var command = connectionManagerMock.StoredProcedure("StoredProcedure");

        connectionManagerMock.Factory.Returns(SqlClientFactory.Instance);

        object? field = null;
        command.WithReturn(v => field = v);
        command.ExecuteNonQuery();

        connectionManagerMock.Received().ExecuteNonQuery(Arg.Is("StoredProcedure"),
            Arg.Is(CommandType.StoredProcedure),
            Arg.Is<int?>(v => v == null),
            Arg.Is<IEnumerable<SqlParameter>>(v => v.SingleOrDefault(i => i.ParameterName == command.ReturnParameterName
            && i.Value == DBNull.Value
            && i.Direction == ParameterDirection.ReturnValue) != null));
    }

    [TestMethod]
    public void WithParametersArray()
    {
        var connectionManagerMock = Substitute.For<IConnectionManager<SqlDataReader, SqlParameter>>();
        var command = connectionManagerMock.StoredProcedure("StoredProcedure");

        connectionManagerMock.Factory.Returns(SqlClientFactory.Instance);

        command.WithParameters(false, ("@parameter1", "value"), ("@parameter2", null));
        command.ExecuteNonQuery();

        connectionManagerMock.Received().ExecuteNonQuery(Arg.Is("StoredProcedure"),
            Arg.Is(CommandType.StoredProcedure),
            Arg.Is<int?>(v => v == null),
            Arg.Is<IEnumerable<SqlParameter>>(v => v.SingleOrDefault(i => i.ParameterName == "@parameter1"
            && i.Value.ToString() == "value"
            && i.Direction == ParameterDirection.Input) != null
            && v.SingleOrDefault(i => i.ParameterName == "@parameter2"
            && i.Value == null
            && i.Direction == ParameterDirection.Input) != null));
    }

    [TestMethod]
    public void WithParametersArrayNullDbNull()
    {
        var connectionManagerMock = Substitute.For<IConnectionManager<SqlDataReader, SqlParameter>>();
        var command = connectionManagerMock.StoredProcedure("StoredProcedure");

        connectionManagerMock.Factory.Returns(SqlClientFactory.Instance);

        command.WithParameters(("@parameter1", "value"), ("@parameter2", null));
        command.ExecuteNonQuery();

        connectionManagerMock.Received().ExecuteNonQuery(Arg.Is("StoredProcedure"),
            Arg.Is(CommandType.StoredProcedure),
            Arg.Is<int?>(v => v == null),
            Arg.Is<IEnumerable<SqlParameter>>(v => v.SingleOrDefault(i => i.ParameterName == "@parameter1"
            && i.Value.ToString() == "value"
            && i.Direction == ParameterDirection.Input) != null
            && v.SingleOrDefault(i => i.ParameterName == "@parameter2"
            && i.Value == DBNull.Value
            && i.Direction == ParameterDirection.Input) != null));
    }

    [TestMethod]
    public void WithParametersEnumerable()
    {
        var connectionManagerMock = Substitute.For<IConnectionManager<SqlDataReader, SqlParameter>>();
        var command = connectionManagerMock.StoredProcedure("StoredProcedure");

        connectionManagerMock.Factory.Returns(SqlClientFactory.Instance);

        command.WithParameters(new List<(string, object?)>() { ("@parameter1", "value"), ("@parameter2", null) });
        command.ExecuteNonQuery();

        connectionManagerMock.Received().ExecuteNonQuery(Arg.Is("StoredProcedure"),
            Arg.Is(CommandType.StoredProcedure),
            Arg.Is<int?>(v => v == null),
            Arg.Is<IEnumerable<SqlParameter>>(v => v.SingleOrDefault(i => i.ParameterName == "@parameter1"
            && i.Value.ToString() == "value"
            && i.Direction == ParameterDirection.Input) != null
            && v.SingleOrDefault(i => i.ParameterName == "@parameter2"
            && i.Value == DBNull.Value
            && i.Direction == ParameterDirection.Input) != null));
    }

    [TestMethod]
    public void WithConfiguration()
    {
        var connectionManagerMock = Substitute.For<IConnectionManager<SqlDataReader, SqlParameter>>();
        var command = connectionManagerMock.StoredProcedure("StoredProcedure");

        connectionManagerMock.Factory.Returns(SqlClientFactory.Instance);

        command.WithConfiguration((ds) => throw new TestException());
        Assert.ThrowsException<TestException>(() => command.GetDataSet());
    }

    [TestMethod]
    public void ExecuteNonQuery()
    {
        var connectionManagerMock = Substitute.For<IConnectionManager<SqlDataReader, SqlParameter>>();
        var command = connectionManagerMock.StoredProcedure("StoredProcedure");

        connectionManagerMock.Factory.Returns(SqlClientFactory.Instance);

        command.ExecuteNonQuery();

        connectionManagerMock.Received().ExecuteNonQuery(Arg.Is("StoredProcedure"),
            Arg.Is(CommandType.StoredProcedure),
            Arg.Is<int?>(v => v == null),
            Arg.Is<IEnumerable<SqlParameter>>(v => !v.Any()));
    }

    [TestMethod]
    public void ExecuteScalar()
    {
        var connectionManagerMock = Substitute.For<IConnectionManager<SqlDataReader, SqlParameter>>();
        var command = connectionManagerMock.StoredProcedure("StoredProcedure");

        connectionManagerMock.Factory.Returns(SqlClientFactory.Instance);

        command.ExecuteScalar<int>();

        connectionManagerMock.Received().ExecuteScalar<int>(Arg.Is("StoredProcedure"),
            Arg.Is(CommandType.StoredProcedure),
            Arg.Is<int?>(v => v == null),
            Arg.Is<IEnumerable<SqlParameter>>(v => !v.Any()));
    }

    [TestMethod]
    public void ExecuteReader()
    {
        var connectionManagerMock = Substitute.For<IConnectionManager<SqlDataReader, SqlParameter>>();
        var command = connectionManagerMock.StoredProcedure("StoredProcedure");

        connectionManagerMock.Factory.Returns(SqlClientFactory.Instance);

        command.ExecuteReader(CommandBehavior.SingleResult);

        connectionManagerMock.Received().ExecuteReader(Arg.Is("StoredProcedure"),
            Arg.Is(CommandType.StoredProcedure),
            Arg.Is<int?>(v => v == null),
            Arg.Is<CommandBehavior>(v => v == CommandBehavior.SingleResult),
            Arg.Is<IEnumerable<SqlParameter>>(v => !v.Any()));
    }

    [TestMethod]
    public void GetDataSet()
    {
        var connectionManagerMock = Substitute.For<IConnectionManager<SqlDataReader, SqlParameter>>();
        var command = connectionManagerMock.StoredProcedure("StoredProcedure");

        connectionManagerMock.Factory.Returns(SqlClientFactory.Instance);

        command.GetDataSet();

        connectionManagerMock.Received().GetDataSet(Arg.Is("StoredProcedure"),
            Arg.Is(CommandType.StoredProcedure),
            Arg.Is<int?>(v => v == null),
            Arg.Is<IEnumerable<SqlParameter>>(v => !v.Any()));
    }

    [TestMethod]
    public void GetDataTable()
    {
        var connectionManagerMock = Substitute.For<IConnectionManager<SqlDataReader, SqlParameter>>();
        var command = connectionManagerMock.StoredProcedure("StoredProcedure");
        var ds = new DataSet();
        var dt = new DataTable();

        ds.Tables.Add(dt);

        connectionManagerMock.Factory.Returns(SqlClientFactory.Instance);
        connectionManagerMock.GetDataTable(Arg.Is("StoredProcedure"),
            Arg.Is(CommandType.StoredProcedure),
            Arg.Is<int?>(v => v == null),
            Arg.Is<IEnumerable<SqlParameter>>(v => !v.Any()))
            .Returns(dt);

        command.GetDataTable();

        connectionManagerMock.Received().GetDataTable(Arg.Is("StoredProcedure"),
            Arg.Is(CommandType.StoredProcedure),
            Arg.Is<int?>(v => v == null),
            Arg.Is<IEnumerable<SqlParameter>>(v => !v.Any()));
    }

    [TestMethod]
    public void GetDataRow()
    {
        var connectionManagerMock = Substitute.For<IConnectionManager<SqlDataReader, SqlParameter>>();
        var command = connectionManagerMock.StoredProcedure("StoredProcedure");
        var ds = new DataSet();
        var dt = new DataTable();

        ds.Tables.Add(dt);
        dt.Columns.Add("col1");
        dt.Rows.Add("val1");

        connectionManagerMock.Factory.Returns(SqlClientFactory.Instance);
        connectionManagerMock.GetDataRow(Arg.Is("StoredProcedure"),
            Arg.Is(CommandType.StoredProcedure),
            Arg.Is<int?>(v => v == null),
            Arg.Is<IEnumerable<SqlParameter>>(v => !v.Any()))
            .Returns(dt.Rows[0]);

        command.GetDataRow();

        connectionManagerMock.Received().GetDataRow(Arg.Is("StoredProcedure"),
            Arg.Is(CommandType.StoredProcedure),
            Arg.Is<int?>(v => v == null),
            Arg.Is<IEnumerable<SqlParameter>>(v => !v.Any()));
    }

    [TestMethod]
    public async Task ExecuteNonQueryAsync()
    {
        var connectionManagerMock = Substitute.For<IConnectionManager<SqlDataReader, SqlParameter>>();
        var command = connectionManagerMock.StoredProcedure("StoredProcedure");

        connectionManagerMock.Factory.Returns(SqlClientFactory.Instance);

        await command.ExecuteNonQueryAsync();

        await connectionManagerMock.Received().ExecuteNonQueryAsync(Arg.Is("StoredProcedure"),
            Arg.Is(CommandType.StoredProcedure),
            Arg.Is<int?>(v => v == null),
            Arg.Is<IEnumerable<SqlParameter>>(v => !v.Any()));
    }

    [TestMethod]
    public async Task ExecuteScalarAsync()
    {
        var connectionManagerMock = Substitute.For<IConnectionManager<SqlDataReader, SqlParameter>>();
        var command = connectionManagerMock.StoredProcedure("StoredProcedure");

        connectionManagerMock.Factory.Returns(SqlClientFactory.Instance);

        await command.ExecuteScalarAsync<int>();

        await connectionManagerMock.Received().ExecuteScalarAsync<int>(Arg.Is("StoredProcedure"),
            Arg.Is(CommandType.StoredProcedure),
            Arg.Is<int?>(v => v == null),
            Arg.Is<IEnumerable<SqlParameter>>(v => !v.Any()));
    }

    [TestMethod]
    public async Task ExecuteReaderAsync()
    {
        var connectionManagerMock = Substitute.For<IConnectionManager<SqlDataReader, SqlParameter>>();
        var command = connectionManagerMock.StoredProcedure("StoredProcedure");

        connectionManagerMock.Factory.Returns(SqlClientFactory.Instance);

        await command.ExecuteReaderAsync(CommandBehavior.SingleResult);

        await connectionManagerMock.Received().ExecuteReaderAsync(Arg.Is("StoredProcedure"),
            Arg.Is(CommandType.StoredProcedure),
            Arg.Is<int?>(v => v == null),
            Arg.Is<CommandBehavior>(v => v == CommandBehavior.SingleResult),
            Arg.Is<IEnumerable<SqlParameter>>(v => !v.Any()));
    }

    [TestMethod]
    public async Task GetDataSetAsync()
    {
        var connectionManagerMock = Substitute.For<IConnectionManager<SqlDataReader, SqlParameter>>();
        var command = connectionManagerMock.StoredProcedure("StoredProcedure");

        connectionManagerMock.Factory.Returns(SqlClientFactory.Instance);

        await command.GetDataSetAsync();

        await connectionManagerMock.Received().GetDataSetAsync(Arg.Is("StoredProcedure"),
            Arg.Is(CommandType.StoredProcedure),
            Arg.Is<int?>(v => v == null),
            Arg.Is<IEnumerable<SqlParameter>>(v => !v.Any()));
    }

    [TestMethod]
    public async Task GetDataTableAsync()
    {
        var connectionManagerMock = Substitute.For<IConnectionManager<SqlDataReader, SqlParameter>>();
        var command = connectionManagerMock.StoredProcedure("StoredProcedure");
        var ds = new DataSet();
        var dt = new DataTable();

        ds.Tables.Add(dt);

        connectionManagerMock.Factory.Returns(SqlClientFactory.Instance);
        connectionManagerMock.GetDataTableAsync(Arg.Is("StoredProcedure"),
            Arg.Is(CommandType.StoredProcedure),
            Arg.Is<int?>(v => v == null),
            Arg.Is<IEnumerable<SqlParameter>>(v => !v.Any()))
            .Returns(Task.FromResult(dt));

        await command.GetDataTableAsync();

        await connectionManagerMock.Received().GetDataTableAsync(Arg.Is("StoredProcedure"),
            Arg.Is(CommandType.StoredProcedure),
            Arg.Is<int?>(v => v == null),
            Arg.Is<IEnumerable<SqlParameter>>(v => !v.Any()));
    }

    [TestMethod]
    public async Task GetDataRowAsync()
    {
        var connectionManagerMock = Substitute.For<IConnectionManager<SqlDataReader, SqlParameter>>();
        var command = connectionManagerMock.StoredProcedure("StoredProcedure");
        var ds = new DataSet();
        var dt = new DataTable();

        ds.Tables.Add(dt);
        dt.Columns.Add("col1");
        dt.Rows.Add("val1");

        connectionManagerMock.Factory.Returns(SqlClientFactory.Instance);
        connectionManagerMock.GetDataRowAsync(Arg.Is("StoredProcedure"),
            Arg.Is(CommandType.StoredProcedure),
            Arg.Is<int?>(v => v == null),
            Arg.Is<IEnumerable<SqlParameter>>(v => !v.Any()))
            .Returns(Task.FromResult((DataRow?)dt.Rows[0]));

        await command.GetDataRowAsync();

        await connectionManagerMock.Received().GetDataRowAsync(Arg.Is("StoredProcedure"),
            Arg.Is(CommandType.StoredProcedure),
            Arg.Is<int?>(v => v == null),
            Arg.Is<IEnumerable<SqlParameter>>(v => !v.Any()));
    }

    [TestMethod]
    public void ReturnParameterValue()
    {
        var connectionManagerMock = Substitute.For<IConnectionManager<SqlDataReader, SqlParameter>>();
        var command = connectionManagerMock.StoredProcedure("StoredProcedure");

        connectionManagerMock.Factory.Returns(SqlClientFactory.Instance);
        connectionManagerMock.ExecuteScalar<int>(Arg.Is("StoredProcedure"),
            Arg.Is(CommandType.StoredProcedure),
            Arg.Is<int?>(v => v == null),
            Arg.Is<IEnumerable<SqlParameter>>(v => v.SingleOrDefault(i => i.ParameterName == command.ReturnParameterName
            && i.Value == DBNull.Value
            && i.Direction == ParameterDirection.ReturnValue) != null))
            .Returns(1)
            .AndDoes(x => ((IEnumerable<SqlParameter>)x[3]).First().Value = 30);

        object? field = null;
        command.WithReturn(v => field = v);
        command.ExecuteScalar<int>();

        Assert.AreEqual(30, field);
    }

    [TestMethod]
    public void OutputParameterValue()
    {
        var connectionManagerMock = Substitute.For<IConnectionManager<SqlDataReader, SqlParameter>>();
        var command = connectionManagerMock.StoredProcedure("StoredProcedure");

        connectionManagerMock.Factory.Returns(SqlClientFactory.Instance);
        connectionManagerMock.ExecuteScalar<int>(Arg.Is("StoredProcedure"),
            Arg.Is(CommandType.StoredProcedure),
            Arg.Is<int?>(v => v == null),
            Arg.Is<IEnumerable<SqlParameter>>(v => v.SingleOrDefault(i => i.ParameterName == "@parameter"
            && i.Value == DBNull.Value
            && i.Direction == ParameterDirection.Output) != null))
            .Returns(1)
            .AndDoes(x => ((IEnumerable<SqlParameter>)x[3]).First().Value = 30);

        object? field = null;
        command.WithOutputParameter("@parameter", v => field = v);
        command.ExecuteScalar<int>();

        Assert.AreEqual(30, field);
    }

    [TestMethod]
    public void InputOutputParameterValue()
    {
        var connectionManagerMock = Substitute.For<IConnectionManager<SqlDataReader, SqlParameter>>();
        var command = connectionManagerMock.StoredProcedure("StoredProcedure");

        connectionManagerMock.Factory.Returns(SqlClientFactory.Instance);
        connectionManagerMock.ExecuteScalar<int>(Arg.Is("StoredProcedure"),
            Arg.Is(CommandType.StoredProcedure),
            Arg.Is<int?>(v => v == null),
            Arg.Is<IEnumerable<SqlParameter>>(v => v.SingleOrDefault(i => i.ParameterName == "@parameter"
            && (int)i.Value == 10
            && i.Direction == ParameterDirection.InputOutput) != null))
            .Returns(1)
            .AndDoes(x => ((IEnumerable<SqlParameter>)x[3]).First().Value = 30);

        object? field = null;
        command.WithOutputParameter("@parameter", v => field = v, 10);
        command.ExecuteScalar<int>();

        Assert.AreEqual(30, field);
    }
}