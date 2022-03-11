using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PiraSoft.Tools.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PiraSoft.Tools.UnitTest.Data.Extensions;

[TestClass]
public class IConnectionManagerExtensionsUnitTest
{
    [TestMethod]
    public void StoredProcedure()
    {
        var connectionManagerMock = Substitute.For<IConnectionManager<SqlDataReader, SqlParameter>>();
        var command = connectionManagerMock.StoredProcedure("");

        Assert.AreEqual(CommandType.StoredProcedure, command.GetNotPublicFieldValue<CommandType>("_commandType"));
    }

    [TestMethod]
    public void Query()
    {
        var connectionManagerMock = Substitute.For<IConnectionManager<SqlDataReader, SqlParameter>>();
        var command = connectionManagerMock.Query("");

        Assert.AreEqual(CommandType.Text, command.GetNotPublicFieldValue<CommandType>("_commandType"));
    }

    [TestMethod]
    public void ExecuteNonQuery()
    {
        var connectionManagerMock = Substitute.For<IConnectionManager<SqlDataReader, SqlParameter>>();

        connectionManagerMock.ExecuteNonQuery("", new SqlParameter());
        connectionManagerMock.Received().ExecuteNonQuery(Arg.Any<string>(), Arg.Is(CommandType.StoredProcedure), Arg.Is<int?>(v => v == null), Arg.Is<IEnumerable<SqlParameter>>(v => v.Count() == 1));

        connectionManagerMock.ExecuteNonQuery("", CommandType.Text, 30, new SqlParameter());
        connectionManagerMock.Received().ExecuteNonQuery(Arg.Any<string>(), Arg.Is(CommandType.Text), Arg.Is<int?>(v => v == 30), Arg.Is<IEnumerable<SqlParameter>>(v => v.Count() == 1));
    }

    [TestMethod]
    public async Task ExecuteNonQueryAsync()
    {
        var connectionManagerMock = Substitute.For<IConnectionManager<SqlDataReader, SqlParameter>>();

        await connectionManagerMock.ExecuteNonQueryAsync("", new SqlParameter());
        await connectionManagerMock.Received().ExecuteNonQueryAsync(Arg.Any<string>(), Arg.Is(CommandType.StoredProcedure), Arg.Is<int?>(v => v == null), Arg.Is<IEnumerable<SqlParameter>>(v => v.Count() == 1), Arg.Is<CancellationToken>(v => v == default));

        await connectionManagerMock.ExecuteNonQueryAsync("", CommandType.Text, 30, default, new SqlParameter());
        await connectionManagerMock.Received().ExecuteNonQueryAsync(Arg.Any<string>(), Arg.Is(CommandType.Text), Arg.Is<int?>(v => v == 30), Arg.Is<IEnumerable<SqlParameter>>(v => v.Count() == 1), Arg.Is<CancellationToken>(v => v == default));
    }

    [TestMethod]
    public void ExecuteScalar()
    {
        var connectionManagerMock = Substitute.For<IConnectionManager<SqlDataReader, SqlParameter>>();

        connectionManagerMock.ExecuteScalar<int, SqlDataReader, SqlParameter>("", new SqlParameter());
        connectionManagerMock.Received().ExecuteScalar<int>(Arg.Any<string>(), Arg.Is(CommandType.StoredProcedure), Arg.Is<int?>(v => v == null), Arg.Is<IEnumerable<SqlParameter>>(v => v.Count() == 1));

        connectionManagerMock.ExecuteScalar<int, SqlDataReader, SqlParameter>("", CommandType.Text, 30, new SqlParameter());
        connectionManagerMock.Received().ExecuteScalar<int>(Arg.Any<string>(), Arg.Is(CommandType.Text), Arg.Is<int?>(v => v == 30), Arg.Is<IEnumerable<SqlParameter>>(v => v.Count() == 1));
    }

    [TestMethod]
    public async Task ExecuteScalarAsync()
    {
        var connectionManagerMock = Substitute.For<IConnectionManager<SqlDataReader, SqlParameter>>();

        await connectionManagerMock.ExecuteScalarAsync<int, SqlDataReader, SqlParameter>("", new SqlParameter());
        await connectionManagerMock.Received().ExecuteScalarAsync<int>(Arg.Any<string>(), Arg.Is(CommandType.StoredProcedure), Arg.Is<int?>(v => v == null), Arg.Is<IEnumerable<SqlParameter>>(v => v.Count() == 1), Arg.Is<CancellationToken>(v => v == default));

        await connectionManagerMock.ExecuteScalarAsync<int, SqlDataReader, SqlParameter>("", CommandType.Text, 30, default, new SqlParameter());
        await connectionManagerMock.Received().ExecuteScalarAsync<int>(Arg.Any<string>(), Arg.Is(CommandType.Text), Arg.Is<int?>(v => v == 30), Arg.Is<IEnumerable<SqlParameter>>(v => v.Count() == 1), Arg.Is<CancellationToken>(v => v == default));
    }

    [TestMethod]
    public void ExecuteReader()
    {
        var connectionManagerMock = Substitute.For<IConnectionManager<SqlDataReader, SqlParameter>>();

        connectionManagerMock.ExecuteReader("", new SqlParameter());
        connectionManagerMock.Received().ExecuteReader(Arg.Any<string>(), Arg.Is(CommandType.StoredProcedure), Arg.Is<int?>(i => i == null), Arg.Is<CommandBehavior>(i => i == CommandBehavior.Default), Arg.Is<IEnumerable<SqlParameter>>(v => v.Count() == 1));

        connectionManagerMock.ExecuteReader("", CommandType.Text, 30, CommandBehavior.SingleResult, new SqlParameter());
        connectionManagerMock.Received().ExecuteReader(Arg.Any<string>(), Arg.Is(CommandType.Text), Arg.Is<int?>(i => i == 30), Arg.Is<CommandBehavior>(i => i == CommandBehavior.SingleResult), Arg.Is<IEnumerable<SqlParameter>>(v => v.Count() == 1));
    }

    [TestMethod]
    public async Task ExecuteReaderAsync()
    {
        var connectionManagerMock = Substitute.For<IConnectionManager<SqlDataReader, SqlParameter>>();

        await connectionManagerMock.ExecuteReaderAsync("", new SqlParameter());
        await connectionManagerMock.Received().ExecuteReaderAsync(Arg.Any<string>(), Arg.Is(CommandType.StoredProcedure), Arg.Is<int?>(i => i == null), Arg.Is<CommandBehavior>(i => i == CommandBehavior.Default), Arg.Is<IEnumerable<SqlParameter>>(v => v.Count() == 1), Arg.Is<CancellationToken>(v => v == default));

        await connectionManagerMock.ExecuteReaderAsync("", CommandType.Text, 30, CommandBehavior.SingleResult, default, new SqlParameter());
        await connectionManagerMock.Received().ExecuteReaderAsync(Arg.Any<string>(), Arg.Is(CommandType.Text), Arg.Is<int?>(i => i == 30), Arg.Is<CommandBehavior>(i => i == CommandBehavior.SingleResult), Arg.Is<IEnumerable<SqlParameter>>(v => v.Count() == 1), Arg.Is<CancellationToken>(v => v == default));
    }

    [TestMethod]
    public void GetDataSet()
    {
        var connectionManagerMock = Substitute.For<IConnectionManager<SqlDataReader, SqlParameter>>();

        connectionManagerMock.GetDataSet("", new SqlParameter());
        connectionManagerMock.Received().GetDataSet(Arg.Any<string>(), Arg.Is(CommandType.StoredProcedure), Arg.Is<int?>(i => i == null), Arg.Is<IEnumerable<SqlParameter>>(v => v.Count() == 1));

        connectionManagerMock.GetDataSet("", CommandType.Text, 30, new SqlParameter());
        connectionManagerMock.Received().GetDataSet(Arg.Any<string>(), Arg.Is(CommandType.Text), Arg.Is<int?>(i => i == 30), Arg.Is<IEnumerable<SqlParameter>>(v => v.Count() == 1));
    }

    [TestMethod]
    public async Task GetDataSetAsync()
    {
        var connectionManagerMock = Substitute.For<IConnectionManager<SqlDataReader, SqlParameter>>();

        await connectionManagerMock.GetDataSetAsync("", new SqlParameter());
        await connectionManagerMock.Received().GetDataSetAsync(Arg.Any<string>(), Arg.Is(CommandType.StoredProcedure), Arg.Is<int?>(i => i == null), Arg.Is<IEnumerable<SqlParameter>>(v => v.Count() == 1), Arg.Is<CancellationToken>(v => v == default));

        await connectionManagerMock.GetDataSetAsync("", CommandType.Text, 30, default, new SqlParameter());
        await connectionManagerMock.Received().GetDataSetAsync(Arg.Any<string>(), Arg.Is(CommandType.Text), Arg.Is<int?>(i => i == 30), Arg.Is<IEnumerable<SqlParameter>>(v => v.Count() == 1), Arg.Is<CancellationToken>(v => v == default));
    }

    [TestMethod]
    public void GetDataTable()
    {
        var connectionManagerMock = Substitute.For<IConnectionManager<SqlDataReader, SqlParameter>>();

        connectionManagerMock.GetDataTable("", new SqlParameter());
        connectionManagerMock.Received().GetDataTable(Arg.Any<string>(), Arg.Is(CommandType.StoredProcedure), Arg.Is<int?>(i => i == null), Arg.Is<IEnumerable<SqlParameter>>(v => v.Count() == 1));

        connectionManagerMock.GetDataTable("", CommandType.Text, 30, new SqlParameter());
        connectionManagerMock.Received().GetDataTable(Arg.Any<string>(), Arg.Is(CommandType.Text), Arg.Is<int?>(i => i == 30), Arg.Is<IEnumerable<SqlParameter>>(v => v.Count() == 1));
    }

    [TestMethod]
    public async Task GetDataTableAsync()
    {
        var connectionManagerMock = Substitute.For<IConnectionManager<SqlDataReader, SqlParameter>>();

        await connectionManagerMock.GetDataTableAsync("", new SqlParameter());
        await connectionManagerMock.Received().GetDataTableAsync(Arg.Any<string>(), Arg.Is(CommandType.StoredProcedure), Arg.Is<int?>(i => i == null), Arg.Is<IEnumerable<SqlParameter>>(v => v.Count() == 1), Arg.Is<CancellationToken>(v => v == default));

        await connectionManagerMock.GetDataTableAsync("", CommandType.Text, 30, default, new SqlParameter());
        await connectionManagerMock.Received().GetDataTableAsync(Arg.Any<string>(), Arg.Is(CommandType.Text), Arg.Is<int?>(i => i == 30), Arg.Is<IEnumerable<SqlParameter>>(v => v.Count() == 1), Arg.Is<CancellationToken>(v => v == default));
    }

    [TestMethod]
    public void GetDataRow()
    {
        var connectionManagerMock = Substitute.For<IConnectionManager<SqlDataReader, SqlParameter>>();

        connectionManagerMock.GetDataRow("", new SqlParameter());
        connectionManagerMock.Received().GetDataRow(Arg.Any<string>(), Arg.Is(CommandType.StoredProcedure), Arg.Is<int?>(i => i == null), Arg.Is<IEnumerable<SqlParameter>>(v => v.Count() == 1));

        connectionManagerMock.GetDataRow("", CommandType.Text, 30, new SqlParameter());
        connectionManagerMock.Received().GetDataRow(Arg.Any<string>(), Arg.Is(CommandType.Text), Arg.Is<int?>(i => i == 30), Arg.Is<IEnumerable<SqlParameter>>(v => v.Count() == 1));
    }

    [TestMethod]
    public async Task GetDataRowAsync()
    {
        var connectionManagerMock = Substitute.For<IConnectionManager<SqlDataReader, SqlParameter>>();

        await connectionManagerMock.GetDataRowAsync("", new SqlParameter());
        await connectionManagerMock.Received().GetDataRowAsync(Arg.Any<string>(), Arg.Is(CommandType.StoredProcedure), Arg.Is<int?>(i => i == null), Arg.Is<IEnumerable<SqlParameter>>(v => v.Count() == 1), Arg.Is<CancellationToken>(v => v == default));

        await connectionManagerMock.GetDataRowAsync("", CommandType.Text, 30, default, new SqlParameter());
        await connectionManagerMock.Received().GetDataRowAsync(Arg.Any<string>(), Arg.Is(CommandType.Text), Arg.Is<int?>(i => i == 30), Arg.Is<IEnumerable<SqlParameter>>(v => v.Count() == 1), Arg.Is<CancellationToken>(v => v == default));
    }
}