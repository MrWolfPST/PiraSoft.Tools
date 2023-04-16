using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PiraSoft.Tools.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace PiraSoft.Tools.UnitTest.Data;

[TestClass]
public class ConnectionManagerImplementationUnitTest
{
    internal class FakeConnectionManager : ConnectionManagerImplementation<DbConnection, DbDataReader, DbDataAdapter, DbCommand, DbParameter>
    {
        public FakeConnectionManager(ILogger? logger, DbProviderFactory? factory)
            : base(logger, factory)
        { }

        protected internal override DbConnection GetConnection()
            => Substitute.For<DbConnection>();
    }

    internal class FakeParameterCollection : DbParameterCollection
    {
        private readonly IList _list = new List<object>();

        public override int Count => _list.Count;

        public override object SyncRoot => this;

        public override int Add(object value)
            => _list.Add(value);

        public override void AddRange(Array values)
            => throw new NotImplementedException();

        public override void Clear() => throw new NotImplementedException();

        public override bool Contains(object value)
            => throw new NotImplementedException();

        public override bool Contains(string value)
            => throw new NotImplementedException();

        public override void CopyTo(Array array, int index)
            => throw new NotImplementedException();

        public override IEnumerator GetEnumerator()
            => _list.GetEnumerator();

        public override int IndexOf(object value)
            => throw new NotImplementedException();

        public override int IndexOf(string parameterName)
            => throw new NotImplementedException();

        public override void Insert(int index, object value)
            => throw new NotImplementedException();

        public override void Remove(object value)
            => throw new NotImplementedException();

        public override void RemoveAt(int index)
            => throw new NotImplementedException();

        public override void RemoveAt(string parameterName)
            => throw new NotImplementedException();

        protected override DbParameter GetParameter(int index)
            => throw new NotImplementedException();

        protected override DbParameter GetParameter(string parameterName)
            => throw new NotImplementedException();

        protected override void SetParameter(int index, DbParameter value)
            => throw new NotImplementedException();

        protected override void SetParameter(string parameterName, DbParameter value)
            => throw new NotImplementedException();
    }

    [TestMethod]
    public void Ctor()
    {
        var loggerMock = Substitute.For<ILogger>();

        Assert.ThrowsException<ArgumentNullException>(() => new FakeConnectionManager(null, null));
        Assert.ThrowsException<ArgumentNullException>(() => new FakeConnectionManager(loggerMock, null));
    }

    [TestMethod]
    public void ExecuteNonQuery()
    {
        var loggerMock = Substitute.For<ILogger>();
        var factoryMock = Substitute.For<DbProviderFactory>();
        var connectionManager = new FakeConnectionManager(loggerMock, factoryMock);
        var commandMock = Substitute.For<DbCommand>();

        factoryMock.CreateCommand().Returns(commandMock);
#pragma warning disable NS1000 // Non-virtual setup specification.
        commandMock.Parameters.Returns(new FakeParameterCollection());
#pragma warning restore NS1000 // Non-virtual setup specification.

        connectionManager.ExecuteNonQuery("StoredProcedure", CommandType.StoredProcedure, 30, new List<SqlParameter>() { new SqlParameter("@parameter", "value") });

        Assert.AreEqual(CommandType.StoredProcedure, commandMock.CommandType);
        Assert.AreEqual(30, commandMock.CommandTimeout);
        Assert.AreEqual(1, commandMock.Parameters.Count);
        commandMock.Received().ExecuteNonQuery();
    }

    [TestMethod]
    public void ExecuteScalar()
    {
        var loggerMock = Substitute.For<ILogger>();
        var factoryMock = Substitute.For<DbProviderFactory>();
        var connectionManager = new FakeConnectionManager(loggerMock, factoryMock);
        var commandMock = Substitute.For<DbCommand>();

        factoryMock.CreateCommand().Returns(commandMock);
#pragma warning disable NS1000 // Non-virtual setup specification.
        commandMock.Parameters.Returns(new FakeParameterCollection());
#pragma warning restore NS1000 // Non-virtual setup specification.
        commandMock.ExecuteScalar().Returns(0);

        connectionManager.ExecuteScalar<int>("StoredProcedure", CommandType.StoredProcedure, 30, new List<SqlParameter>() { new SqlParameter("@parameter", "value") });

        Assert.AreEqual(CommandType.StoredProcedure, commandMock.CommandType);
        Assert.AreEqual(30, commandMock.CommandTimeout);
        Assert.AreEqual(1, commandMock.Parameters.Count);
        commandMock.Received().ExecuteScalar();
    }

    [TestMethod]
    public void ExecuteReader()
    {
        var loggerMock = Substitute.For<ILogger>();
        var factoryMock = Substitute.For<DbProviderFactory>();
        var connectionManager = new FakeConnectionManager(loggerMock, factoryMock);
        var commandMock = Substitute.For<DbCommand>();

        factoryMock.CreateCommand().Returns(commandMock);
#pragma warning disable NS1000 // Non-virtual setup specification.
        commandMock.Parameters.Returns(new FakeParameterCollection());
#pragma warning restore NS1000 // Non-virtual setup specification.

        connectionManager.ExecuteReader("StoredProcedure", CommandType.StoredProcedure, 30, CommandBehavior.SingleRow, new List<SqlParameter>() { new SqlParameter("@parameter", "value") });

        Assert.AreEqual(CommandType.StoredProcedure, commandMock.CommandType);
        Assert.AreEqual(30, commandMock.CommandTimeout);
        Assert.AreEqual(1, commandMock.Parameters.Count);
#pragma warning disable NS1001 // Non-virtual setup specification.
#pragma warning disable NS1004 // Argument matcher used with a non-virtual member of a class.
        commandMock.Received().ExecuteReader(Arg.Any<CommandBehavior>());
#pragma warning restore NS1004 // Argument matcher used with a non-virtual member of a class.
#pragma warning restore NS1001 // Non-virtual setup specification.
    }

    [TestMethod]
    public void GetDataSet()
    {
        var loggerMock = Substitute.For<ILogger>();
        var factoryMock = Substitute.For<DbProviderFactory>();
        var connectionManager = new FakeConnectionManager(loggerMock, factoryMock);
        var commandMock = Substitute.For<DbCommand>();
        var dataAdapterMock = Substitute.For<DbDataAdapter>();

        factoryMock.CreateCommand().Returns(commandMock);
        factoryMock.CreateDataAdapter().Returns(dataAdapterMock);
#pragma warning disable NS1000 // Non-virtual setup specification.
        commandMock.Parameters.Returns(new FakeParameterCollection());
#pragma warning restore NS1000 // Non-virtual setup specification.

        var ds = connectionManager.GetDataSet("StoredProcedure", CommandType.StoredProcedure, 30, new List<SqlParameter>() { new SqlParameter("@parameter", "value") });

        Assert.IsNotNull(ds);
        Assert.AreEqual(CommandType.StoredProcedure, commandMock.CommandType);
        Assert.AreEqual(30, commandMock.CommandTimeout);
        Assert.AreEqual(1, commandMock.Parameters.Count);
        dataAdapterMock.Received().Fill(Arg.Any<DataSet>());
    }

    [TestMethod]
    public void GetDataTable()
    {
        var loggerMock = Substitute.For<ILogger>();
        var factoryMock = Substitute.For<DbProviderFactory>();
        var connectionManager = new FakeConnectionManager(loggerMock, factoryMock);
        var commandMock = Substitute.For<DbCommand>();
        var dataAdapterMock = Substitute.For<DbDataAdapter>();

        factoryMock.CreateCommand().Returns(commandMock);
        factoryMock.CreateDataAdapter().Returns(dataAdapterMock);
#pragma warning disable NS1000 // Non-virtual setup specification.
        commandMock.Parameters.Returns(new FakeParameterCollection());
#pragma warning disable NS1004 // Argument matcher used with a non-virtual member of a class.
        commandMock.ExecuteReader(Arg.Any<CommandBehavior>()).Returns(Substitute.For<DbDataReader>());
#pragma warning restore NS1004 // Argument matcher used with a non-virtual member of a class.
#pragma warning restore NS1000 // Non-virtual setup specification.

        var dt = connectionManager.GetDataTable("StoredProcedure", CommandType.StoredProcedure, 30, new List<SqlParameter>() { new SqlParameter("@parameter", "value") });

        Assert.IsNotNull(dt);
        Assert.AreEqual(CommandType.StoredProcedure, commandMock.CommandType);
        Assert.AreEqual(30, commandMock.CommandTimeout);
        Assert.AreEqual(1, commandMock.Parameters.Count);
        dataAdapterMock.Received().Fill(Arg.Any<DataSet>());
    }

    [TestMethod]
    public void GetDataRow()
    {
        var loggerMock = Substitute.For<ILogger>();
        var factoryMock = Substitute.For<DbProviderFactory>();
        var connectionManager = new FakeConnectionManager(loggerMock, factoryMock);
        var commandMock = Substitute.For<DbCommand>();
        var dataAdapterMock = Substitute.For<DbDataAdapter>();

        factoryMock.CreateCommand().Returns(commandMock);
        factoryMock.CreateDataAdapter().Returns(dataAdapterMock);
#pragma warning disable NS1000 // Non-virtual setup specification.
        commandMock.Parameters.Returns(new FakeParameterCollection());
#pragma warning disable NS1004 // Argument matcher used with a non-virtual member of a class.
        commandMock.ExecuteReader(Arg.Any<CommandBehavior>()).Returns(Substitute.For<DbDataReader>());
#pragma warning restore NS1004 // Argument matcher used with a non-virtual member of a class.
#pragma warning restore NS1000 // Non-virtual setup specification.

        var dr = connectionManager.GetDataRow("StoredProcedure", CommandType.StoredProcedure, 30, new List<SqlParameter>() { new SqlParameter("@parameter", "value") });

        Assert.IsNull(dr);
        Assert.AreEqual(CommandType.StoredProcedure, commandMock.CommandType);
        Assert.AreEqual(30, commandMock.CommandTimeout);
        Assert.AreEqual(1, commandMock.Parameters.Count);
        dataAdapterMock.Received().Fill(Arg.Any<DataSet>());
    }

    [TestMethod]
    public async Task ExecuteNonQueryAsync()
    {
        var loggerMock = Substitute.For<ILogger>();
        var factoryMock = Substitute.For<DbProviderFactory>();
        var connectionManager = new FakeConnectionManager(loggerMock, factoryMock);
        var commandMock = Substitute.For<DbCommand>();

        factoryMock.CreateCommand().Returns(commandMock);
#pragma warning disable NS1000 // Non-virtual setup specification.
        commandMock.Parameters.Returns(new FakeParameterCollection());
#pragma warning restore NS1000 // Non-virtual setup specification.

        await connectionManager.ExecuteNonQueryAsync("StoredProcedure", CommandType.StoredProcedure, 30, new List<SqlParameter>() { new SqlParameter("@parameter", "value") });

        Assert.AreEqual(CommandType.StoredProcedure, commandMock.CommandType);
        Assert.AreEqual(30, commandMock.CommandTimeout);
        Assert.AreEqual(1, commandMock.Parameters.Count);
#pragma warning disable NS1001 // Non-virtual setup specification.
        await commandMock.Received().ExecuteNonQueryAsync();
#pragma warning restore NS1001 // Non-virtual setup specification.
    }

    [TestMethod]
    public async Task ExecuteScalarAsync()
    {
        var loggerMock = Substitute.For<ILogger>();
        var factoryMock = Substitute.For<DbProviderFactory>();
        var connectionManager = new FakeConnectionManager(loggerMock, factoryMock);
        var commandMock = Substitute.For<DbCommand>();

        factoryMock.CreateCommand().Returns(commandMock);
#pragma warning disable NS1000 // Non-virtual setup specification.
        commandMock.Parameters.Returns(new FakeParameterCollection());
        commandMock.ExecuteScalarAsync().Returns(0);
#pragma warning restore NS1000 // Non-virtual setup specification.

        await connectionManager.ExecuteScalarAsync<int>("StoredProcedure", CommandType.StoredProcedure, 30, new List<SqlParameter>() { new SqlParameter("@parameter", "value") });

        Assert.AreEqual(CommandType.StoredProcedure, commandMock.CommandType);
        Assert.AreEqual(30, commandMock.CommandTimeout);
        Assert.AreEqual(1, commandMock.Parameters.Count);
#pragma warning disable NS1001 // Non-virtual setup specification.
        await commandMock.Received().ExecuteScalarAsync();
#pragma warning restore NS1001 // Non-virtual setup specification.
    }

    [TestMethod]
    public async Task ExecuteReaderAsync()
    {
        var loggerMock = Substitute.For<ILogger>();
        var factoryMock = Substitute.For<DbProviderFactory>();
        var connectionManager = new FakeConnectionManager(loggerMock, factoryMock);
        var commandMock = Substitute.For<DbCommand>();

        factoryMock.CreateCommand().Returns(commandMock);
#pragma warning disable NS1000 // Non-virtual setup specification.
        commandMock.Parameters.Returns(new FakeParameterCollection());
#pragma warning restore NS1000 // Non-virtual setup specification.

        await connectionManager.ExecuteReaderAsync("StoredProcedure", CommandType.StoredProcedure, 30, CommandBehavior.SingleRow, new List<SqlParameter>() { new SqlParameter("@parameter", "value") });

        Assert.AreEqual(CommandType.StoredProcedure, commandMock.CommandType);
        Assert.AreEqual(30, commandMock.CommandTimeout);
        Assert.AreEqual(1, commandMock.Parameters.Count);
#pragma warning disable NS1001 // Non-virtual setup specification.
#pragma warning disable NS1004 // Argument matcher used with a non-virtual member of a class.
        await commandMock.Received().ExecuteReaderAsync(Arg.Is(CommandBehavior.SingleRow), Arg.Any<CancellationToken>());
#pragma warning restore NS1004 // Argument matcher used with a non-virtual member of a class.
#pragma warning restore NS1001 // Non-virtual setup specification.
    }

    [TestMethod]
    public async Task GetDataSetAsync()
    {
        var loggerMock = Substitute.For<ILogger>();
        var factoryMock = Substitute.For<DbProviderFactory>();
        var connectionManager = new FakeConnectionManager(loggerMock, factoryMock);
        var commandMock = Substitute.For<DbCommand>();
        var dataAdapterMock = Substitute.For<DbDataAdapter>();

        factoryMock.CreateCommand().Returns(commandMock);
        factoryMock.CreateDataAdapter().Returns(dataAdapterMock);
#pragma warning disable NS1000 // Non-virtual setup specification.
        commandMock.Parameters.Returns(new FakeParameterCollection());
        commandMock.ExecuteReaderAsync().Returns(Substitute.For<DbDataReader>());
#pragma warning restore NS1000 // Non-virtual setup specification.

        var ds = await connectionManager.GetDataSetAsync("StoredProcedure", CommandType.StoredProcedure, 30, new List<SqlParameter>() { new SqlParameter("@parameter", "value") });

        Assert.IsNotNull(ds);
        Assert.AreEqual(CommandType.StoredProcedure, commandMock.CommandType);
        Assert.AreEqual(30, commandMock.CommandTimeout);
        Assert.AreEqual(1, commandMock.Parameters.Count);
#pragma warning disable NS1001 // Non-virtual setup specification.
        await commandMock.Received().ExecuteReaderAsync();
#pragma warning restore NS1001 // Non-virtual setup specification.
    }

    [TestMethod]
    public async Task GetDataTableAsync()
    {
        var loggerMock = Substitute.For<ILogger>();
        var factoryMock = Substitute.For<DbProviderFactory>();
        var connectionManager = new FakeConnectionManager(loggerMock, factoryMock);
        var commandMock = Substitute.For<DbCommand>();
        var dataAdapterMock = Substitute.For<DbDataAdapter>();

        factoryMock.CreateCommand().Returns(commandMock);
        factoryMock.CreateDataAdapter().Returns(dataAdapterMock);
#pragma warning disable NS1000 // Non-virtual setup specification.
        commandMock.Parameters.Returns(new FakeParameterCollection());
        commandMock.ExecuteReaderAsync().Returns(Substitute.For<DbDataReader>());
#pragma warning restore NS1000 // Non-virtual setup specification.

        var dt = await connectionManager.GetDataTableAsync("StoredProcedure", CommandType.StoredProcedure, 30, new List<SqlParameter>() { new SqlParameter("@parameter", "value") });

        Assert.IsNotNull(dt);
        Assert.AreEqual(CommandType.StoredProcedure, commandMock.CommandType);
        Assert.AreEqual(30, commandMock.CommandTimeout);
        Assert.AreEqual(1, commandMock.Parameters.Count);
#pragma warning disable NS1001 // Non-virtual setup specification.
        await commandMock.Received().ExecuteReaderAsync();
#pragma warning restore NS1001 // Non-virtual setup specification.
    }

    [TestMethod]
    public async Task GetDataRowAsync()
    {
        var loggerMock = Substitute.For<ILogger>();
        var factoryMock = Substitute.For<DbProviderFactory>();
        var connectionManager = new FakeConnectionManager(loggerMock, factoryMock);
        var commandMock = Substitute.For<DbCommand>();
        var dataAdapterMock = Substitute.For<DbDataAdapter>();

        factoryMock.CreateCommand().Returns(commandMock);
        factoryMock.CreateDataAdapter().Returns(dataAdapterMock);
#pragma warning disable NS1000 // Non-virtual setup specification.
        commandMock.Parameters.Returns(new FakeParameterCollection());
        commandMock.ExecuteReaderAsync().Returns(Substitute.For<DbDataReader>());
#pragma warning restore NS1000 // Non-virtual setup specification.

        var dr = await connectionManager.GetDataRowAsync("StoredProcedure", CommandType.StoredProcedure, 30, new List<SqlParameter>() { new SqlParameter("@parameter", "value") });

        Assert.IsNull(dr);
        Assert.AreEqual(CommandType.StoredProcedure, commandMock.CommandType);
        Assert.AreEqual(30, commandMock.CommandTimeout);
        Assert.AreEqual(1, commandMock.Parameters.Count);
#pragma warning disable NS1001 // Non-virtual setup specification.
        await commandMock.Received().ExecuteReaderAsync();
#pragma warning restore NS1001 // Non-virtual setup specification.
    }

    [TestMethod]
    public void ExecuteReaderBehaviour()
    {
        var loggerMock = Substitute.For<ILogger>();
        var factoryMock = Substitute.For<DbProviderFactory>();
        var connectionManager = new FakeConnectionManager(loggerMock, factoryMock);
        var commandMock = Substitute.For<DbCommand>();

        factoryMock.CreateCommand().Returns(commandMock);
#pragma warning disable NS1000 // Non-virtual setup specification.
        commandMock.Parameters.Returns(new FakeParameterCollection());
#pragma warning restore NS1000 // Non-virtual setup specification.

        Assert.ThrowsException<ArgumentException>(() => connectionManager.ExecuteReader("StoredProcedure", CommandType.StoredProcedure, 30, CommandBehavior.CloseConnection, new List<SqlParameter>() { new SqlParameter("@parameter", "value") }));
    }

    [TestMethod]
    public async Task ExecuteReaderBehaviourAsync()
    {
        var loggerMock = Substitute.For<ILogger>();
        var factoryMock = Substitute.For<DbProviderFactory>();
        var connectionManager = new FakeConnectionManager(loggerMock, factoryMock);
        var commandMock = Substitute.For<DbCommand>();

        factoryMock.CreateCommand().Returns(commandMock);
#pragma warning disable NS1000 // Non-virtual setup specification.
        commandMock.Parameters.Returns(new FakeParameterCollection());
#pragma warning restore NS1000 // Non-virtual setup specification.

        await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await connectionManager.ExecuteReaderAsync("StoredProcedure", CommandType.StoredProcedure, 30, CommandBehavior.CloseConnection, new List<SqlParameter>() { new SqlParameter("@parameter", "value") }));
    }

    [TestMethod]
    public void ExecuteReaderCommandTimeout()
    {
        var loggerMock = Substitute.For<ILogger>();
        var factoryMock = Substitute.For<DbProviderFactory>();
        var connectionManager = new FakeConnectionManager(loggerMock, factoryMock);
        var commandMock = Substitute.For<DbCommand>();

        factoryMock.CreateCommand().Returns(commandMock);
#pragma warning disable NS1000 // Non-virtual setup specification.
        commandMock.Parameters.Returns(new FakeParameterCollection());
#pragma warning restore NS1000 // Non-virtual setup specification.

        Assert.ThrowsException<ArgumentOutOfRangeException>(() => connectionManager.ExecuteReader("StoredProcedure", CommandType.StoredProcedure, -1, CommandBehavior.SingleResult, new List<SqlParameter>() { new SqlParameter("@parameter", "value") }));
    }

    [TestMethod]
    public async Task ExecuteReaderCommandTimeoutAsync()
    {
        var loggerMock = Substitute.For<ILogger>();
        var factoryMock = Substitute.For<DbProviderFactory>();
        var connectionManager = new FakeConnectionManager(loggerMock, factoryMock);
        var commandMock = Substitute.For<DbCommand>();

        factoryMock.CreateCommand().Returns(commandMock);
#pragma warning disable NS1000 // Non-virtual setup specification.
        commandMock.Parameters.Returns(new FakeParameterCollection());
#pragma warning restore NS1000 // Non-virtual setup specification.

        await Assert.ThrowsExceptionAsync<ArgumentOutOfRangeException>(async () => await connectionManager.ExecuteReaderAsync("StoredProcedure", CommandType.StoredProcedure, -1, CommandBehavior.SingleResult, new List<SqlParameter>() { new SqlParameter("@parameter", "value") }));
    }

    [TestMethod]
    public void ExecuteException()
    {
        var loggerMock = Substitute.For<ILogger>();
        var factoryMock = Substitute.For<DbProviderFactory>();
        var connectionManager = new FakeConnectionManager(loggerMock, factoryMock);
        var commandMock = Substitute.For<DbCommand>();

        factoryMock.CreateCommand().Returns(commandMock);
#pragma warning disable NS1000 // Non-virtual setup specification.
        commandMock.Parameters.Returns(new FakeParameterCollection());
#pragma warning restore NS1000 // Non-virtual setup specification.
        commandMock.ExecuteNonQuery().Throw(new TestException());

        Assert.ThrowsException<TestException>(() => connectionManager.ExecuteNonQuery("StoredProcedure"));
    }

    [TestMethod]
    public async Task ExecuteExceptionAsync()
    {
        var loggerMock = Substitute.For<ILogger>();
        var factoryMock = Substitute.For<DbProviderFactory>();
        var connectionManager = new FakeConnectionManager(loggerMock, factoryMock);
        var commandMock = Substitute.For<DbCommand>();

        factoryMock.CreateCommand().Returns(commandMock);
#pragma warning disable NS1000 // Non-virtual setup specification.
        commandMock.Parameters.Returns(new FakeParameterCollection());
#pragma warning restore NS1000 // Non-virtual setup specification.
        commandMock.ExecuteNonQueryAsync(Arg.Any<CancellationToken>()).ThrowAsync(new TestException());

        await Assert.ThrowsExceptionAsync<TestException>(async () => await connectionManager.ExecuteNonQueryAsync("StoredProcedure"));
    }
}